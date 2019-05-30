using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day06
{
    class Program
    {
        private const string inputFile = "..\\..\\..\\..\\input6.txt";

        static void Main(string[] args)
        {
            Console.WriteLine("Day 6");
            Console.WriteLine("Star 1");
            Console.WriteLine("");


            //The reallocation routine operates in cycles. In each cycle, it finds the memory bank 
            //  with the most blocks (ties won by the lowest-numbered memory bank) and 
            //  redistributes those blocks among the banks.

            //Cyclical reallocation starting with next block

            //The debugger would like to know how many redistributions can be done before a
            //  blocks-in-banks configuration is produced that has been seen before.

            byte[] blocks = File.ReadAllText(inputFile).Split('\t').Select(byte.Parse).ToArray();


            //Visited Configurations
            HashSet<(long, long)> visitedConfigs = new HashSet<(long, long)>();

            int steps = 0;
            while (visitedConfigs.Add(EncodeState(blocks)))
            {
                byte max = blocks.Max();
                int maxBank = 0;
                for (int i = 0; i < blocks.Length; i++)
                {
                    if (blocks[i] == max)
                    {
                        maxBank = i;
                        break;
                    }
                }

                blocks[maxBank] = 0;
                int currentIndex = (maxBank + 1) % blocks.Length;

                while (max > 0)
                {
                    blocks[currentIndex]++;
                    currentIndex = (currentIndex + 1) % blocks.Length;
                    max--;
                }

                steps++;
            }


            Console.WriteLine($"Took {steps} reconfigurations to loop");

            Console.WriteLine("");
            Console.WriteLine("Star 2");
            Console.WriteLine("");

            //Find size of loop:

            //Reset visitedConfigs
            visitedConfigs.Clear();

            //Run until loop
            steps = 0;
            while (visitedConfigs.Add(EncodeState(blocks)))
            {
                byte max = blocks.Max();
                int maxBank = 0;
                for (int i = 0; i < blocks.Length; i++)
                {
                    if (blocks[i] == max)
                    {
                        maxBank = i;
                        break;
                    }
                }

                blocks[maxBank] = 0;
                int currentIndex = (maxBank + 1) % blocks.Length;

                while (max > 0)
                {
                    blocks[currentIndex]++;
                    currentIndex = (currentIndex + 1) % blocks.Length;
                    max--;
                }

                steps++;
            }

            Console.WriteLine($"Loop is {steps} steps long.");


            Console.WriteLine("");
            Console.ReadKey();
        }

        private static (long, long) EncodeState(byte[] blocks)
        {
            if (blocks.Length != 16)
            {
                throw new ArgumentException();
            }

            long lower = 0;
            long upper = 0;

            for (int i = 0; i < 8; i++)
            {
                lower |= ((long)blocks[i] << 8 * i);
                upper |= ((long)blocks[8 + i] << 8 * i);
            }

            return (lower, upper);
        }
    }
}
