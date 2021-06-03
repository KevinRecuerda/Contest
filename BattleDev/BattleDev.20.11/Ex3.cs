using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace BattleDev._2020_11.Ex3
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
            var n = ConsoleHelper.ReadLineAs<int>();

            var dico = new Dictionary<int, Node>();
            for (var i = 0; i < n-1; i++)
            {
                var input = ConsoleHelper.ReadLine().Split(' ').Select(int.Parse).ToArray();
                var a = AddIfNotExists(dico, input[0]);
                var b = AddIfNotExists(dico, input[1]);
                
                a.Parent = b;
                b.Children.Add(a);
            }

            dico[0].SetDepth(1);

            var res = dico.Values.GroupBy(x => x.Depth).OrderBy(x => x.Key).Select(x => x.Count()).ToList();
            res.AddRange(Enumerable.Repeat(0, 10 - res.Count));
            ConsoleHelper.WriteLine(string.Join(" ", res));
        }

        private static Node AddIfNotExists(Dictionary<int, Node> dico, int x)
        {
            if (!dico.ContainsKey(x))
                dico[x] = new Node(x);

            return dico[x];
        }

        public class Node
        {
            public Node(int id)
            {
                Id = id;
                Children = new List<Node>();
                Depth = -1;
            }

            public int Id { get; set; }
            public List<Node> Children { get; set; }
            public Node Parent { get; set; }
            public int Depth { get; set; }

            public void SetDepth(int depth)
            {
                Depth = depth;
                Children.ForEach(c => c.SetDepth(depth+1));
            }
        }
    }
}