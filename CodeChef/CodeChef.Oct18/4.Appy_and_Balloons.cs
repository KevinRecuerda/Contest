using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace CodeChef.Oct18.HMAPPY
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
            var input = ConsoleHelper.ReadLineAndSplitAsListOf<long>();
            var n = input[0];
            var m = input[1];

            var a = ConsoleHelper.ReadLineAndSplitAsListOf<long>();
            var b = ConsoleHelper.ReadLineAndSplitAsListOf<long>();

            var res = 0L;
            var balloonNeeded = a.Sum();
            if (m < balloonNeeded)
            {
                res = SolveFaster(n, m, a, b);
                //res = SolveFast(n, m, a, b);
                //res = m <= balloonNeeded / 2
                //    ? Solve(n, m, a, b)
                //    : SolveInverse(n, balloonNeeded - m, a, b);
            }

            ConsoleHelper.WriteLine(res);
        }

        private static long SolveFaster(long n, long m, List<long> a, List<long> b)
        {
            var dico = new Dictionary<long, long>();
            for (var i = 0; i < n; i++)
            {
                for (var k = 1; k <= a[i]; k++)
                {   
                    var value = k * b[i];
                    if (dico.ContainsKey(value))
                        dico[value]++;
                    else
                        dico[value] = 1;
                }
            }

            var res = 0L;
            foreach (var valuePair in dico.OrderByDescending(x => x.Key))
            {
                m -= valuePair.Value;
                if (m < 0)
                {
                    res = valuePair.Key;
                    break;
                }
            }

            return res;
        }

        private static long SolveFast(long n, long m, List<long> a, List<long> b)
        {
            var sortedDays = new SortedSet<Day>(new DayComparer());
            for (var i = 0; i < n; i++)
            {
                var day = new Day(i, a[i], b[i], 0);
                sortedDays.Add(day);
            }

            var cTotal = 0L;
            var aTotal = 0L;
            for (var i = 0; i < n; i++)
            {
                cTotal += a[i] * b[i];
                aTotal += a[i];
            }

            foreach (var day in sortedDays.ToList())
            {
                cTotal -= day.C;
                aTotal -= day.A;
                n--;

                // C_average_adjusted = C_average * (1 - (M-BALLOON) / A_total)
                // = C_day - B_day * BALLOON
                // <=> BALLOON = (C_day - C_average * (1 - M / A_total))  /  (C_average / A_total + B_day)

                var balloonNeeded = n > 0
                    ? (day.C - cTotal * 1.0 / n * (1 - m * 1.0 / aTotal)) / ((cTotal * 1.0 / n) / aTotal + day.B)
                    : day.A;
                var balloon = Math.Min((long) Positiv(balloonNeeded), m);
                if (balloon > 0)
                {
                    sortedDays.Remove(day);

                    m -= balloon;
                    day.AddBalloon(balloon);

                    sortedDays.Add(day);
                }
            }

            for (var k = m; k > 0; k--)
            {
                var day = sortedDays.Max;
                sortedDays.Remove(day);

                day.AddBalloon();
                sortedDays.Add(day);
            }

            var nextSortedDays = new SortedSet<Day>(new NextBalloonDayComparer());
            foreach (var day in sortedDays)
            {
                if (day.CanRemoveBalloon())
                    nextSortedDays.Add(day);
            }

            // switch
            while (sortedDays.Max.C > nextSortedDays.Min.C + nextSortedDays.Min.B)
            {
                var day = sortedDays.Max;
                sortedDays.Remove(day);
                nextSortedDays.Remove(day);

                var nextDay = nextSortedDays.Min;
                sortedDays.Remove(nextDay);
                nextSortedDays.Remove(nextDay);

                day.AddBalloon();
                sortedDays.Add(day);
                nextSortedDays.Add(day);

                nextDay.RemoveBalloon();
                sortedDays.Add(nextDay);
                if (day.CanRemoveBalloon())
                    nextSortedDays.Add(nextDay);
            }

            var res = sortedDays.Max.C;
            return res;
        }

        private static long Positiv(long number) => Math.Max(number, 0);
        private static double Positiv(double number) => Math.Max(number, 0);

        private static long Solve(long n, long m, List<long> a, List<long> b)
        {
            var sortedDays = new SortedSet<Day>(new DayComparer());
            for (var i = 0; i < n; i++)
            {
                var day = new Day(i, a[i], b[i], 0);
                sortedDays.Add(day);
            }

            for (var k = 0; k < m; k++)
            {
                var day = sortedDays.Max;
                sortedDays.Remove(day);

                day.AddBalloon();
                sortedDays.Add(day);
            }

            var res = sortedDays.Max.C;
            return res;
        }

        private static long SolveInverse(long n, long inversedM, List<long> a, List<long> b)
        {
            var sortedDays = new SortedSet<Day>(new NextBalloonDayComparer());
            for (var i = 0; i < n; i++)
            {
                var day = new Day(i, a[i], b[i], a[i]);
                if (day.CanRemoveBalloon())
                    sortedDays.Add(day);
            }

            var last = 0L;
            for (var k = 0; k < inversedM; k++)
            {
                var day = sortedDays.Min;
                sortedDays.Remove(day);

                day.RemoveBalloon();
                if (day.CanRemoveBalloon())
                    sortedDays.Add(day);
                else
                    last = day.C;
            }

            var res = sortedDays.Max(d => d.C);
            res = Math.Max(last, res);
            return res;
        }

        public class Day
        {
            public Day(long id, long a, long b, long ballon)
            {
                this.Id = id;
                A = a;
                B = b;

                Balloon = ballon;
                C = (A - Balloon) * B;
            }

            public bool CanAddBalloon() => Balloon < A;

            public void AddBalloon(long count = 1)
            {
                C -= B * count;
                Balloon += count;
            }

            public bool CanRemoveBalloon() => Balloon > 0;

            public void RemoveBalloon()
            {
                C += B;
                Balloon--;
            }

            public long Id;
            public long A;
            public long B;
            public long C;

            public long Balloon = 0;

            public override string ToString()
            {
                return $"Id={Id} | Balloon={Balloon}/{A} | B={B} | C={C}";
            }
        }

        public class DayComparer : IComparer<Day>
        {
            public int Compare(Day x, Day y)
            {
                var compare = x.C.CompareTo(y.C);
                if (compare == 0)
                    compare = x.Id.CompareTo(y.Id);

                return compare;
            }
        }

        public class NextBalloonDayComparer : IComparer<Day>
        {
            public int Compare(Day x, Day y)
            {
                var compare = (x.C + x.B).CompareTo(y.C + y.B);
                if (compare == 0)
                    compare = x.Id.CompareTo(y.Id);

                return compare;
            }
        }
    }
}