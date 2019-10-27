using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace CodeChef.Jan19.DPAIRS
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
            Solve();
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
            var size = ConsoleHelper.ReadLineAndSplitAsListOf<int>();
            var n = size[0];
            var m = size[1];

            var a = ConsoleHelper.ReadLineAndSplitAsListOf<long>();
            var b = ConsoleHelper.ReadLineAndSplitAsListOf<long>();

            var a_min = a.Min();
            var b_max = b.Max();

            var a_min_index = a.IndexOf(a_min);
            var b_max_index = b.IndexOf(b_max);

            var verticalLine = Enumerable.Range(0, m).Select(x => $"{a_min_index} {x}");
            var horizontalLine = Enumerable.Range(0, n).Where(x => x != a_min_index).Select(x => $"{x} {b_max_index}");
            var result = verticalLine.Union(horizontalLine);

            ConsoleHelper.WriteLine(string.Join(Environment.NewLine, result));
        }
    }
}