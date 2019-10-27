using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeChef.October2019.MARM
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
            var k = input[1];

            var values = ConsoleHelper.ReadLineAndSplitAsListOf<long>().ToArray();

            for (var i = 0; i < n / 2; i++)
            {
                var n_i = n - 1 - i;

                var repeat = (k - 1) / n;
                var mod = k - repeat * n;

                var repeatA = repeat + (i < mod ? 1 : 0);
                var repeatB = repeat + (n_i < mod ? 1 : 0) + 2;

                var a = values[i];
                var b = values[n_i];

                values[i] = Apply(a, b, repeatA);
                values[n_i] = Apply(a, b, repeatB);
            }

            if (n % 2 == 1 && n/2 < k)
                values[n / 2] = 0;

            ConsoleHelper.WriteLine(string.Join(" ", values));
        }

        public static long Apply(long a, long b, long repeat)
        {
            switch (repeat % 3)
            {
                case 0: return a;
                case 1: return a ^ b;
                case 2: return b;
                default: throw new Exception();
            }
        }
    }
}