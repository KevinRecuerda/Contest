using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeChef.Oct18.MCO4
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

        private static int N;
        private static int L;

        private static Dictionary<int, List<Edge>> edgesByVertice;

        public static void Solve()
        {
            N = ConsoleHelper.ReadLineAs<int>();
            L = (int)Math.Log(N, 2);

            edgesByVertice = Enumerable.Range(0, N).ToDictionary(i => i, i => new List<Edge>());

            var edgesCount = N * (N - 1) / 2;
            for (var i = 1; i <= edgesCount; i++)
            {
                var input = ConsoleHelper.ReadLineAndSplitAsListOf<int>();
                var u = input[0];
                var v = input[1];
                var edge = new Edge(i, u, v);
                edgesByVertice[u].Add(edge);
            }

            Solve(L, new List<Possibility>(){new Possibility()});

            ConsoleHelper.WriteLine(0);
        }

        private static bool Solve(
            double depth,
            List<Possibility> possibilities)
        {
            foreach (var possibility in possibilities)
            {
                var potentialVertices = edgesByVertice
                    .Where(x => x.Value.Count >= depth && !possibility.AlreadyUsed.Contains(x.Key))
                    .ToList();

                if (potentialVertices.Count == 0)
                    return false;

                foreach (var potentialVertex in potentialVertices)
                {
                    possibility.AlreadyUsed.Add(potentialVertex.Key);

                    var can = false; //Solve(n, eadyUsed, depth - 1);
                    if (can)
                    {
                    }

                    possibility.AlreadyUsed.Remove(potentialVertex.Key);
                }

            }

            return false;
        }

        public class Edge
        {
            public Edge(int id, int u, int v)
            {
                this.Id = id;
                this.U = u;
                this.V = v;
            }

            public int Id { get; set; }
            public int U { get; set; }
            public int V { get; set; }
        }

        public class Possibility
        {
            public HashSet<int> AlreadyUsed { get; set; } = new HashSet<int>();
            public List<int> RemovedEdges { get; set; } = new List<int>();
        }
    }
}