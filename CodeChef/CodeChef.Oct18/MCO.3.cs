﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeChef.Oct18.MCO3
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
            var h = ConsoleHelper.ReadLineAndSplitAsListOf<int>().ToArray();

            var left = 0;
            var right = n - 1;
            var reservoirCount = Solve(left, right, h);

            ConsoleHelper.WriteLine(reservoirCount);
        }

        public static int Solve(int left, int right, int[] h)
        {
            if (left > right)
                return 0;

            var max = int.MinValue;
            var maxIndex = -1;
            for (var k = left; k <= right; k++)
            {
                if (h[k] > max)
                {
                    max = h[k];
                    maxIndex = k;
                }
            }

            var reservoirCount = 1;

            if (left < maxIndex && maxIndex < right)
            {
                var subRight = Solve(maxIndex + 1, right, h);
                var subLeft = Solve(left, maxIndex - 1, h);
                reservoirCount += Math.Min(subRight, subLeft);
            }

            return reservoirCount;
        }
    }
}