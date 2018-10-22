using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;

namespace Contest.War_of_XORs
{
    #region ConsoleHelper

    public interface IConsoleHelper
    {
        string ReadLine();
        T ReadLineAs<T>();

        string[] ReadLineAndSplit(char delimiter = ' ');
        List<T> ReadLineAndSplitAsListOf<T>(char delimiter = ' ');

        void WriteLine(object obj);
        void Debug(object obj);
    }

    public class ConsoleHelper : IConsoleHelper
    {
        public virtual string ReadLine()
        {
            return Console.ReadLine();
        }

        public T ReadLineAs<T>()
        {
            var line = this.ReadLine();

            return ConvertTo<T>(line);
        }

        public string[] ReadLineAndSplit(char delimiter = ' ')
        {
            var splittedLine = this.ReadLine().Split(delimiter);
            return splittedLine;
        }

        public List<T> ReadLineAndSplitAsListOf<T>(char delimiter = ' ')
        {
            var splittedLine = this.ReadLineAndSplit();

            return splittedLine.Select(ConvertTo<T>).ToList();
        }

        public virtual void WriteLine(object obj)
        {
            Console.WriteLine(obj);
        }

        public void Debug(object obj)
        {
            Console.Error.WriteLine(obj);
        }

        private static T ConvertTo<T>(string value)
        {
            return (T) Convert.ChangeType(value, typeof(T));
        }
    }

    #endregion

    public static class Program
    {
        public static IConsoleHelper ConsoleHelper;

        static Program()
        {
            ConsoleHelper = new ConsoleHelper();
        }

        public static void Main(string[] args)
        {
            Solve();
        }

        public static void Solve()
        {
            // even integer can be expressed as the sum of two odd primes https://en.wikipedia.org/wiki/Goldbach%27s_conjecture
            // odd integer can be expressed as the sum of two numbers having same parity (sum of 2 numbers with same parity is an even integer)
            // => a pair with different parity can't be a valid pair
            // => a pair with 2th bit changed can't be a valid pair

            var t = ConsoleHelper.ReadLineAs<int>();
            for (var k = 0; k < t; k++)
            {
                var n = ConsoleHelper.ReadLineAs<int>();
                var array = ConsoleHelper.ReadLineAndSplitAsListOf<int>();

                var evenNumbers = new Dictionary<int, int>();
                var evenCount = 0;

                var oddNumbers = new Dictionary<int, int>();
                var oddCount = 0;

                for (var i = 0; i < n; i++)
                {
                    if (array[i] % 2 == 0)
                    {
                        AddToDico(array[i], evenNumbers);
                        evenCount++;
                    }
                    else
                    {
                        AddToDico(array[i], oddNumbers);
                        oddCount++;
                    }
                }

                var validPairs = 0;
                validPairs += CountValidPairs(evenNumbers, evenCount);
                validPairs += CountValidPairs(oddNumbers, oddCount);

                ConsoleHelper.WriteLine(validPairs);
            }
        }

        private static int CountValidPairs(IDictionary<int, int> sameParityDictionary, int count)
        {
            var validPairs = 0;
            foreach (var number in sameParityDictionary.ToList())
            {
                sameParityDictionary.Remove(number.Key);
                count -= number.Value;

                var validPair = count;
                var incompatiblePairNumber = GetIncompatiblePairNumber(number);
                if (sameParityDictionary.ContainsKey(incompatiblePairNumber))
                    validPair -= sameParityDictionary[incompatiblePairNumber];

                validPairs += validPair * number.Value;
            }
            return validPairs;
        }

        private static int GetIncompatiblePairNumber(KeyValuePair<int, int> number)
        {
            var secondBit = number.Key & 2;
            // secondBit == 0 => +2
            // secondBit == 2 => -2

            var incompatibleNumber = number.Key + 2*(1-secondBit);
            return incompatibleNumber;
        }

        private static void AddToDico(int number, IDictionary<int, int> dico)
        {
            if (dico.ContainsKey(number))
                dico[number]++;
            else
                dico[number] = 1;
        }
    }
}