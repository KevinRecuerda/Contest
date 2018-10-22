using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BattleDev.March18.Ex5.Tests
{
    [TestFixture]
    public class ProgramTest
    {
        [TestCase(@"
4
0 2 6 1
2 0 8 9
6 8 0 3
1 9 3 0", @"
9")]
        [TestCase(@"
8
0 61 81 26 95 80 27 90
61 0 36 23 26 13 63 51
81 36 0 24 29 86 68 24
26 23 24 0 73 72 14 41
95 26 29 73 0 17 47 71
80 13 86 72 17 0 44 28
27 63 68 14 47 44 0 35
90 51 24 41 71 28 35 0", @"
312")]
        public void Test(string input, string output)
        {
            var consoleHelperForTests = new ConsoleHelperForTests(input);
            Program.ConsoleHelper = consoleHelperForTests;

            Program.Solve();

            Assert.AreEqual(output.Trim(), consoleHelperForTests.Output.First());
        }

        public class ConsoleHelperForTests : ConsoleHelper
        {
            private readonly string[] input;
            private int readIndex;
            public readonly List<string> Output;

            public ConsoleHelperForTests(string input)
                : this(input.Split(new[] { Environment.NewLine, "\n" }, StringSplitOptions.RemoveEmptyEntries))
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
