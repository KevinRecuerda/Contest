using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace Contest.CodinGame.AStarCraft
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

        public const int N = 10;
        public const int M = 19;

        public static char[][] map = new char[N][];

        public static int groupCount = 0;
        public static int groupIndex = 0;
        public static int[,] group = new int[N, M];

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
            ReadMap();

            var robotCount = ConsoleHelper.ReadLineAs<int>();
            var robots = new Robot[robotCount];

            for (var k = 0; k < robotCount; k++)
            {
                var input = ConsoleHelper.ReadLineAndSplit();
                var x = int.Parse(input[0]);
                var y = int.Parse(input[1]);
                var direction = input[2].ToCharArray()[0];
                var robotGroup = group[y, x];

                var robot = new Robot(k, x, y, direction, robotGroup);
                robots[k] = robot;
            }

            var commands = new List<Command>();
            foreach (var robotGroup in robots.GroupBy(r => r.Group))
            {
                var groupCommands = Solve(robotGroup.Key, robotGroup.ToArray());
                commands.AddRange(groupCommands);
            }

            ConsoleHelper.WriteLine(string.Join(" ", commands));
        }

        public static IEnumerable<Command> Solve(int groupId, params Robot[] robots)
        {
            var commands = new List<Command>();

            var score = CalculateScore(robots);

            foreach (var robot in robots)
            {
                var subScore = Explore(robot, robots, null, null, true, out var subCommands);
                if (subScore > score)
                    commands.AddRange(subCommands);
            }

            return commands;
        }

        private static int Explore(
            Robot robot,
            Robot[] robots,
            HashSet<State> parentVistedStates,
            HashSet<Cell> parentVistedCells,
            bool isFirst,
            out List<Command> commands)
        {
            commands = new List<Command>();
            var score = 0;

            robot = robot.Clone();
            robot.Direction = map[robot.Y][robot.X] != '.' ? map[robot.Y][robot.X] : robot.Direction;

            var visitedStates =
                parentVistedStates != null
                    ? new HashSet<State>(parentVistedStates)
                    : new HashSet<State>(new List<State> {new State(robot)});
            var visitedCells =
                parentVistedCells != null
                    ? new HashSet<Cell>(parentVistedCells)
                    : new HashSet<Cell>();

            while (true)
            {
                if (!isFirst)
                {
                    score++;

                    robot.Move();

                    var pos = map[robot.Y][robot.X];
                    if (pos != '.' && pos != '#')
                        robot.Direction = pos;

                    var state = new State(robot);
                    if (pos == '#' || visitedStates.Contains(state))
                        break;

                    visitedStates.Add(state);
                }

                isFirst = false;

                var cell = new Cell(robot);
                if (visitedCells.Contains(cell))
                    continue;
                else
                    visitedCells.Add(cell);

                var down = map[(cell.Y + 1) % N][cell.X];
                var up = map[(cell.Y - 1 + N) % N][cell.X];
                var right = map[cell.Y][(cell.X + 1) % M];
                var left = map[cell.Y][(cell.X - 1 + M) % M];

                var directions = new List<char>();
                if (down != '#' && robot.Direction != 'D' && (robot.Direction != 'U' || up == '#')) directions.Add('D');
                if (up != '#' && robot.Direction != 'U' && (robot.Direction != 'D' || down == '#')) directions.Add('U');
                if (right != '#' && robot.Direction != 'R' && (robot.Direction != 'L' || left == '#'))
                    directions.Add('R');
                if (left != '#' && robot.Direction != 'L' && (robot.Direction != 'R' || right == '#'))
                    directions.Add('L');

                if (directions.Count > 0)
                {
                    var subScore = Explore(robot, robots, visitedStates, visitedCells, false, out var subCommands);
                    ;
                    foreach (var direction in directions)
                    {
                        map[cell.Y][cell.X] = direction;
                        var subState = new State(robot) {Direction = direction};
                        visitedStates.Add(subState);
                        var subScoreForDirection = Explore(robot, robots, visitedStates, visitedCells, false,
                            out var subCommandsForDirection);
                        visitedStates.Remove(subState);

                        var command = new Command(cell.X, cell.Y, direction);
                        subCommandsForDirection.Add(command);

                        if (subScoreForDirection > subScore)
                        {
                            subScore = subScoreForDirection;
                            subCommands = subCommandsForDirection;
                        }
                    }

                    score += subScore;
                    commands = subCommands;
                    break;
                }
            }

            return score;
        }

        public static int CalculateScore(params Robot[] robots)
        {
            var score = 0;
            var remainingRobots = robots.Select(r => r.Clone()).ToList();
            remainingRobots.ForEach(r => r.Direction = map[r.Y][r.X] != '.' ? map[r.Y][r.X] : r.Direction);

            var vistedStates = robots.ToDictionary(
                r => r.Id,
                r => new HashSet<State>(new List<State> {new State(r)}));

            while (remainingRobots.Count > 0)
            {
                score += remainingRobots.Count;

                foreach (var robot in remainingRobots.ToList())
                {
                    robot.Move();

                    var pos = map[robot.Y][robot.X];
                    if (pos != '.' && pos != '#')
                        robot.Direction = pos;

                    var state = new State(robot);
                    if (pos == '#' || vistedStates[robot.Id].Contains(state))
                    {
                        remainingRobots.Remove(robot);
                    }
                    else
                        vistedStates[robot.Id].Add(state);
                }
            }

            return score;
        }

        public static List<State> Trace(Robot robot)
        {
            robot = robot.Clone();
            robot.Direction = map[robot.Y][robot.X] != '.' ? map[robot.Y][robot.X] : robot.Direction;

            var states = new List<State> {new State(robot)};

            var vistedStates = new HashSet<State>(states);

            while (true)
            {
                robot.Move();

                var pos = map[robot.Y][robot.X];
                if (pos != '.' && pos != '#')
                    robot.Direction = pos;

                var state = new State(robot);
                if (pos == '#' || vistedStates.Contains(state))
                    break;

                states.Add(state);
                vistedStates.Add(state);
            }

            return states;
        }

        #region Input

        public static string GroupToString()
        {
            var stringBuilder = new StringBuilder();
            for (var i = 0; i < N; i++)
            {
                stringBuilder.AppendLine();
                for (var j = 0; j < M; j++)
                    stringBuilder.Append(group[i, j]);
            }

            return stringBuilder.ToString();
        }

        public static string MapToString()
        {
            var stringBuilder = new StringBuilder();
            for (var i = 0; i < N; i++)
            {
                stringBuilder.AppendLine();
                for (var j = 0; j < M; j++)
                    stringBuilder.Append(map[i][j]);
            }

            return stringBuilder.ToString();
        }

        public static void ReadMap()
        {
            map = new char[N][];

            groupCount = 0;
            groupIndex = 0;
            group = new int[N, M];

            for (var i = 0; i < N; i++)
            {
                map[i] = ConsoleHelper.ReadLine().ToCharArray();
                for (var j = 0; j < M; j++)
                {
                    var cellGroup = 0;
                    if (map[i][j] != '#')
                    {
                        var leftCellGroup = j > 0 ? group[i, j - 1] : 0;
                        var upCellGroup = i > 0 ? group[i - 1, j] : 0;

                        cellGroup = MergeGroup(leftCellGroup, upCellGroup);
                        if (cellGroup == 0)
                        {
                            groupCount++;
                            groupIndex++;
                            cellGroup = groupIndex;
                        }

                        // Manage tore map
                        if (i == N - 1)
                        {
                            var downCellGroup = group[0, j];
                            cellGroup = MergeGroup(cellGroup, downCellGroup);
                        }

                        if (j == M - 1)
                        {
                            var rightCellGroup = group[i, 0];
                            cellGroup = MergeGroup(cellGroup, rightCellGroup);
                        }
                    }

                    group[i, j] = cellGroup;
                }
            }
        }

        private static int MergeGroup(int id1, int id2)
        {
            if (id1 == id2)
                return id1;

            if (id1 == 0)
                return id2;

            if (id2 == 0)
                return id1;

            var idToKeep = id1;
            var idToReplace = id2;
            if (id2 < id1)
            {
                idToKeep = id2;
                idToReplace = id1;
            }

            groupCount--;

            for (var i = 0; i < N; i++)
            for (var j = 0; j < M; j++)
            {
                if (group[i, j] == idToReplace)
                    group[i, j] = idToKeep;
            }

            return idToKeep;
        }

        #endregion
    }

    public class Robot
    {
        public Robot(int id, int x, int y, char direction, int group)
        {
            this.Id = id;
            this.X = x;
            this.Y = y;
            this.Direction = direction;
            this.Group = group;
        }

        public int Id { get; }
        public int X { get; set; }
        public int Y { get; set; }
        public char Direction { get; set; }
        public int Group { get; set; }

        public void Move()
        {
            switch (Direction)
            {
                case 'R':
                    this.X = (this.X + 1) % Program.M;
                    break;
                case 'D':
                    this.Y = (this.Y + 1) % Program.N;
                    break;
                case 'L':
                    this.X = (this.X - 1 + Program.M) % Program.M;
                    break;
                case 'U':
                    this.Y = (this.Y - 1 + Program.N) % Program.N;
                    break;
            }
        }

        public Robot Clone()
        {
            return this.MemberwiseClone() as Robot;
        }

        public override string ToString()
        {
            return $"{this.X} {this.Y} {this.Direction} | Group={this.Group}";
        }
    }

    public class State
    {
        public State(Robot robot)
        {
            this.X = robot.X;
            this.Y = robot.Y;
            this.Direction = robot.Direction;
        }

        public int X { get; set; }
        public int Y { get; set; }
        public char Direction { get; set; }

        public override bool Equals(object obj)
        {
            var state = obj as State;
            return state != null &&
                   X == state.X &&
                   Y == state.Y &&
                   Direction == state.Direction;
        }

        public override int GetHashCode()
        {
            var hashCode = 1889109973;
            hashCode = hashCode * -1521134295 + X.GetHashCode();
            hashCode = hashCode * -1521134295 + Y.GetHashCode();
            hashCode = hashCode * -1521134295 + Direction.GetHashCode();
            return hashCode;
        }

        public override string ToString()
        {
            return $"{this.X} {this.Y} {this.Direction}";
        }
    }

    public class Cell
    {
        public Cell(Robot robot)
        {
            this.X = robot.X;
            this.Y = robot.Y;
        }

        public int X { get; set; }
        public int Y { get; set; }

        public override bool Equals(object obj)
        {
            var cell = obj as Cell;
            return cell != null &&
                   X == cell.X &&
                   Y == cell.Y;
        }

        public override int GetHashCode()
        {
            var hashCode = 1861411795;
            hashCode = hashCode * -1521134295 + X.GetHashCode();
            hashCode = hashCode * -1521134295 + Y.GetHashCode();
            return hashCode;
        }

        public override string ToString()
        {
            return $"{this.X} {this.Y}";
        }
    }

    public class Command
    {
        public Command(int x, int y, char direction)
        {
            this.X = x;
            this.Y = y;
            this.Direction = direction;
        }

        public int X { get; set; }
        public int Y { get; set; }
        public char Direction { get; set; }

        public override string ToString()
        {
            return $"{this.X} {this.Y} {this.Direction}";
        }
    }
}