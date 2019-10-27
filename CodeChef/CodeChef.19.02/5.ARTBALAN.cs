using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace CodeChef.Feb19.ARTBALAN
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
            var s = ConsoleHelper.ReadLine();

            var charCountDico = s.ToCharArray().GroupBy(c => c).ToDictionary(c => c.Key, c => c.Count());
            var charCount = charCountDico.Values.ToList();
            while (charCount.Count < 26)
                charCount.Add(0);

            charCount = charCount.OrderByDescending(v => v).ToList();

            var min = int.MaxValue;
            for (var i = 1; i <= 26; i++)
            {
                if (s.Length % i != 0)
                    continue;

                var average = s.Length / i;
                var dist = charCount.Take(i).Sum(c => Math.Abs(c - average))
                         + charCount.Skip(i).Sum();

                var opCount = dist / 2;
                if (opCount < min)
                    min = opCount;
            }

            ConsoleHelper.WriteLine(min);
        }
    }
}