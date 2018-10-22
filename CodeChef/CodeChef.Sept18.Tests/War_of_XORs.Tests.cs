using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Contest.War_of_XORs.Tests
{
    [TestFixture]
    public class ProgramTest
    {
        [TestCase(@"
5
5
2 4 8 1 3
6
2 4 8 1 3 2
6
2 4 8 1 3 5
6
2 4 8 1 3 0
8
2 4 8 1 3 5 3 5", @"
3
5
5
5
9")]
        public void Test(string input, string output)
        {
            var consoleHelperForTests = new ConsoleHelperForTests(input);
            Program.ConsoleHelper = consoleHelperForTests;

            Program.Solve();

            var actual = string.Join(Environment.NewLine, consoleHelperForTests.Output);
            Assert.AreEqual(output.Trim(), actual);
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
