using System;
using System.Collections.Generic;

namespace QuantumAlgorithms.Common
{
    public static class Utilities
    {
        public static bool IsEven(this int number) => number % 2 == 0;

        public static int GCD(int a, int b) => b == 0 ? a : GCD(b, a % b);

        public static bool IsCoprime(this int a, int b) => GCD(a, b) == 1;

        //public static int PickCoprime(int number) => PickCoprimeHelper(new Random(), number);

        public static int BitSize(int number)
        {
            int size = 0;
            while (number > 0)
            {
                number >>= 1;
                ++size;
            }
            return size;
        }

        public static IEnumerable<bool> BinaryIntegerRepresentation(int number, int bitSize)
        {
            for (var i = 0; i < bitSize; ++i)
            {
                if (number % 2 == 0)
                    yield return false;
                else
                    yield return true;

                number >>= 1;
            }
        }

        public static int CalculateExpMod(int exponentBase, int exponentPower, int modulus)
        {
            var result = 1;
            var expPow2Mod = exponentBase;

            foreach (var bit in BinaryIntegerRepresentation(exponentPower, BitSize(exponentPower)))
            {
                if (bit)
                {
                    result = (result * expPow2Mod) % modulus;
                }
                expPow2Mod = expPow2Mod * expPow2Mod % modulus;
            }
            return result;
        }

        //private static int PickCoprimeHelper(Random random, int number)
        //{
        //    var coprimeCandidate = random.Next(number - 2) + 2;
        //    return coprimeCandidate.IsCoprime(number) ? coprimeCandidate : PickCoprimeHelper(random, number);
        //}
    }
}
