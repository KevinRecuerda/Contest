using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace CodeJam.April18.Ex4
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
                var line = ConsoleHelper.ReadLine();
                var a = double.Parse(line, CultureInfo.InvariantCulture);

                var points = Solve(a);
                var res = string.Join("", points.Select(p => $"{Environment.NewLine}{p}"));
                ConsoleHelper.WriteLine($"Case #{i + 1}:{res}");
            }
        }

        public static List<Point> Solve(double a)
        {
            var l = a / Math.Sqrt(2);
            var alpha = Math.PI / 4 - Math.Acos(l);
            var cos = Math.Cos(alpha) / 2;
            var sin = Math.Cos(alpha) / 2;
            return new List<Point>()
            {
                new Point(cos, sin, 0),
                new Point(-sin, cos, 0),
                new Point(0, 0, 0.5),
            };
        }

        public class Point
        {
            public double X { get; set; }
            public double Y { get; set; }
            public double Z { get; set; }

            public Point(double x, double y, double z)
            {
                X = x;
                Y = y;
                Z = z;
            }

            public override string ToString()
            {
                return $"{DisplayNumber(X)} {DisplayNumber(Y)} {DisplayNumber(Z)}";
            }

            private static string DisplayNumber(double number)
            {
                return number.ToString("F10", CultureInfo.InvariantCulture);
            }
        }
    }
}
