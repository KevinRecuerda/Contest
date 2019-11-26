using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeChef._2019_11.PHCUL
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
            var input = ConsoleHelper.ReadLineAndSplitAsListOf<int>();
            var x = input[0];
            var y = input[1];
            var start = new Point(x, y);

            input = ConsoleHelper.ReadLineAndSplitAsListOf<int>();
            var n = input[0];
            var m = input[1];
            var k = input[2];

            var a = BuildPoints(n);
            var c = BuildPoints(m);
            var e = BuildPoints(k);

            var min1 = CalculateMinDist(start, a, c, e);
            var min2 = CalculateMinDist(start, c, a, e);

            var min = Math.Min(min1, min2);

            ConsoleHelper.WriteLine(min);
        }

        private static double CalculateMinDist(Point start, Point[] inter, Point[] rotate, Point[] end)
        {
            var min = double.MaxValue;
            foreach (var r in rotate.Select(r => new {Point=r, D=start.CalculateDistance(r)}).OrderBy(r => r.D))
            {
                if (r.D >= min)
                    continue;

                // dist1 = start - inter - r
                var dist1 = inter.Min(x => start.CalculateDistance(x) + x.CalculateDistance(r.Point));
                // dist2 = r - end
                var dist2 = end.Min(x => r.Point.CalculateDistance(x));

                var dist = dist1 + dist2;
                if (dist < min)
                    min = dist;
            }

            return min;
        }

        private static Point[] BuildPoints(int size)
        {
            var input = ConsoleHelper.ReadLineAndSplitAsListOf<int>();

            var points = new Point[size];
            for (var i = 0; i < size; i++)
            {
                var x = input[2 * i];
                var y = input[2 * i + 1];
                var point = new Point(x, y);
                points[i] = point;
            }

            return points;
        }
    }

    public class Point
    {
        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X { get; set; }
        public int Y { get; set; }

        public double CalculateDistance(Point other)
        {
            return Math.Sqrt(Math.Pow(this.X - other.X, 2) + Math.Pow(this.Y - other.Y, 2));
        }
    }
}