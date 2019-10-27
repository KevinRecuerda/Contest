using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace CodeChef.April19.MCO4
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
            var size = ConsoleHelper.ReadLineAndSplitAsListOf<int>();
            var n = size[0];
            var m = size[1];

            var nodes = ConsoleHelper.ReadLineAndSplitAsListOf<long>().Select(x => new Node(x)).ToArray();

            for (var i = 0; i < m; i++)
            {
                var input = ConsoleHelper.ReadLineAndSplitAsListOf<int>();
                var a = nodes[input[0]-1];
                var b = nodes[input[1]-1];
                if (a.Value > b.Value)
                    b.HigherNodes.Add(a);
                else if (b.Value > a.Value)
                    a.HigherNodes.Add(b);
            }

            var length = FindLength(nodes, n);

            ConsoleHelper.WriteLine(length);
        }

        private static int FindLength(Node[] nodes, int n)
        {
            var maxLength = -1;
            for (var i = 0; i < n; i++)
            {
                var node = nodes[i];
                if (!node.AlreadyVisited)
                    node.Visit(0);

                if (node.MaxLength > maxLength)
                    maxLength = node.MaxLength;
            }

            return maxLength + 1;
        }

        public class Node
        {
            public long Value { get; set; }

            public List<Node> HigherNodes { get; set; }

            public Dictionary<long, int> MaxLengthByDelta { get; set; }

            public int MaxLength { get; set; }

            public bool AlreadyVisited { get; set; }

            public Node(long value)
            {
                this.Value = value;
                this.HigherNodes = new List<Node>();
                this.MaxLengthByDelta = new Dictionary<long, int>();
                this.AlreadyVisited = false;
            }

            public int Visit(long delta)
            {
                if (!this.AlreadyVisited)
                {
                    foreach (var higherNode in HigherNodes)
                    {
                        var subDelta = higherNode.Value - this.Value;
                        var subMaxLength = 1 + higherNode.Visit(subDelta);
                        if (!this.MaxLengthByDelta.ContainsKey(subDelta) || this.MaxLengthByDelta[subDelta] < subMaxLength)
                            this.MaxLengthByDelta[subDelta] = subMaxLength;
                    }

                    this.MaxLength = this.MaxLengthByDelta.Count > 0 ? this.MaxLengthByDelta.Values.Max() : 0;

                    this.AlreadyVisited = true;
                }

                var maxLengths = this.MaxLengthByDelta.Where(x => delta < x.Key).Select(x => x.Value).ToList();
                return maxLengths.Count > 0 ? maxLengths.Max() : 0;
            }
        }
    }
}