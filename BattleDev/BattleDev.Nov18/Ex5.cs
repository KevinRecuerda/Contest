using System;
using System.Collections.Generic;
using System.Linq;

namespace BattleDev.Nov18.Ex5
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
            var students = new List<Student>();
            var meetings = new List<Meeting>();
            for (var i = 0; i < n; i++)
            {
                var input = ConsoleHelper.ReadLineAndSplitAsListOf<int>();
                var student = new Student(i, input[0], input[1]);
                students.Add(student);
                meetings.Add(student.Meeting1);
                meetings.Add(student.Meeting2);
            }

            foreach (var student in students)
            {
                student.Meeting1.Blockings.AddRange(meetings.Where(m => Math.Abs(student.Meeting1.Time - m.Time) <= 60));
                student.Meeting1.Blockings.Remove(student.Meeting1);
                student.Meeting2.Blockings.AddRange(meetings.Where(m => Math.Abs(student.Meeting2.Time - m.Time) <= 60));
                student.Meeting2.Blockings.Remove(student.Meeting2);
            }

            while (meetings.Any())
            {
                var best = meetings.OrderBy(m => m.Weight).ThenBy(m => m.Time).First();
                meetings.Remove(best);

                var other = best.Student.OtherMeeting(best);
                other.Exclude();
                meetings.Remove(other);

                foreach (var blocking in best.Blockings.ToArray())
                {
                    blocking.Exclude();
                    meetings.Remove(blocking);
                }

                best.Student.Choose(best);
            }

            var res = students.All(s => s.Choice > 0)
                ? string.Join(Environment.NewLine, students.Select(s => s.Choice))
                : "KO";
            ConsoleHelper.WriteLine(res);
        }

        public class Student
        {
            public Student(int id, int available1, int available2)
            {
                Id = id;
                Meeting1 = new Meeting(available1, this);
                Meeting2 = new Meeting(available2, this);
            }

            public int Id { get; set; }
            public Meeting Meeting1 { get; set; }
            public Meeting Meeting2 { get; set; }
            public int Choice { get; set; }

            public void Choose(Meeting meeting)
            {
                this.Choice = meeting == this.Meeting1 ? 1 : 2;
            }

            public Meeting OtherMeeting(Meeting meeting)
            {
                return meeting == Meeting1 ? Meeting2 : Meeting1;
            }
        }

        public class Meeting
        {
            public Meeting(int time, Student student)
            {
                Time = time;
                Student = student;
                Blockings = new List<Meeting>();
            }

            public int Time { get; set; }
            public Student Student { get; set; }
            public List<Meeting> Blockings { get; set; }
            public int Weight => Blockings.Count;

            public void Exclude()
            {
                foreach (var blocking in Blockings)
                    blocking.Blockings.Remove(this);

                this.Blockings.Clear();
            }

            public override string ToString()
            {
                return $"{Time} - Weight={Weight}";
            }
        }
    }
}