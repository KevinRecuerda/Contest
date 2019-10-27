using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeChef.October2019.SAKTAN.Tests
{
    [TestFixture]
    public class ProgramTest
    {
        [TestCase(@"
1
2 2 3
1 1
1 2
2 1", @"
2")]
        [TestCase(@"
1
3 3 3
1 1
2 2
3 3", @"
0")]
        [TestCase(@"
1
3 4 2
1 4
3 2", @"
6")]
        public void Test(string input, string output)
        {
            var consoleHelperForTests = new ConsoleHelperForTests(input);
            Program.ConsoleHelper = consoleHelperForTests;

            Program.Main(null);

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
