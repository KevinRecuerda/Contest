using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace CodeChef.April19.MCO3.Tests
{
    [TestFixture]
    public class ProgramTest
    {
        [TestCase(@"
5
3 10 15 5 10 100
12 15 18
5 10 15 5 10 100
12 15 25 25 25
5 10 15 5 10 100
12 15 24 24 24
3 10 15 5 10 100
5 5 10
4 40 80 30 30 100
100 100 100 100", @"
4
2
3
RIP
1")]
        public void Test(string input, string output)
        {
            var consoleHelperForTests = new ConsoleHelperForTests(input);
            Program.ConsoleHelper = consoleHelperForTests;

            Program.Main(null);

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
