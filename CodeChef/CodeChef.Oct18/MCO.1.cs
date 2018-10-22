using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeChef.Oct18.MCO1
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
            var input = ConsoleHelper.ReadLineAndSplit();
            var n = int.Parse(input[0]);
            var s = input[1];

            var deeRounds = 0;
            var dumRounds = 0;
            for (var i = 0; i < n; i++)
            {
                var stack = ConsoleHelper.ReadLine();
                if (stack.Length == 0)
                    continue;

                var last = 'x';
                foreach (var current in stack)
                {
                    if (last == current)
                        continue;

                    if (current == '0')
                        deeRounds++;
                    else
                        dumRounds++;

                    last = current;
                }
            }

            var winner = (deeRounds < dumRounds || deeRounds == dumRounds && s == "Dee")
                ? "Dee"
                : "Dum";
            ConsoleHelper.WriteLine(winner);
        }
    }
}