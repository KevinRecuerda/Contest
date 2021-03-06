﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeChef.October2019.SAKTAN
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
            var input = ConsoleHelper.ReadLineAndSplitAsListOf<int>();
            var n = input[0];
            var m = input[1];
            var q = input[2];

            var rows = new HashSet<int>();
            var cols = new HashSet<int>();
            for (var i = 0; i < q; i++)
            {
                input = ConsoleHelper.ReadLineAndSplitAsListOf<int>();
                var x = input[0];
                var y = input[1];

                if (rows.Contains(x))
                    rows.Remove(x);
                else
                    rows.Add(x);

                if (cols.Contains(y))
                    cols.Remove(y);
                else
                    cols.Add(y);
            }

            //var r = rows.Count * (m - cols.Count) + cols.Count * (n - rows.Count);
            var r = rows.Count * m + cols.Count * n - 2 * rows.Count * cols.Count;
            ConsoleHelper.WriteLine(r);
        }
    }
}
