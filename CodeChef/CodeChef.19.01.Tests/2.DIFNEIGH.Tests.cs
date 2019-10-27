using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace CodeChef.Jan19.DIFNEIGH.Tests
{
    [TestFixture]
    public class ProgramTest
    {
        [TestCase(@"
4
1 1
2 3
4 4
5 5", @"
1
1
3
1 2 3
1 2 3
4
1 2 3 4
1 2 3 4
3 4 1 2
3 4 1 2
4
1 2 3 4 1
1 2 3 4 1
3 4 1 2 3
3 4 1 2 3
1 2 3 4 1")]
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
