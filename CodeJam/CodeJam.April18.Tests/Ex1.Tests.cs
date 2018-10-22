using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeJam.April18.Ex1.Tests
{
    [TestFixture]
    public class ProgramTest
    {
        [TestCase(@"
6
1 CS
2 CS
1 SS
6 SCCSSC
2 CC
3 CSCSS", @"
Case #1: 1
Case #2: 0
Case #3: IMPOSSIBLE
Case #4: 2
Case #5: 0
Case #6: 5")]
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
