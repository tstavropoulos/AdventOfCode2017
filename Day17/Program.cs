using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day17
{
    class Program
    {
        private const int skipNumber = 348;

        static void Main(string[] args)
        {
            Console.WriteLine("Day 17");
            Console.WriteLine("Star 1");
            Console.WriteLine("");

            List<int> buffer = new List<int>(2018) { 0 };

            int index = 0;
            for (int i = 1; i < 2018; i++)
            {
                index = ((index + skipNumber) % buffer.Count) + 1;
                buffer.Insert(index, i);
            }

            index++;
            Console.WriteLine($"The number after 2017: {((index == buffer.Count) ? 0 : buffer[index])}");


            Console.WriteLine("");
            Console.WriteLine("Star 2");
            Console.WriteLine("");

            //What is the value after zero after 50_000_000 insertions?
            //0 will be at index 0
            //We don't need to simulate it anymore
            //Just track how many numbers exist and insertions at 1

            int currentValue = 0;
            index = 0;
            for (int i = 1; i <= 50_000_000; i++)
            {
                index = ((index + skipNumber) % i) + 1;
                if (index == 1)
                {
                    currentValue = i;
                }
            }

            Console.WriteLine($"The number after 50M: {currentValue}");

            Console.WriteLine("");
            Console.ReadKey();

        }
    }
}
