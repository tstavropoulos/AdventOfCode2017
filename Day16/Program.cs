using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day16
{
    class Program
    {
        private const string inputFile = "..\\..\\..\\..\\input16.txt";

        static void Main(string[] args)
        {
            Console.WriteLine("Day 16");
            Console.WriteLine("Star 1");
            Console.WriteLine("");

            List<char> programs = new List<char> {
                'a', 'b', 'c', 'd',
                'e', 'f', 'g', 'h',
                'i', 'j', 'k', 'l',
                'm', 'n', 'o', 'p'};

            IEnumerable<string> orders = File.ReadAllText(inputFile).Split(',');

            foreach (string order in orders)
            {
                ExecuteOrder(programs, order);
            }

            Console.WriteLine($"Final order: {new string(programs.ToArray())}");


            Console.WriteLine("");
            Console.WriteLine("Star 2");
            Console.WriteLine("");

            //Step 1: Figure out the mapping

            //How long until we cycle back to starting order?
            int loopSize = 0;

            for (int i = 2; i < 1_000_000; i++)
            {
                foreach (string order in orders)
                {
                    ExecuteOrder(programs, order);
                }

                if (new string(programs.ToArray()) == "abcdefghijklmnop")
                {
                    Console.WriteLine($"Loop Detected at: {i}");
                    loopSize = i;
                    break;
                }
            }

            //Need to do 1 billion loops.
            long remainingLoops = 1_000_000_000 % loopSize;

            //Reset programs
            programs = new List<char> {
                'a', 'b', 'c', 'd',
                'e', 'f', 'g', 'h',
                'i', 'j', 'k', 'l',
                'm', 'n', 'o', 'p' };

            for (int i = 0; i < remainingLoops; i++)
            {
                foreach (string order in orders)
                {
                    ExecuteOrder(programs, order);
                }
            }

            Console.WriteLine($"Final order: {new string(programs.ToArray())}");

            Console.WriteLine("");
            Console.ReadKey();
        }

        public static void ExecuteOrder(List<char> programs, string order)
        {
            switch (order[0])
            {
                case 's':
                    //Spin
                    int value = int.Parse(order.Substring(1));
                    for (int i = 0; i < value; i++)
                    {
                        char c = programs[15];
                        programs.RemoveAt(15);
                        programs.Insert(0, c);
                    }
                    break;

                case 'x':
                    //Swap
                    string[] swapSplit = order.Substring(1).Split('/');
                    int swap1 = int.Parse(swapSplit[0]);
                    int swap2 = int.Parse(swapSplit[1]);
                    char swapTemp = programs[swap1];
                    programs[swap1] = programs[swap2];
                    programs[swap2] = swapTemp;
                    break;

                case 'p':
                    //Partner
                    int partner1 = programs.IndexOf(order[1]);
                    int partner2 = programs.IndexOf(order[3]);
                    char partnerTemp = programs[partner1];
                    programs[partner1] = programs[partner2];
                    programs[partner2] = partnerTemp;
                    break;

                default:
                    throw new Exception();
            }
        }
    }
}
