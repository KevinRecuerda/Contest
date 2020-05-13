using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeChef._2020_05.NRWP.Tests
{
    [TestFixture]
    public class ProgramTest
    {
        [TestCase("codechef", @"
1
2 2 3
1 4
-1 -2
1 1 3
1 2 1
2 2 2", @"
12
1 1 -1")]
        [TestCase("duo", @"
4
1 2 2
1 1
1 1 3
1 2 4
1 2 2
-1 -1
1 1 3
1 2 4
1 2 2
-1 1
1 1 3
1 2 4
1 2 2
-10 10
1 1 3
1 2 4", @"
19
1 1
19
-1 -1
13
1 1
58
-1 1")]
        [TestCase("trio", @"
4
1 3 3
-1 1 -1
1 1 5
1 2 6
1 3 7
1 3 3
-100 100 -100
1 1 5
1 2 6
1 3 7
1 3 3
-5 5 -5
1 1 5
1 2 6
1 3 7
1 3 3
5 5 -5
1 1 5
1 2 6
1 3 7", @"
78
-1 -1 -1
1728
-1 1 -1
102
-1 -1 -1
92
1 1 1")]
        public void Test(string _, string input, string output)
        {
            var consoleHelperForTests = new ConsoleHelperForTests(input);
            Program.ConsoleHelper = consoleHelperForTests;

            Program.Main(null);

            Assert.AreEqual(output.Trim(), string.Join(Environment.NewLine, consoleHelperForTests.Output));
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