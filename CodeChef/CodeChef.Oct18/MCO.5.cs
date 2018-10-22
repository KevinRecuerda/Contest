using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeChef.Oct18.MCO5
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
            var N = input[0];
            var M = input[1];
            var r = input[2]-1;
            var c = input[3]-1;
            var K = input[4];

            var nodes = new Node[N,M];
            for (var i = 0; i < N; i++)
            for (var j = 0; j < M; j++)
            {
                var node = new Node(j, i, M);
                nodes[i, j] = node;
            }

            nodes[r, c].Weight = 0;
            var queue = new Queue<Node>();
            queue.Enqueue(nodes[r, c]);

            var alreadyEnqueued = new HashSet<Node> {nodes[r, c]};

            while (queue.Count > 0)
            {
                var node = queue.Dequeue();
                if (node.Weight >= K)
                    continue;

                var i = node.Y;
                var j = node.X;
                while (i > 0 && j > 0)
                {
                    i--;
                    j--;

                    var toCheck = nodes[i, j];
                    if (!alreadyEnqueued.Contains(toCheck))
                    {
                        alreadyEnqueued.Add(toCheck);
                        queue.Enqueue(toCheck);
                        toCheck.Weight = node.Weight + 1;
                    }
                }

                i = node.Y;
                j = node.X;
                while (i < N-1 && j < M-1)
                {
                    i++;
                    j++;

                    var toCheck = nodes[i, j];
                    if (!alreadyEnqueued.Contains(toCheck))
                    {
                        alreadyEnqueued.Add(toCheck);
                        queue.Enqueue(toCheck);
                        toCheck.Weight = node.Weight + 1;
                    }
                }

                i = node.Y;
                j = node.X;
                while (i > 0 && j < M-1)
                {
                    i--;
                    j++;

                    var toCheck = nodes[i, j];
                    if (!alreadyEnqueued.Contains(toCheck))
                    {
                        alreadyEnqueued.Add(toCheck);
                        queue.Enqueue(toCheck);
                        toCheck.Weight = node.Weight + 1;
                    }
                }

                i = node.Y;
                j = node.X;
                while (i < N - 1 && j > 0)
                {
                    i++;
                    j--;

                    var toCheck = nodes[i, j];
                    if (!alreadyEnqueued.Contains(toCheck))
                    {
                        alreadyEnqueued.Add(toCheck);
                        queue.Enqueue(toCheck);
                        toCheck.Weight = node.Weight + 1;
                    }
                }
            }

            var count = alreadyEnqueued.Count(x => x.Weight <= K);
            ConsoleHelper.WriteLine(count);
        }

        public class Node
        {
            public Node(int x, int y, int m)
            {
                this.X = x;
                this.Y = y;

                this.Weight = int.MaxValue;
            }
            public int X { get; set; }
            public int Y { get; set; }

            public int Weight { get; set; }
        }
    }
}