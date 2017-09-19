using System;
using System.Collections.Generic;
using System.Linq;

namespace MDF17.Ex3
{
    #region ConsoleHelper
    public interface IConsoleHelper
    {
        string ReadLine();
        T ReadLineAs<T>();
        List<T> ReadLineAndSplitAsListOf<T>(char delimiter = ' ');
        void WriteLine(object obj);
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

        public List<T> ReadLineAndSplitAsListOf<T>(char delimiter = ' ')
        {
            var splittedLine = this.ReadLine().Split(delimiter);

            return splittedLine.Select(ConvertTo<T>).ToList();
        }

        public virtual void WriteLine(object obj)
        {
            Console.WriteLine(obj);
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
            var h = ConsoleHelper.ReadLineAs<int>();
            var l = ConsoleHelper.ReadLineAs<int>();
            var map = new Cell[h, l];
            Cell firstClick = null;
            for (var i = 0; i < h; i++)
            {
                var line = ConsoleHelper.ReadLine();
                for (var j = 0; j < l; j++)
                {
                    map[i, j] = new Cell(i, j, line[j]);
                    if (map[i, j].Value == 'x')
                    {
                        firstClick = map[i, j];
                    }
                }
            }

            // count
            for (var i = 0; i < h; i++)
            {
                for (var j = 0; j < l; j++)
                {
                    var neighboors = GetNeighboors(map[i, j], map, h, l);
                    map[i, j].Weight = neighboors.Count(x => x.IsMine());
                }
            }

            // Check
            var result = new HashSet<Cell>();
            Check(firstClick, map, h, l, result);
            ConsoleHelper.WriteLine(result.Count);
        }

        private static void Check(Cell cell, Cell[,] map, int h, int l, HashSet<Cell> result)
        {
            result.Add(cell);
            if (cell.Weight != 0)
                return;

            var neighboors = GetNeighboors(cell, map, h, l);
            var safeNeighboors = neighboors.Where(x => !x.IsMine()).ToList();
            //foreach (var safeNeighboor in safeNeighboors)
            //{
            //    Check(safe, map, h, l, result);
            //}

            foreach (var safe in safeNeighboors.Where(x => !result.Contains(x)))
            {
                result.Add(safe);
                Check(safe, map, h, l, result);
            }
        }

        private static List<Cell> GetNeighboors(Cell cell, Cell[,] map, int h, int l)
        {
            var neighboors = new List<Cell>();
            for (var i = cell.Y - 1; i <= cell.Y + 1; i++)
            {
                for (var j = cell.X - 1; j <= cell.X + 1; j++)
                {
                    if (i == cell.Y && j == cell.X)
                        continue;

                    if (0 <= i && i < h
                        && 0 <= j && j < l)
                    {
                        neighboors.Add(map[i, j]);
                    }
                }
            }
            return neighboors;
        }

        public class Cell
        {
            public int Y;
            public int X;
            public char Value;
            public int Weight;

            public Cell(int y, int x, char value)
            {
                this.Y = y;
                this.X = x;
                this.Value = value;
                this.Weight = -1;
            }

            public bool IsMine()
            {
                return this.Value == '*';
            }
        }
    }
}
