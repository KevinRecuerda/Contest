using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeChef._2020_05.NRWP
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
            var input = ConsoleHelper.ReadLineAndSplitAsListOf<int>();
            var h = input[0];
            var w = input[1];
            var n = input[2];

            var map = new int[h][];
            for (var i = 0; i < h; i++)
            {
                var line = ConsoleHelper.ReadLineAndSplitAsListOf<int>().ToArray();
                map[i] = line;
            }

            var neighbors = new List<Neighbor>();
            var particles = new Particle[n];
            for (var k = 0; k < n; k++)
            {
                input = ConsoleHelper.ReadLineAndSplitAsListOf<int>();
                var y = input[0]-1;
                var x = input[1]-1;
                var p = input[2];
                particles[k] = new Particle(y, x, map[y][x], p);

                for (var l = 0; l < k; l++)
                {
                    if (Math.Abs(particles[l].Y - y) + Math.Abs(particles[l].X - x) == 1)
                    {
                        var neighbor = new Neighbor(particles[l], particles[k]);
                        neighbors.Add(neighbor);
                    }
                }
            }

            var groups = particles.Select(p => p.Group).Distinct().ToList();
            foreach (var group in groups)
                Maximize(group.Particles);

            var sum = particles.Sum(p => p.V);
            sum += neighbors.Sum(g => g.V);
            
            ConsoleHelper.WriteLine(sum);
            ConsoleHelper.WriteLine(string.Join(" ", particles.Select(p => p.Choice)));
        }

        private static void Maximize(List<Particle> particles)
        {
            while (particles.Count > 0)
            {
                var prioritized = particles.Where(p => Math.Abs(p.Weight) >= p.AdjacentCells.Values.Sum()).ToList();
                if (prioritized.Count > 0)
                {
                    foreach (var p in prioritized)
                    {
                        var c = p.Weight >= 0 ? 1 : -1;
                        Choose(p, c);
                        particles.Remove(p);
                    }
                    continue;
                }

                var max = particles.Max(p => Math.Abs(p.Weight));
                var particle = particles.First(p => Math.Abs(p.Weight) == max);

                var sum = particles.Sum(p => p.Weight);
                var choice = sum >= 0 ? 1 : -1;
                Choose(particle, choice);
                particles.Remove(particle);
            }
        }

        private static void Choose(Particle particle, int choice)
        {
            particle.Choice = choice;

            foreach (var g in particle.AdjacentCells)
            {
                var adjacent = g.Key;
                adjacent.AdjacentCells.Remove(particle);

                adjacent.Weight += adjacent.P * particle.P * particle.Choice;
            }
        }
    }

    public class Particle
    {
        public Particle(int y, int x, int g, int p)
        {
            Y = y;
            X = x;
            G = g;
            P = p;

            Choice = 1;
            Weight = G * P;
            
            Group = new Group(this);
        }

        public readonly int Y, X, P, G;
        public int Choice, Weight;

        public Group Group;
        public readonly Dictionary<Particle, int> AdjacentCells = new Dictionary<Particle, int>();

        public int V => G * P * Choice;

        public override string ToString()
        {
            return $"[{Y},{X}] P={P} | G={G} | W={Weight}";
        }
    }

    public class Neighbor
    {
        public Neighbor(Particle particle1, Particle particle2)
        {
            Particle1 = particle1;
            Particle2 = particle2;

            particle1.AdjacentCells[particle2] = V;
            particle2.AdjacentCells[particle1] = V;

            particle1.Group.Combine(particle2.Group);
        }
        
        public Particle Particle1 { get; set; }
        public Particle Particle2 { get; set; }

        public int V => Particle1.P * Particle2.P * Particle1.Choice * Particle2.Choice;
    }

    public class Group
    {
        public Group(Particle particle)
        {
            this.Particles = new List<Particle>(){particle};
        }

        public readonly List<Particle> Particles;

        public void Combine(Group other)
        {
            Particles.AddRange(other.Particles);

            foreach (var particle in other.Particles)
                particle.Group = this;
        }
    }
}