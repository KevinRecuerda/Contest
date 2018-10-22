using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeChef.Oct18.MINDSUM
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

        private static void Solve()
        {
            var input = ConsoleHelper.ReadLineAndSplitAsListOf<long>();
            var n = input[0];
            var d = input[1];

            var min = Solve(n, d);
            ConsoleHelper.WriteLine(min);
        }

        private static Node Solve(long n, long d)
        {
            var firstNode = new Node(n, 0, 0);

            var nodes = new HashSet<Node> { firstNode };

            var queue = new Queue<Node>();
            queue.Enqueue(firstNode);

            while (queue.Count > 0)
            {
                var node = queue.Dequeue();
                if (node.Value == 1)
                    return node;

                var op1 = new Node(ToDigitsSum(node.Value), node.OpCount + 1, node.AddOpCount);
                if(!nodes.Contains(op1))
                {
                    nodes.Add(op1);
                    queue.Enqueue(op1);
                }

                var op2 = new Node(node.Value + d, node.OpCount + 1, node.AddOpCount+1);
                if (op2.AddOpCount < 9 && !nodes.Contains(op2))
                {
                    nodes.Add(op2);
                    queue.Enqueue(op2);
                }
            }

            var min = nodes.Min();
            return min;
        }

        private static long ToDigitsSum(long number)
        {
            var sum = number.ToString().Sum(c => c - '0');
            return sum;
        }
    }

    public class Node : IComparable
    {
        public Node(long value, int opCount, int addOpCount)
        {
            Value = value;
            OpCount = opCount;
            AddOpCount = addOpCount;
        }

        public long Value { get; set; }
        public int OpCount { get; set; }
        public int AddOpCount { get; set; }

        public override bool Equals(object obj)
        {
            var node = obj as Node;
            return node != null && Value == node.Value;
        }

        public override int GetHashCode()
        {
            return -1937169414 + Value.GetHashCode();
        }

        public int CompareTo(object obj)
        {
            var node = (Node)obj;
            var compareTo = this.Value.CompareTo(node.Value);
            if (compareTo == 0)
                compareTo = this.OpCount.CompareTo(node.OpCount);
            return compareTo;
        }

        public override string ToString()
        {
            return $"{Value} {OpCount}";
        }
    }
}