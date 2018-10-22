using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeChef.Oct18.CCIRCLES
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
            var input = ConsoleHelper.ReadLineAndSplitAsListOf<int>();
            var n = input[0];
            var q = input[1];

            var circles = new List<Circle>();

            var min = new List<double>();
            var max = new List<double>();

            for (var i = 0; i < n; i++)
            {
                input = ConsoleHelper.ReadLineAndSplitAsListOf<int>();
                var circle = new Circle(input[0], input[1], input[2]);

                foreach (var other in circles)
                {
                    var pair = new Pair(other, circle);
                    min.Add(pair.Min);
                    max.Add(pair.Max);
                }

                circles.Add(circle);
            }
            min = min.OrderBy(x => x).ToList();
            max = max.OrderBy(x => x).ToList();

            var queries = new Query[q];
            for (var i = 0; i < q; i++)
            {
                var k = ConsoleHelper.ReadLineAs<int>();
                var query = new Query(k, i);
                queries[i] = query;
            }
            queries = queries.OrderBy(query => query.K).ToArray();

            var minIndex = 0;
            var maxIndex = 0;

            var goodPair = 0;
            var results = new int[q];

            for (var i = 0; i < q; i++)
            {
                var query = queries[i];

                while (minIndex < min.Count && min[minIndex] <= query.K)
                {
                    minIndex++;
                    goodPair++;
                }

                while (maxIndex < max.Count && max[maxIndex] < query.K)
                {
                    maxIndex++;
                    goodPair--;
                }

                results[query.I] = goodPair;
            }

            for (var i = 0; i < q; i++)
                ConsoleHelper.WriteLine(results[i]);
        }

        public class Query
        {
            public Query(int k, int i)
            {
                K = k;
                I = i;
            }

            public int K { get; set; }
            public int I { get; set; }
        }

        public class Circle
        {
            public Circle(int x, int y, int r)
            {
                X = x;
                Y = y;
                R = r;
            }

            public int X { get; set; }
            public int Y { get; set; }

            public int R { get; set; }
        }

        public class Pair
        {
            public Pair(Circle c1, Circle c2)
            {
                Circle1 = c1;
                Circle2 = c2;

                this.Distance = Math.Sqrt(Math.Pow(c1.X - c2.X, 2) + Math.Pow(c1.Y - c2.Y, 2));

                this.Max = this.Distance + c1.R + c2.R;

                //var maxR = Math.Max(c1.R, c2.R);
                //this.Min = 2 * Math.Max(this.Distance, maxR) - this.Max;
                //if (this.Min < 0)
                //    this.Min = 0;

                // exclude
                if (this.Distance > c1.R + c2.R)
                {
                    this.Min = this.Distance - c1.R - c2.R;
                }
                // c1 in c2
                else if (this.Distance + c1.R < c2.R)
                {
                    this.Min = c2.R - c1.R - this.Distance;
                }
                // c1 in c2
                else if (this.Distance + c2.R < c1.R)
                {
                    this.Min = c1.R - c2.R - this.Distance;
                }
                // intersect
                else
                {
                    this.Min = 0;
                }
            }

            public Circle Circle1 { get; set; }
            public Circle Circle2 { get; set; }

            public double Distance { get; set; }

            public double Min { get; set; }
            public double Max { get; set; }
        }
    }
}