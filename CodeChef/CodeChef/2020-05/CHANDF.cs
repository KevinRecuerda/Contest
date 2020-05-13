using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeChef._2020_05.CHANDF
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
            var line = ReadLine();

            return ConvertTo<T>(line);
        }

        public string[] ReadLineAndSplit(char delimiter = ' ')
        {
            var splittedLine = ReadLine().Split(delimiter);
            return splittedLine;
        }

        public List<T> ReadLineAndSplitAsListOf<T>(char delimiter = ' ')
        {
            var splittedLine = ReadLineAndSplit();

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
            var input = ConsoleHelper.ReadLineAndSplitAsListOf<long>();
            var x = input[0];
            var y = input[1];
            var l = input[2];
            var r = input[3];

            if (x == 0 || y == 0)
            {
                ConsoleHelper.WriteLine(l);
                return;
            }

            var z = x | y;
            if (z > r || z < l)
            {
                var xb = Convert.ToString(x, 2);
                var yb = Convert.ToString(y, 2);
                var rb = Convert.ToString(r, 2);
                var lb = Convert.ToString(l, 2);
                var size = Math.Max(Math.Max(xb.Length, yb.Length), Math.Max(rb.Length, lb.Length));
                xb = Expand(xb, size);
                yb = Expand(yb, size);
                rb = Expand(rb, size);
                lb = Expand(lb, size);

                var w = new W(size);
                for (var i = size - 1; i >= 0; i--)
                {
                    var wx = xb[i] == '1' ? 1L << (size - 1 - i) : 0;
                    w.X[i] = wx;
                    w.XSum[i] = wx + w.XSum[i + 1];

                    var wy = yb[i] == '1' ? 1L << (size - 1 - i) : 0;
                    w.Y[i] = wy;
                    w.YSum[i] = wy + w.YSum[i + 1];
                }

                var rz = FindLowerZ(rb, lb, false, 0, new F(), w).Z;
                z = Convert.ToInt64(rz, 2);
            }

            if ((z & x) == 0 || (z & y) == 0)
                z = l;
            ConsoleHelper.WriteLine(z);
        }

        private static F FindLowerZ(string rb, string lb, bool upperLb, int i, F f, W w)
        {
            if (i == rb.Length)
                return f;

            if (rb[i] == '0')
            {
                var sub = f.Clone();
                sub.Z += "0";
                sub = FindLowerZ(rb, lb, upperLb, i + 1, sub, w);
                return sub;
            }

            // use 1
            var sub1 = f.Clone();
            sub1.Z += "1";
            sub1.A += w.X[i];
            sub1.B += w.Y[i];
            sub1 = FindLowerZ(rb, lb, upperLb || lb[i] == '0', i + 1, sub1, w);

            // use 0 and full 1 after
            var sub2 = f.Clone();
            sub2.Z += "0"; // + new string('1', rb.Length - i - 1);
            sub2.A += w.XSum[i + 1];
            sub2.B += w.YSum[i + 1];

            if (sub1.Value > sub2.Value || lb[i] == '1' && !upperLb)
                return sub1;
            
            for (var j = i + 1; j < lb.Length; j++)
            {
                if (w.X[j] + w.Y[j] > 0)
                {
                    sub2.Z += "1";
                    upperLb = upperLb || lb[j] == '0';
                    continue;
                }

                sub2.Z += lb[j] == '0' || upperLb ? "0" : "1";
            }

            return sub2;
        }

        public static string Expand(string s, int size)
        {
            if (s.Length >= size)
                return s;

            var extension = new string('0', size - s.Length);
            return extension + s;
        }
    }

    public class F
    {
        public string Z { get; set; } = "";
        public long A { get; set; }
        public long B { get; set; }

        public long Value => A * B;

        public F Clone()
        {
            return new F()
            {
                Z = Z,
                A = A,
                B = B
            };
        }

        public override string ToString()
        {
            return $"F={A}*{B}={Value} with Z={Z}";
        }
    }

    public class W
    {
        public W(int size)
        {
            X = new long[size];
            XSum = new long[size + 1];
            Y = new long[size];
            YSum = new long[size + 1];

            XSum[size] = YSum[size] = 0;
        }

        public long[] X { get; set; }
        public long[] XSum { get; set; }
        public long[] Y { get; set; }
        public long[] YSum { get; set; }
    }
}