using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeJam.April18.Ex1
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
            var t = ConsoleHelper.ReadLineAs<int>();
            for (var i = 0; i < t; i++)
            {
                var line = ConsoleHelper.ReadLineAndSplit();
                var d = Convert.ToInt32(line[0]);
                var p = line[1];

                var bestHacks = Solve(d, p);
                var res = bestHacks >= 0 ? bestHacks.ToString() : "IMPOSSIBLE";
                ConsoleHelper.WriteLine($"Case #{i+1}: {res}");
            }
        }

        public static int Solve(int d, string p)
        {
            var actions = BuildActions(p);
            var total = actions.Sum(a => a.Damage());
            if (total <= d)
                return 0;

            var swap = 0;
            for (var i = actions.Length - 1; i > 0; i--)
            {
                if (actions[i] is Charge)
                    continue;

                var k = GetPreviousCharge(actions, i);
                if (k == -1)
                    return -1;

                // swap k with (k+1 ... i)
                for (var j = k + 1; j <= i; j++)
                {
                    var shoot = actions[j];
                    actions[j] = actions[j - 1];
                    actions[j - 1] = shoot;

                    shoot.Strength /= 2;
                    total -= shoot.Strength;
                    swap++;

                    if (total <= d)
                        return swap;
                }
            }

            return -1;
        }

        private static int GetPreviousCharge(Action[] actions, int i)
        {
            for (var k = i - 1; k >= 0; k--)
            {
                if (actions[k] is Charge)
                    return k;
            }

            return -1;
        }

        private static Action[] BuildActions(string p)
        {
            var currentStrength = 1;
            var actions = new Action[p.Length];
            for (var i = 0; i < p.Length; i++)
            {
                var c = p[i];

                var action = BuildAction(c, currentStrength);

                actions[i] = action;

                if (action is Charge)
                    currentStrength *= 2;
            }

            return actions;
        }

        private static Action BuildAction(char c, int currentStrength)
        {
            switch (c)
            {
                case 'C':
                case 'c':
                    return new Charge(currentStrength);
                case 'S':
                case 's':
                    return new Shoot(currentStrength);
                default:
                    throw new NotImplementedException();
            }
        }

        public abstract class Action
        {
            public Action(int strength)
            {
                Strength = strength;
            }

            public int Strength { get; set; }

            public abstract int Damage();
        }

        public class Shoot : Action
        {
            public Shoot(int strength) : base(strength)
            {
            }

            public override int Damage() => this.Strength;
        }

        public class Charge : Action
        {
            public Charge(int strength) : base(strength)
            {
            }

            public override int Damage() => 0;
        }
    }
}