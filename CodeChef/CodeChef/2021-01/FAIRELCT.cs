using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeChef._2021_01.FAIRELCT
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
            var input = ConsoleHelper.ReadLineAndSplitAsListOf<long>();
            var n = input[0];
            var m = input[1];

            var a = ConsoleHelper.ReadLineAndSplitAsListOf<long>().OrderBy(x => x).ToArray();
            var b = ConsoleHelper.ReadLineAndSplitAsListOf<long>().OrderByDescending(x => x).ToArray();

            var res = FindMinimalSwap(a, b);
            ConsoleHelper.WriteLine(res);
        }

        private static int FindMinimalSwap(long[] a, long[] b)
        {
            var sumA = a.Sum();
            var sumB = b.Sum();
            if (sumA > sumB)
                return 0;

            for (var i = 0; i < Math.Min(a.Length, b.Length); i++)
            {
                var delta = b[i] - a[i];
                if (delta <= 0)
                    return -1;

                sumA += delta;
                sumB -= delta;

                if (sumA > sumB)
                    return i + 1;
            }

            return -1;
        }
    }
}