using System;
using System.Collections.Generic;
using System.Linq;

namespace Contest.MDF18.Ex3
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
            var group = new int[n][];
            var riveIndex = 1;

            var rive1 = new List<Position>();
            var rive2 = new List<Position>();
            for (var i = 0; i < n; i++)
            {
                map[i] = ConsoleHelper.ReadLine().ToCharArray();
                group[i] = new int[n];
                for (var j = 0; j < n; j++)
                {
                    if (map[i][j] == '.')
                        group[i][j] = 0;
                    else
                    {
                        var top = -1;
                        if (i > 0)
                            top = group[i - 1][j];

                        var left = -1;
                        if (j > 0)
                            left = group[i][j-1];

                        var max = Math.Max(top, left);
                        if (max <= 0)
                            max = riveIndex++;

                        group[i][j] = max;

                        if (left > 0 && left != max)
                            Merge(group, max, left);
                        if (top > 0 && top != max)
                            Merge(group, max, top);
                    }
                }
            }

            var res = int.MaxValue;
            foreach (var pos1 in rive1)
            {
                foreach (var pos2 in rive2)
                {
                    var dist = Dist(pos1, pos2);
                    if (dist < res)
                        res = dist;
                }
            }

            Console.WriteLine(res);
        }

        public static void Merge(int[][] group, int idToKeep, int idToRemove)
        {
            for (var i = 0; i < group.Length; i++)
            for (var j = 0; j < group.Length; j++)
                if (group[i][j] == idToRemove)
                    group[i][j] = idToKeep;
        }

        public static int Dist(Position pos1, Position pos2)
        {
            var real =  Math.Sqrt(Math.Pow(pos1.X - pos2.X, 2) + Math.Pow(pos1.Y - pos2.Y, 2));

            return (int)Math.Ceiling(real);
        }

        public class Position
        {
            public int X { get; set; }
            public int Y { get; set; }

            public Position(int x, int y)
            {
                this.X = x;
                this.Y = y;
            }
        }
    }
}
