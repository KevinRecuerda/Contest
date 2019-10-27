using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Linq;

namespace CodeChef.Feb19.MANRECT
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

        public static long Max = (long)Math.Pow(10, 9);

        private static void Solve()
        {
            var d0 = AskDist(0, 0);
            var d1 = AskDist(Max, 0);
            var delta = d1 - (Max - d0);
            delta = delta > 0 ? delta / 2 + delta % 2 : 0;
            var d2 = AskDist(d0 - delta, delta);

            var xl = d0 - delta - d2;
            var yl = delta + d2;

            var dx = AskDist(Max, yl);
            var dy = AskDist(xl, Max);

            var xu = Max - dx;
            var yu = Max - dy;

            ConsoleHelper.WriteLine($"A {xl} {yl} {xu} {yu}");
            var result = ConsoleHelper.ReadLineAs<long>();
            if (result != 1)
                throw new Exception("wrong solution");
        }

        private static long AskDist(long x, long y)
        {
            ConsoleHelper.WriteLine($"Q {x} {y}");
            return ConsoleHelper.ReadLineAs<long>();
        }
    }
}