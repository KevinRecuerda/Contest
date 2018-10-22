using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Contest.CodinGame.AStarCraft.Tests
{
    [TestFixture]
    public class ProgramTest
    {
        [TestCase(@"
###################
###################
###################
###################
###............####
###################
###################
###################
###################
###################
1
3 4 R", @"
14 4 L",
            TestName = "01. Simple")]
        [TestCase(@"
###################
###################
###################
###################
###............####
###################
###################
###################
###################
###################
1
3 4 L", @"
3 4 R 14 4 L",
            TestName = "03. Robot buggu�")]
        [TestCase(@"
###################
#.................#
#.###############.#
#.#.............#.#
#.#.###########.#.#
#.#.###########.#.#
#.#.............#.#
#.###############.#
#.................#
###################
2
1 1 R
15 6 L", @"
17 1 R 1 1 D 1 8 R 17 8 U 3 6 R 15 6 U 15 3 L 3 3 D",
            TestName = "06. Rond point")]
        [TestCase(@"
#########.#########
#########D#########
#########.#########
#########.#########
......R......R.....
#########.#########
#########.#########
#########D#########
#########.#########
#########.#########
1
10 4 R", @"
9 4 D 9 3 U",
            TestName = "08. Portal")]
        public void Test(string input, string output)
        {
            var consoleHelperForTests = new ConsoleHelperForTests(input);
            Program.ConsoleHelper = consoleHelperForTests;

            Program.Solve();

            Assert.AreEqual(output.Trim(), consoleHelperForTests.Output.First());
        }

        [TestCase(@"
###################
###################
###################
###################
###R...........####
###################
###################
###################
###################
###################",
            1, @"
0000000000000000000
0000000000000000000
0000000000000000000
0000000000000000000
0001111111111110000
0000000000000000000
0000000000000000000
0000000000000000000
0000000000000000000
0000000000000000000",
            TestName = "I. Simple")]
        [TestCase(@"
###################
###################
###R...........####
###################
###################
###################
###################
###...........L####
###################
###################",
             2, @"
0000000000000000000
0000000000000000000
0001111111111110000
0000000000000000000
0000000000000000000
0000000000000000000
0000000000000000000
0002222222222220000
0000000000000000000
0000000000000000000",
            TestName = "II. Multiple 2")]
        [TestCase(@"
###################
#R..#.D.#..L#...###
#...#...#...#.D.###
#...#...#...#...###
###################
#...#...#...#...###
#...#...#...#.U.###
#R..#.U.#..L#...###
###################
###################",
            8, @"
0000000000000000000
0111022203330444000
0111022203330444000
0111022203330444000
0000000000000000000
0555066607770888000
0555066607770888000
0555066607770888000
0000000000000000000
0000000000000000000",
            TestName = "II. Multiple 8")]
        [TestCase(@"
##.################
#...###############
#...###############
#...###############
###################
###################
###################
###################
###################
###################",
           1, @"
0010000000000000000
0111000000000000000
0111000000000000000
0111000000000000000
0000000000000000000
0000000000000000000
0000000000000000000
0000000000000000000
0000000000000000000
0000000000000000000",
            TestName = "III. Complex")]
        [TestCase(@"
###################
........L#R........
#u.......#.......u#
.........#.........
#u.......#.......u#
#d.......#.......d#
.........#.........
#d.......#.......d#
........L#R........
###################",
            1, @"
0000000000000000000
1111111110111111111
0111111110111111110
1111111110111111111
0111111110111111110
0111111110111111110
1111111110111111111
0111111110111111110
1111111110111111111
0000000000000000000",
            TestName = "IV. Tore")]
        [TestCase(@"
l..#############..u
...#############...
..L#############U..
###################
###################
###################
###################
..D#############R..
...#############...
d..#############..r",
            1, @"
1110000000000000111
1110000000000000111
1110000000000000111
0000000000000000000
0000000000000000000
0000000000000000000
0000000000000000000
1110000000000000111
1110000000000000111
1110000000000000111",
            TestName = "IV. Tore bis")]
        public void Should_manage_groups(string map, int groupCount, string group)
        {
            var consoleHelperForTests = new ConsoleHelperForTests(map);
            Program.ConsoleHelper = consoleHelperForTests;

            Program.ReadMap();

            var actual = Program.GroupToString();
            Assert.AreEqual(groupCount, Program.groupCount);
            Assert.AreEqual(group, actual);
        }

        public class ConsoleHelperForTests : ConsoleHelper
        {
            private readonly string[] input;
            private int readIndex;
            public readonly List<string> Output;

            public ConsoleHelperForTests(string input)
                : this(input.Split(new[] {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries))
            {
            }

            public ConsoleHelperForTests(string[] input)
            {
                this.input = input;
                this.readIndex = 0;
                this.Output = new List<string>();
            }

            public override string ReadLine()
            {
                if (this.readIndex < this.input.Length)
                    return this.input[this.readIndex++];

                return string.Empty;
            }

            public override void WriteLine(object obj)
            {
                this.Output.Add(obj.ToString());
            }
        }
    }
}