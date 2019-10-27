using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;

namespace CodeChef.Feb19.GUESSRT
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
            return (T) Convert.ChangeType(value, typeof(T), CultureInfo.InvariantCulture);
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

        private static void Solve()
        {
            var input = ConsoleHelper.ReadLineAndSplitAsListOf<long>();
            var n = input[0];
            var k = input[1];
            var m = input[2];

            var r = n;
            while (r > k)
                r -= k;

            var pur1 = 1L;
            var pur2 = 1L;
            for (var i = 0; i <= m / 2; i++)
                pur2 *= r;

            var probaUp = 0L;
            var probaDown = n * pur2;

            for (var i = 0; i <= m / 2; i++)
            {
                probaUp += pur1 * pur2;
                pur1 *= (r - 1);
                pur2 /= r;
            }

            var gcd = Gcd(probaUp, probaDown);
            probaUp /= gcd;
            probaDown /= gcd;

            ConsoleHelper.WriteLine(probaUp);
            ConsoleHelper.WriteLine(probaDown);
        }

        private static long Gcd(long a, long b)
        {
            while (a != 0 && b != 0)
            {
                if (a > b)
                    a %= b;
                else
                    b %= a;
            }

            return a == 0 ? b : a;
        }

        public static void Test()
        {
            var box = 5;
            var add = 5;
            var count = 10;

            var start = new Node()
            {
                BoxCount = box,
                Proba = 1,
                ProbaN = 1,
                Depth = 0
            };
            var nodes = Discover(start, add, count).OrderByDescending(n => n.Proba).ToList();
            ;
        }

        public static ICollection<Node> Discover(Node node, int boxToAdd, int maxDepth)
        {
            if (node.Depth == maxDepth)
                return new [] {node};

            var nodes = new List<Node>();

            // Peek
            var peek = node.CreateChild();
            peek.Choice = "P";
            peek.Proba += peek.ProbaN / peek.BoxCount;
            peek.ProbaN *= (peek.BoxCount - 1.0) / peek.BoxCount;
            peek.BoxCount += boxToAdd;
            var pNodes = Discover(peek, boxToAdd, maxDepth);
            nodes.AddRange(pNodes);

            // Remove
            if (node.BoxCount > boxToAdd)
            {
                var remove = node.CreateChild();
                remove.Choice = "R";
                while (remove.BoxCount > boxToAdd)
                    remove.BoxCount -= boxToAdd;
                var rNodes = Discover(remove, boxToAdd, maxDepth);
                nodes.AddRange(rNodes);
            }

            return nodes;
        }

        public class Node
        {
            public Node Parent { get; set; }

            public int BoxCount { get; set; }

            public double Proba { get; set; }
            public double ProbaN { get; set; }

            public int Depth { get; set; }

            public string Choice { get; set; }

            public Node CreateChild()
            {
                return new Node
                {
                    Parent = this,
                    BoxCount = this.BoxCount,
                    Proba = this.Proba,
                    ProbaN = this.ProbaN,
                    Depth = this.Depth + 1
                };
            }

            public override string ToString()
            {
                return $"{this.Parent} {this.Choice}";
            }
        }
    }
}