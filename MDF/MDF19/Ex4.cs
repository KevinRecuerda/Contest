using System;
using System.Collections.Generic;
using System.Linq;

namespace Contest.MDF19.Ex4
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
            var m = input[1];

            var nodes = Enumerable.Range(0, n).Select(i => new Node(i)).ToArray();

            for (int i = 0; i < n; i++)
            {
                var p = ConsoleHelper.ReadLineAs<int>();
                if (p == -1)
                    continue;

                nodes[i].SetParent(nodes[p]);
            }

            var toChecks = nodes.Select(x => x.Id).ToList();
            var r = Solve(toChecks, nodes);

            var res = string.Join(" ", r.OrderBy(x => x));
            ConsoleHelper.WriteLine(res);
        }

        public static List<int> Solve(List<int> toChecks, Node[] nodes)
        {
            if (toChecks.Count == 0)
                return new List<int>();

            var max = toChecks.Max(x => nodes[x].Weight);
            var ids = toChecks.Where(x => nodes[x].Weight == max).ToList();

            int bestNode= -1;
            var removed = new List<int>();
            foreach (var id in ids)
            {
                var tc = toChecks.ToList();
                    tc.Remove(id);
                if (nodes[id].Parent > -1)
                    tc.Remove(nodes[id].Parent);

                foreach (var child in nodes[id].Children)
                    tc.Remove(child);

                var newNodes = nodes.Select(n => n.Copy()).ToArray();

                var node = newNodes[id];
                var parentId = newNodes[id].Parent;
                if (parentId > -1)
                {
                    var parent = newNodes[parentId];
                    if (parent.Parent > -1)
                    {
                        newNodes[parent.Parent].Children.Remove(parentId);
                        parent.Parent = -1;
                    }

                    foreach (var parentChild in parent.Children)
                    {
                        var c = newNodes[parentChild];
                        c.Parent = -1;
                    }
                }


                foreach (var child in node.Children)
                {
                    var c = newNodes[child];
                    c.Parent = -1;
                }

                var r = Solve(tc, newNodes);
                if (r.Count > removed.Count)
                {
                    removed = r;
                    bestNode = id;
                }
            }
            removed.Add(bestNode);
            return removed;
        }

        public class Node
        {
            public int Id;

            public int Parent = -1;
            public List<int> Children = new List<int>();

            public int Weight => (this.Parent > -1 ? 1 : 0) + this.Children.Count;

            public Node(int id)
            {
                this.Id = id;
            }

            public void SetParent(Node node)
            {
                this.Parent = node;
                node.Children.Add(this);
            }

            public Node Copy()
            {
                return new Node(this.Id)
                {
                    Parent = this.Parent,
                    Children = this.Children.ToList()
                };
            }
        }
    }
}