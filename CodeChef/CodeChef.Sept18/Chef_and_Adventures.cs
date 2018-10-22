using System;
using System.Collections.Generic;
using System.Linq;

namespace Contest.Chef_and_Adventures
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
            var t = ConsoleHelper.ReadLineAs<int>();
            for (var k = 0; k < t; k++)
            {
                var input = ConsoleHelper.ReadLineAndSplitAsListOf<int>();
                var n = input[0] - 1;
                var m = input[1] - 1;
                var x = input[2];
                var y = input[3];

                var canSolve = Solve(n, m, x, y) || Solve(n-1, m-1, x, y);
                var result = canSolve
                    ? "Chefirnemo"
                    : "Pofik";
                ConsoleHelper.WriteLine(result);
            }
        }

        private static bool Solve(int n, int m, int x, int y)
        {
            if (n < 0 || m < 0)
                return false;

            if (n == 0 && m == 0)
                return true;

            var r1 = n % x;
            var r2 = m % y;
            return r1 == 0 && r2 == 0;
        }
    }
}
