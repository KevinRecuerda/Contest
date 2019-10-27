using System;
using System.Collections.Generic;
using System.Linq;

namespace BattleDev.Nov18.Ex3
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
            var numbers = ConsoleHelper.ReadLineAndSplitAsListOf<int>();

            var x = n / 2.0;
            var infinityFound = false;
            var count = numbers[0] == x ? 1 : 0;
            for (var i = 0; i < n; i++)
            {
                if (x == numbers[i] && x == numbers[i + 1])
                {
                    infinityFound = true;
                    break;
                }

                if (numbers[i] < x && x <= numbers[i + 1]
                    || numbers[i] > x && x >= numbers[i+1])
                    count++;
            }

            var res = infinityFound?"INF": count.ToString();
            ConsoleHelper.WriteLine(res);
        }
    }
}
