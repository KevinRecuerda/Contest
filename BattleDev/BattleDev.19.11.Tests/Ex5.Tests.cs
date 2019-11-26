using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BattleDev._2019_11.Ex5.Tests
{
    [TestFixture]
    public class ProgramTest
    {
        [TestCase(@"
A
Earaindir Rithralas
Hilad Fioldor
Delanduil Rithralas
Urarion Elrebrimir
Elrebrimir Fioldor
Eowul Fioldor
Beladrieng Anaramir
Urarion Eowul
Earaindir Sanakil
Delanduil Isilmalad
Earylas Isilmalad
Rithralas Sanakil
Unithral Elrebrimir
Earylas Eowul
Beladrieng Hilad
Isilmalad Sanakil
Unithral Earylas
Earaindir Anaramir
Unithral Beladrieng
Hilad Anaramir
Delanduil Urarion", @"
Delanduil")]
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
