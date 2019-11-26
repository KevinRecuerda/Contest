using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BattleDev._2019_11.Ex3.Tests
{
    [TestFixture]
    public class ProgramTest
    {
        [TestCase(@"
6 7
1 3
1 4
1 5
1 6
1 7
2 9
3 11", @"
1 2 3 4 5 6 1")]
        [TestCase(@"
6 7
1 3
1 4
2 8
1 5
1 6
1 7
1 9", @"
pas possible")]
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
                : this(input.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries))
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
