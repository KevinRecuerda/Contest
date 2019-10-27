using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Contest.MDF19.Ex2.Tests
{
    [TestFixture]
    public class ProgramTest
    {
        [TestCase(@"
6 8
5 6
1 2
2 3
3 4
5 1
4 5
1 4
1 5
2 1
1 3", @"
4")]
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
