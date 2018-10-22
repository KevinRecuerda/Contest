using System;
using System.Collections.Generic;
using System.Linq;

namespace BattleDev.March18.Ex4
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
            var line = this.ReadLine();

            return ConvertTo<T>(line);
        }

        public string[] ReadLineAndSplit(char delimiter = ' ')
        {
            var splittedLine = this.ReadLine().Split(delimiter);
            return splittedLine;
        }

        public List<T> ReadLineAndSplitAsListOf<T>(char delimiter = ' ')
        {
            var splittedLine = this.ReadLineAndSplit();

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
            return (T)Convert.ChangeType(value, typeof(T));
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
            Solve();
        }

        public static void Solve()
        {
            var sizes = new int[6];
            for (var i = 0; i < 6; i++)
            {
                var size = ConsoleHelper.ReadLineAs<int>();
                sizes[5-i] = size;
            }

            var cost = GetMinimalCost(sizes, 0);
            ConsoleHelper.WriteLine(cost);
        }

        public static int GetMinimalCost(int[] sizes, int depth)
        {
            if (IsOrdered(sizes))
                return depth;

            if (depth == 7)
                return int.MaxValue;

            var min = int.MaxValue;
            for (var i = 0; i < 5; i++)
            {
                var permut = Permut(sizes, i);
                var cost = GetMinimalCost(permut, depth + 1);
                min = Math.Min(min, cost);
            }

            return min;
        }

        public static bool IsOrdered(int[] sizes)
        {
            return sizes[0] > sizes[1]
                   && sizes[1] > sizes[2]
                   && sizes[2] > sizes[3]
                   && sizes[3] > sizes[4]
                   && sizes[4] > sizes[5];
        }

        public static int[] Permut(int[] sizes, int index)
        {
            var permut = new int[6];
            for (var i = 0; i < index; i++)
                permut[i] = sizes[i];

            for (var i = index; i < 6; i++)
                permut[i] = sizes[5 - i + index];

            return permut;
        }
    }
}
