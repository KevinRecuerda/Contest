using System;
using System.Collections.Generic;
using System.Linq;

namespace MDF17.Ex4
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
            return (T) Convert.ChangeType(value, typeof (T));
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
            // map
            var n = ConsoleHelper.ReadLineAs<int>();
            var map = new int[n, n];
            for (var i = 0; i < n; i++)
            {
                var line = ConsoleHelper.ReadLine();
                for (var j = 0; j < n; j++)
                {
                    var cellScore = line[j] == 'X' ? 1 : 0;

                    map[i, j] = cellScore;

                    if (i > 0)
                    {
                        var nScore = map[i - 1, j] + cellScore;
                        if (nScore > map[i, j])
                            map[i, j] = nScore;
                    }

                    if (j > 0)
                    {
                        var nScore = map[i, j - 1] + cellScore;
                        if (nScore > map[i, j])
                            map[i, j] = nScore;
                    }

                    if (j > 0 && i > 0)
                    {
                        var nScore = map[i - 1, j - 1] + cellScore;
                        if (nScore > map[i, j])
                            map[i, j] = nScore;
                    }
                    //    neighboors.Add(map[cell.Y - 1, cell.X]);
                    //if (j > 0)
                    //    neighboors.Add(map[cell.Y, cell.X - 1]);
                    //if (i > 0 && j > 0)
                    //    neighboors.Add(map[cell.Y - 1, cell.X - 1]);
                    //foreach (var neighboor in GetNeighboors(i,j, map, n))
                    //{
                    //    var nScore = neighboor.Score + cellScore;
                    //    if (nScore > cell.Score)
                    //        cell.Score = nScore;
                    //}
                }
            }
            //// Check
            //for (var i = 0; i < n; i++)
            //    {
            //        for (var j = 0; j < n; j++)
            //        {
            //            Utils.LocalPrint("I=" + i + ";J=" + j + "|");
            //            var cell = map[i, j];
            //            var cellScore = cell.IsGate() ? 1 : 0;

            //            foreach (var neighboor in GetNeighboors(cell, map, n))
            //            {
            //                var nScore = neighboor.Score + cellScore;
            //                if (nScore > cell.Score)
            //                    cell.Score = nScore;
            //            }
            //        }
            //    }

            ConsoleHelper.WriteLine(map[n - 1, n - 1]);
        }

        public static List<Cell> GetNeighboors(Cell cell, Cell[,] map, int n)
        {
            var neighboors = new List<Cell>();

            if (cell.Y > 0)
                neighboors.Add(map[cell.Y - 1, cell.X]);
            if (cell.X > 0)
                neighboors.Add(map[cell.Y, cell.X - 1]);
            if (cell.Y > 0 && cell.X > 0)
                neighboors.Add(map[cell.Y - 1, cell.X - 1]);

            return neighboors;
        }

        public class Cell
        {
            public int Y;
            public int X;
            public char Value;
            public int Score;

            public Cell(int y, int x, char value)
            {
                this.Y = y;
                this.X = x;
                this.Value = value;
                this.Score = 0;
            }

            public bool IsGate()
            {
                return this.Value == 'X';
            }
        }
    }
}
