using System;
using System.Collections.Generic;
using System.Linq;

namespace BattleDev.Mar18.Ex3
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
            Solve();
        }

        public static void Solve()
        {
            var n = ConsoleHelper.ReadLineAs<int>();

            var map = new char[n][];
            for (var i = 0; i < n; i++)
            {
                var line = ConsoleHelper.ReadLine();
                map[i] = line.ToCharArray();
            }

            var move = "";
            var isRight = true;
            for (var i = 0; i < n; i++)
            {
                for (var j = 0; j < n; j++)
                {
                    if (map[i][isRight ? j : n - 1 - j] == 'o')
                        move += "x";

                    if (j < n - 1)
                        move += isRight ? ">" : "<";
                    else if (i < n-1)
                        move += 'v';
                }
                isRight = !isRight;
            }

            isRight = n%2 == 0;
            for (var i = n-1; i >= 0; i--)
            {
                for (var j = 0; j < n; j++)
                {
                    if (map[i][isRight ? j : n - 1 - j] == '*')
                        move += "x";

                    if (j < n - 1)
                        move += isRight ? ">" : "<";
                    else if (i > 0)
                        move += '^';
                }

                isRight = !isRight;
            }

            ConsoleHelper.WriteLine(move);
        }
    }
}
