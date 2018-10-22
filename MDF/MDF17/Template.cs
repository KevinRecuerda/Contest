using System;
using System.Collections.Generic;
using System.Linq;

namespace Contest
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
            Solve();
        }

        public static void Solve()
        {
            var n = ConsoleHelper.ReadLineAs<int>();
            var map = new Cell[n,n];
            for (var i = 0; i < n; i++)
            {
                var line = ConsoleHelper.ReadLine().ToCharArray();
                for (var j = 0; j < n; j++)
                {
                    var cell = new Cell()
                    {
                        X = j,
                        Y = i,
                        Value = line[j]
                    };
                    map[i, j] = cell;
                }
            }

            var start = map[0, 0];
            var sword = map[n - 1, n - 1];

            var traps = GetMinimumTraps(start, sword, map, n);
            ConsoleHelper.WriteLine(traps);
        }

        private static int GetMinimumTraps(Cell start, Cell sword, Cell[,] map, int n)
        {
            var traps = 0;

            var way1 = Dijkstra(start, sword, map, n);
            if (way1 == null)
                return -2;

            foreach (var cell in way1)
            {
                if (cell.IsTrap)
                {
                    cell.Value = '#';
                    traps++;
                }
            }

            var way2 = Dijkstra(sword, start, map, n);
            if (way2 == null)
                return -1;

            foreach (var cell in way2)
            {
                if (cell.IsTrap)
                {
                    cell.Value = '#';
                    traps++;
                }
            }

            return traps;
        }

        private static List<Cell> Dijkstra(Cell start, Cell end, Cell[,] map, int n)
        {
            var cellToChecks = new List<Cell>();
            for (var i = 0; i < n; i++)
            {
                for (var j = 0; j < n; j++)
                {
                    var cell = map[i, j];
                    cell.Weight = int.MaxValue;
                    cell.TrapCount = int.MaxValue;
                    cell.Parent = null;
                    cellToChecks.Add(cell);
                }
            }

            start.Weight = 0;
            start.TrapCount = 0;
            while (cellToChecks.Count > 0)
            {
                var cell = cellToChecks.OrderBy(c => c.TrapCount).ThenBy(c => c.Weight).First();
                var info = cell == end ? "END" : "";
                //Utils.LocalPrint($"{info}cell={cell}");
                cellToChecks.Remove(cell);
                if (cell.Weight == int.MaxValue)
                    break;

                InformNeighboor(cell, map, n);
            }

            if (end.Parent == null)
                return null;

            var ways = new List<Cell>();
            var current = end;
            while (current.Parent != null)
            {
                ways.Add(current);
                current = current.Parent;
            }
            return ways;
        }

        private static void InformNeighboor(Cell cell, Cell[,] map, int n)
        {
            if (cell.X > 0)
                map[cell.Y, cell.X - 1].Inform(cell);
            if (cell.X < n-1)
                map[cell.Y, cell.X + 1].Inform(cell);
            if (cell.Y > 0)
                map[cell.Y - 1, cell.X].Inform(cell);
            if (cell.Y < n - 1)
                map[cell.Y + 1, cell.X].Inform(cell);
        }
    }

    public class Cell
    {
        public int X { get; set; }
        public int Y { get; set; }
        public char Value { get; set; }
        public bool IsWall => this.Value == '#';
        public bool IsTrap => this.Value == '!';

        public int Weight { get; set; }
        public int TrapCount { get; set; }
        public Cell Parent { get; set; }

        public void Inform(Cell cell)
        {
            if (this.IsWall)
                return;

            var trapCount = cell.TrapCount;
            if (this.IsTrap)
                trapCount++;

            var weight = cell.Weight + 1;

            if (trapCount < this.TrapCount 
                || trapCount == this.TrapCount && weight < this.Weight)
            {
                this.TrapCount = trapCount;
                this.Weight = weight;
                this.Parent = cell;
            }
        }

        public override string ToString()
        {
            return $"[{this.Y},{this.X}] {this.Value} (W={this.Weight}|T={this.TrapCount})";
        }
    }
}
