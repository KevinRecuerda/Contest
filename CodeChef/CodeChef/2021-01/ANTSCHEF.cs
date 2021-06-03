using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeChef._2021_01.ANTSCHEF
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
            var n = ConsoleHelper.ReadLineAs<int>();

            var lines = new AntLine[n];
            for (var i = 0; i < n; i++)
                lines[i] = new AntLine(ConsoleHelper.ReadLineAndSplitAsListOf<long>().Skip(1));

            var res = CountCollisions(lines);
            ConsoleHelper.WriteLine(res);
        }

        private static int CountCollisionsSmart(AntLine[] lines)
        {
            var count = 0;
            foreach (var line in lines)
            {
                var left = line.Ants.Count(x => x.Direction);
                var right = line.Ants.Count - left;
                count += left * right;
            }

            return count;
        }

        private static int CountCollisions(AntLine[] lines)
        {
            var collisions = new SortedSet<Collision>(Collision.TimeComparer);
            
            foreach (var line in lines)
            {
                var right = line.Ants.FirstOrDefault(a => !a.Direction);
                var collision = Collision.SimpleSafe(0, right);
                if (collision != null)
                    collisions.Add(collision);
            }

            var count = 0;
            while (collisions.Count > 0)
            {
                var collision = collisions.First();
                collisions.Remove(collision);

                count++;
                var next = collision.Run();
                next.ForEach(x => collisions.Add(x));
            }

            return count;
        }
    }

    public class AntLine
    {
        public List<Ant> Ants { get; set; }
        
        public AntLine(IEnumerable<long> positions)
        {
            // use 2 factor, to avoid 0.5 collision
            Ants = positions.Select(p => new Ant(2*p)).ToList();
            
            for (var i = 1; i < Ants.Count; i++)
                Ants[i].SetLeft(Ants[i-1]);
        }
    }

    public class Ant
    {
        public long Time { get; set; }
        public long Position { get; set; }
        public bool Direction { get; set; }

        public Ant Left { get; set; }
        public Ant Right { get; set; }

        public Ant(long position)
        {
            Time = 0;
            Position = position;
            Direction = position < 0;
        }

        public void SetLeft(Ant left)
        {
            Left = left;
            left.Right = this;
        }

        public void Move(long time)
        {
            if (time <= Time)
                return;
            
            Position += (Time - time) * (Direction ? 1 : -1);
            Time = time;
        }

        public void GoAway() => Direction = !Direction;
    }

    public class Collision
    {
        public long Time { get; set; }
        public List<Ant> Ants { get; set; }
        
        public Collision(long time, params Ant[] ants)
        {
            Time = time;
            Ants = ants.ToList();
        }

        public static Collision Simple(long time, Ant left, Ant right)
        {
            left.Move(time);
            right.Move(time);
            
            var collisionTime = time + (left.Position + right.Position) / 2;
            return new Collision(collisionTime, left, right);
        }

        public static Collision SimpleSafe(long time, Ant ant)
        {
            if (ant == null)
                return null;

            var opposite = ant.Direction ? ant.Right : ant.Left;
            if (opposite == null || opposite.Direction == ant.Direction)
                return null;
            
            return Simple(time, ant, opposite);
        }

        public List<Collision> Run()
        {
            var next = new List<Collision>();
            foreach (var ant in Ants)
            {
                ant.Move(Time);
                ant.GoAway();

                var collision = SimpleSafe(Time, ant);
                if (collision != null)
                    next.Add(collision);
            }

            return next;
        }

        public static IComparer<Collision> TimeComparer { get; } = new TimeRelationalComparer();
        
        private sealed class TimeRelationalComparer : IComparer<Collision>
        {
            public int Compare(Collision x, Collision y)
            {
                if (ReferenceEquals(x, y)) return 0;
                if (ReferenceEquals(null, y)) return 1;
                if (ReferenceEquals(null, x)) return -1;
                return x.Time.CompareTo(y.Time);
            }
        }
    }
}