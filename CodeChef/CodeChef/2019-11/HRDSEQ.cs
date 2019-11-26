using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeChef._2019_11.HRDSEQ
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
            var n = ConsoleHelper.ReadLineAs<int>();

            var sequence = BuildSequence(n);

            var x = sequence[n - 1];
            var count = sequence.Count(i => i == x);

            ConsoleHelper.WriteLine(count);
        }

        private static int[] BuildSequence(int n)
        {
            var sequence = new int[n];
            sequence[0] = 0;

            var lastNumbers = Enumerable.Range(1, n).Select(i => -1).ToArray();

            for (var i = 1; i < n; i++)
            {
                var last = sequence[i - 1];

                sequence[i] = 0;
                if (lastNumbers[last] != -1)
                    sequence[i] = i - lastNumbers[last];

                lastNumbers[last] = i - 1;
            }

            return sequence;
        }
    }
}