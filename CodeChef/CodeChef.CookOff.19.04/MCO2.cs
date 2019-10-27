using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace CodeChef.April19.MCO2
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
            var zeroColIndex = new List<int>();
            var zeroRowIndex = new List<int>();

            for (var i = 0; i < n; i++)
            {
                var row = ConsoleHelper.ReadLineAndSplitAsListOf<int>();
                for (var j = 0; j < n; j++)
                {
                    if (row[j] == 0)
                    {
                        zeroColIndex.Add(j);
                        zeroRowIndex.Add(i);
                    }
                }
            }

            var result = zeroRowIndex.Distinct().Count() == n && zeroColIndex.Distinct().Count() == n
                ? "YES"
                : "NO";

            ConsoleHelper.WriteLine(result);
        }
    }
}