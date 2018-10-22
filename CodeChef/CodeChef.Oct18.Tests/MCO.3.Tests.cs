using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace CodeChef.Oct18.MCO3.Tests
{
    [TestFixture]
    public class ProgramTest
    {
        [TestCase(@"
2
6
4 16 32 6 8 2
10
16 8 7 9 6 10 32 2 4 3", @"
2
2")]
        public void Test(string input, string output)
        {
            var consoleHelperForTests = new ConsoleHelperForTests(input);
            Program.ConsoleHelper = consoleHelperForTests;

            Program.SolveMultiple();

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
