using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace CodeChef.Feb19.DEPCHEF
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
            return (T) Convert.ChangeType(value, typeof(T), CultureInfo.InvariantCulture);
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

        private static void Solve()
        {
            var n = ConsoleHelper.ReadLineAs<int>();

            var a = ConsoleHelper.ReadLineAndSplitAsListOf<int>();
            var d = ConsoleHelper.ReadLineAndSplitAsListOf<int>();

            var maxDef = -1;

            for (var i = 0; i < n; i++)
            {
                var isAlive = d[i] > a[PositiveMod(i - 1, n)] + a[PositiveMod(i + 1, n)];
                if (isAlive && d[i] > maxDef)
                    maxDef = d[i];
            }

            ConsoleHelper.WriteLine(maxDef);
        }

        private static int PositiveMod(int value, int mod)
        {
            return ((value%mod) + mod) % mod;
        }
    }
}