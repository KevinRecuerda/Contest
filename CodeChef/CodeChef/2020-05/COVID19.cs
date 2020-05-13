using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeChef._2020_05.COVID19
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
            var x = ConsoleHelper.ReadLineAndSplitAsListOf<int>();

            var groups = new List<List<int>>();
            var group = new List<int>();
            var previous = int.MinValue;
            foreach (var xi in x.OrderBy(v => v))
            {
                if (xi <= previous+2)
                    group.Add(xi);
                else
                {
                    group = new List<int>{xi};
                    groups.Add(group);
                }

                previous = xi;
            }

            var count = groups.Select(g => g.Count).ToList();
            var max = count.Max();
            var min = count.Min();

            ConsoleHelper.WriteLine($"{min} {max}");
        }
    }
}