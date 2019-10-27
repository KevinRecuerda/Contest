using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace CodeChef.Jan19.DIFNEIGH
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
            var size = ConsoleHelper.ReadLineAndSplitAsListOf<int>();
            var n = size[0];
            var m = size[1];

            var k = InRange((n * m) / 2, 1, 4);

            var pattern1 = "1 2 3 4 ";
            var pattern2 = "3 4 1 2 ";

            if (k < 4)
                pattern2 = pattern2.Replace("4", "3");

            var line1 = (pattern1.Repeat(m / 4) + pattern1.Substring(0, 2 * (m % 4))).RemoveLastChar();
            var line2 = (pattern2.Repeat(m / 4) + pattern2.Substring(0, 2 * (m % 4))).RemoveLastChar();

            var result = Enumerable.Range(0, n).Select(i => i % 4 < 2 ? line1 : line2).ToList();
            result.Insert(0, k.ToString());

            ConsoleHelper.WriteLine(string.Join(Environment.NewLine, result));
        }

        private static string Repeat(this string s, int n)
        {
            if (n <= 0)
                return string.Empty;

            return new StringBuilder(s.Length * n).Insert(0, s, n).ToString();
        }

        private static string RemoveLastChar(this string s)
        {
            return s.Remove(s.Length - 1);
        }

        private static int InRange(int value, int min, int max)
        {
            if (value <= min)
                return min;

            if (value >= max)
                return max;

            return value;
        }
    }
}