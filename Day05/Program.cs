using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day05
{
    class Program
    {
        private const string inputFile = "..\\..\\..\\..\\input5.txt";

        static void Main(string[] args)
        {
            Console.WriteLine("Day 5");
            Console.WriteLine("Star 1");
            Console.WriteLine("");

            int[] instructions = File.ReadAllLines(inputFile).Select(int.Parse).ToArray();

            int position = 0;
            int steps = 0;

            while (position >= 0 && position < instructions.Length)
            {
                position += instructions[position]++;
                steps++;
            }

            Console.WriteLine($"Took {steps} steps to exit.");


            Console.WriteLine("");
            Console.WriteLine("Star 2");
            Console.WriteLine("");

            //Now, if the jump is 3 or more, decrement it by 1

            //Reset instructions
            instructions = File.ReadAllLines(inputFile).Select(int.Parse).ToArray();


            position = 0;
            steps = 0;

            while (position >= 0 && position < instructions.Length)
            {
                if (instructions[position] >= 3)
                {
                    position += instructions[position]--;
                }
                else
                {
                    position += instructions[position]++;
                }

                steps++;
            }


            Console.WriteLine($"Took {steps} steps to exit.");


            Console.WriteLine("");
            Console.ReadKey();
        }
    }
}
