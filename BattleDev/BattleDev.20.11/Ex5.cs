using System;
using System.Collections.Generic;
using System.Linq;

namespace BattleDev._2020_11.Ex5
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
            Solve();
        }

        public static void Solve()
        {
            var pseudo = ConsoleHelper.ReadLine();
            var n = pseudo.Length;

            var MIN = 33;
            var MAX = 126;
            var DIFF = 31;
            var hash = pseudo.ToCharArray().Select(c => (int)c).ToArray();
            for (var i = n-1; i > 0; i--)
            {
                var x = hash[i];
                if (x - DIFF >= MIN)
                {
                    hash[i] -= 31;
                    hash[i - 1] += 1;
                    break;
                }
                else if (x + DIFF <= MAX)
                {
                    hash[i] += 31;
                    hash[i - 1] -= 1;
                    break;
                }
            }

            ConsoleHelper.WriteLine(string.Join("", hash.Select(x => (char)x)));
        }
    }
}