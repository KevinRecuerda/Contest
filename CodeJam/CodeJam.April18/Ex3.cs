using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeJam.April18.Ex3
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
            var t = ConsoleHelper.ReadLineAs<int>();
            for (var i = 0; i < t; i++)
            {
                var a = ConsoleHelper.ReadLineAs<int>();
                Solve(a);
            }
        }

        public static void Solve(int a)
        {
            var done = false;
            var map = new bool[1000, 1000];

            var h = 1;
            var l = 1;

            // First line
            Console.WriteLine("500 500");
            var line = ConsoleHelper.ReadLineAndSplitAsListOf<int>();
            var x = line[0];
            var y = line[1];
            map[y, x] = true;

            var rectangle = new Rectangle()
            {
                X = x,
                Y = y,
                H = 1,
                L = 1
            };

            while (!done)
            {
                // Choose cell
                // 1: 500

                // Read real prepared cell
                line = ConsoleHelper.ReadLineAndSplitAsListOf<int>();
                x = line[0];
                y = line[1];
                if (x == 0 && y == 0)
                    return;
                if (x == -1 && y == -1)
                    return;

                map[y, x] = true;
            }
        }

        public class Rectangle
        {
            public int X { get; set; }
            public int Y { get; set; }

            public int H { get; set; }
            public int L { get; set; }

        }
    }
}
