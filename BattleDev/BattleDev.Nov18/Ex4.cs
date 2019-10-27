using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace BattleDev.Nov18.Ex4
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
            var times = new List<Student>();
            for (var i = 0; i < n; i++)
            {
                var input = ConsoleHelper.ReadLineAndSplitAsListOf<int>();
                times.Add(new Student(i, input[0]));
                times.Add(new Student(i, input[1]));
            }

            times = times.OrderBy(t => t.Available).ToList();

            var count = Find(times, new HashSet<int>(), -1);
            ConsoleHelper.WriteLine(count);
        }

        private static int Find(List<Student> students, HashSet<int> taken, int time)
        {
            if (students.Count == 0)
                return 0;

            var student = students[0];
            var remaining = students.ToList();
            remaining.Remove(student);

            // not taken
            var max = Find(remaining, taken, time);

            if (taken.Contains(student.Id) || student.Available < time)
                return max;

            // taken
            remaining = students.ToList();
            remaining.Remove(student);

            var subTaken = new HashSet<int>(taken) {student.Id};

            var res = 1 + Find(remaining, subTaken, student.Available + 61);
            if (res > max)
                max = res;

            return max;
        }

        public class Student
        {
            public Student(int id, int available)
            {
                Id = id;
                Available = available;
            }

            public int Id { get; set; }
            public int Available { get; set; }
        }
    }
}
