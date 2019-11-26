using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeChef._2019_11.CAMC
{
    #region ConsoleHelper

    public interface IConsoleHelper
    {
        string ReadLine();
        T ReadLineAs<T>();

        string[] ReadLineAndSplit(char delimiter = ' ');
        List<T> ReadLineAndSplitAsListOf<T>(char delimiter = ' ');

        void WriteLine(object obj);
        void Debug(object obj);
    }

    public class ConsoleHelper : IConsoleHelper
    {
        public virtual string ReadLine()
        {
            return Console.ReadLine();
        }

        public T ReadLineAs<T>()
        {
            var line = ReadLine();

            return ConvertTo<T>(line);
        }

        public string[] ReadLineAndSplit(char delimiter = ' ')
        {
            var splittedLine = ReadLine().Split(delimiter);
            return splittedLine;
        }

        public List<T> ReadLineAndSplitAsListOf<T>(char delimiter = ' ')
        {
            var splittedLine = ReadLineAndSplit();

            return splittedLine.Select(ConvertTo<T>).ToList();
        }

        public virtual void WriteLine(object obj)
        {
            Console.WriteLine(obj);
        }

        public void Debug(object obj)
        {
            Console.Error.WriteLine(obj);
        }

        private static T ConvertTo<T>(string value)
        {
            return (T) Convert.ChangeType(value, typeof(T));
        }
    }

    #endregion

    public static class Program
    {
        public static IConsoleHelper ConsoleHelper;

        static Program()
        {
            ConsoleHelper = new ConsoleHelper();
        }

        public static void Main(string[] args)
        {
            SolveMultiple();
        }

        public static void SolveMultiple()
        {
            var t = ConsoleHelper.ReadLineAs<int>();
            for (var k = 0; k < t; k++)
            {
                Solve();
            }
        }

        public static void Solve()
        {
            var input = ConsoleHelper.ReadLineAndSplitAsListOf<int>();
            var n = input[0];
            var m = input[1];

            var items = ConsoleHelper.ReadLineAndSplitAsListOf<int>().ToArray();

            var sortedItems = items.Select((x, i) => new {value = x, key = i % m}).OrderBy(x => x.value).ToList();

            var queues = sortedItems.GroupBy(x => x.key)
                .OrderBy(x => x.Key)
                .Select(x => new Queue<int>(x.Select(y => y.value).OrderBy(y => y).ToList()))
                .ToArray();

            var min = sortedItems[0].value;
            var max = queues.Select(g => g.Dequeue()).Max();
            var diff = max - min;

            for (var i = 0; i < n - 1; i++)
            {
                var key = sortedItems[i].key;
                if (queues[key].Count == 0)
                    break;

                var value = queues[key].Dequeue();
                if (value > max)
                    max = value;

                min = sortedItems[i + 1].value;

                if (diff > max - min)
                    diff = max - min;
            }

            ConsoleHelper.WriteLine(diff);
        }

        public static void SolveConsecutive()
        {
            var input = ConsoleHelper.ReadLineAndSplitAsListOf<int>();
            var n = input[0];
            var m = input[1];

            var array = ConsoleHelper.ReadLineAndSplitAsListOf<int>().ToArray();

            var max = new int[n];
            var min = new int[n];
            var diff = new int[n];

            var maxIndexes = new List<int>();
            var minIndexes = new List<int>();

            for (var i = 0; i < n + m; i++)
            {
                var idx = i % n;

                // dequeue
                maxIndexes.Remove(i - m);
                minIndexes.Remove(i - m);

                // enqueue
                Add(idx, array, maxIndexes, (current, localMax) => current >= localMax);
                Add(idx, array, minIndexes, (current, localMin) => current <= localMin);

                // set
                max[idx] = array[maxIndexes[0]];
                min[idx] = array[minIndexes[0]];
                diff[idx] = max[idx] - min[idx];
            }

            ConsoleHelper.WriteLine(diff.Min());
        }

        private static void Add(int i, int[] array, List<int> indexes, Func<int, int, bool> comparator)
        {
            var idx = indexes.Count;
            for (var k = 0; k < indexes.Count; k++)
            {
                if (comparator(array[i], array[indexes[k]]))
                {
                    idx = k;
                    break;
                }
            }

            indexes.Insert(idx++, i);
            indexes.RemoveRange(idx, indexes.Count - idx);
        }
    }
}