using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace BattleDev.Mar18.Ex5
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
            var line = ConsoleHelper.ReadLine();

            var res = Find(line);

            ConsoleHelper.WriteLine(res);
        }

        private static string FindOld(string line)
        {
            var res = "";
            var start = line.IndexOf('X');

            var leftIndex = start - 1;
            var rightIndex = start + 1;

            var left = "";
            var leftMult = 0;
            var leftOr = 0;

            var right = "";
            var rightMult = 0;
            var rightOr = 0;

            while (leftIndex >= 0 || rightIndex < line.Length)
            {
                if (left == "")
                {
                    leftMult = 0;
                    leftOr = 0;
                    while (leftIndex >= 0 && line[leftIndex] == '*')
                    {
                        leftIndex--;
                        leftMult++;
                        left += "*";
                    }

                    while (leftIndex >= 0 && line[leftIndex] == 'o')
                    {
                        leftIndex--;
                        leftOr++;
                        left += "o";
                    }
                }

                if (right == "")
                {
                    rightMult = 0;
                    rightOr = 0;
                    while (rightIndex < line.Length && line[rightIndex] == '*')
                    {
                        rightIndex++;
                        rightMult++;
                        right += "*";
                    }

                    while (rightIndex < line.Length && line[rightIndex] == 'o')
                    {
                        rightIndex++;
                        rightOr++;
                        right += "o";
                    }
                }

                var leftFirst = leftOr * Math.Pow(2, rightMult) + rightOr;
                var rightFirst = rightOr * Math.Pow(2, leftMult) + leftOr;

                if (leftFirst > rightFirst)
                {
                    res += left;
                    left = "";
                }
                else
                {
                    res += right;
                    right = "";
                }
            }

            res += left + right;

            return res;
        }

        private static string Find(string line)
        {
            var res = "";
            var start = line.IndexOf('X');

            var right = DecomposeRight(line, start);
            var left = DecomposeLeft(line, start);
            var count = right.Count + left.Count;

            var currentR = right.Count > 0 ? right.Dequeue() : new Group() { IsNull = true };
            var currentL = left.Count > 0 ? left.Dequeue() : new Group() { IsNull = true };

            for (var i = 0; i < count; i++)
            {
                if (currentL.IsNull)
                {
                    res += currentR.value;
                    currentR = right.Count > 0 ? right.Dequeue() : new Group() { IsNull = true };
                    continue;
                }
                if (currentR.IsNull)
                {
                    res += currentL.value;
                    currentL = left.Count > 0 ? left.Dequeue() : new Group() { IsNull = true };
                    continue;
                }

                var leftFirst = currentR.Count(currentL.Or);
                var rightFirst = currentL.Count(currentR.Or);

                if (leftFirst > rightFirst)
                {
                    res += currentL.value;
                    currentL = left.Count > 0 ? left.Dequeue() : new Group() { IsNull = true };
                }
                else
                {
                    res += currentR.value;
                    currentR = right.Count > 0 ? right.Dequeue() : new Group(){IsNull = true};
                }
            }

            return res;
        }
        private static Queue<Group> DecomposeRight(string line, int idx)
        {
            var group = new Group();
            var groups = new Queue<Group>();
            groups.Enqueue(group);
            var last = 'x';
            for (var i = idx + 1; i < line.Length; i++)
            {
                if (line[i] == '*')
                {
                    if (last == 'o')
                    {
                        group = new Group();
                        groups.Enqueue(group);
                    }
                    group.Mult++;
                    group.value += '*';
                }
                else
                {
                    group.Or++;
                    group.value += 'o';
                }

                last = line[i];
            }

            return groups;
        }

        private static Queue<Group> DecomposeLeft(string line, int idx)
        {
            var group = new Group();
            var groups = new Queue<Group>();
            groups.Enqueue(group);
            var last = 'x';
            for (var i = idx - 1; i >= 0; i--)
            {
                if (line[i] == '*')
                {
                    if (last == 'o')
                    {
                        group = new Group();
                        groups.Enqueue(group);
                    }
                    group.Mult++;
                    group.value += '*';
                }
                else
                {
                    group.Or++;
                    group.value += 'o';
                }

                last = line[i];
            }

            return groups;
        }

        public class Group
        {
            public string value = "";
            public int Mult;
            public int Or;

            public bool IsNull;

            public int Count(int x)
            {
                return (int)(x * Math.Pow(2, Mult) + Or);
            }
        }
    }
}