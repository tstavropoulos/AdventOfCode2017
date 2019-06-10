using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;

namespace Day15
{
    class Program
    {
        private const int GEN_A_START = 873;
        private const int GEN_B_START = 583;

        private const int GEN_A_FACTOR = 16807;
        private const int GEN_B_FACTOR = 48271;

        private const int GEN_A_TEST_START = 65;
        private const int GEN_B_TEST_START = 8921;

        static void Main(string[] args)
        {
            Console.WriteLine("Day 15");
            Console.WriteLine("Star 1");
            Console.WriteLine("");

            long genA = GEN_A_START;
            long genB = GEN_B_START;

            int matches = 0;

            for (long i = 0; i < 40_000_000; i++)
            {
                genA *= GEN_A_FACTOR;
                genA %= int.MaxValue;

                genB *= GEN_B_FACTOR;
                genB %= int.MaxValue;

                if ((genA & 0b1111_1111_1111_1111) == (genB & 0b1111_1111_1111_1111))
                {
                    matches++;
                }

            }

            Console.WriteLine($"Matches in the first 40M: {matches}");

            Console.WriteLine("");
            Console.WriteLine("Star 2");
            Console.WriteLine("");

            IEnumerable<int> generatorA = GenerateNumbers(GEN_A_START, GEN_A_FACTOR, 4);
            IEnumerable<int> generatorB = GenerateNumbers(GEN_B_START, GEN_B_FACTOR, 8);

            int count = generatorA.Zip(generatorB, (x, y) => x == y).Take(5_000_000).Count(x => x);

            Console.WriteLine($"New matches in the first 5M: {count}");

            Console.WriteLine("");
            Console.ReadKey();
        }

        static IEnumerable<int> GenerateNumbers(int seed, int factor, int checker)
        {
            long value = seed;

            while (true)
            {
                value *= factor;
                value %= int.MaxValue;

                if (value % checker == 0)
                {
                    yield return (int)(value & 0b1111_1111_1111_1111);
                }
            }
        }
    }
}
