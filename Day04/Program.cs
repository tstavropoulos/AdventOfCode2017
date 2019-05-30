using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day04
{
    class Program
    {
        private const string inputFile = "..\\..\\..\\..\\input4.txt";

        static void Main(string[] args)
        {
            Console.WriteLine("Day 4");
            Console.WriteLine("Star 1");
            Console.WriteLine("");

            IEnumerable<string> passphrases = File.ReadAllLines(inputFile);

            IEnumerable<string> validPhrases = passphrases.Where(x => x.Split(' ').Count() == x.Split(' ').Distinct().Count());

            Console.WriteLine($"Valid passphrases: {validPhrases.Count()}");

            Console.WriteLine("");
            Console.WriteLine("Star 2");
            Console.WriteLine("");

            //No valid anagrams allowed now.  Just sort:

            IEnumerable<string> validPhrases2 = passphrases.Where(x => 
                x.Split(' ').Count() == x.Split(' ').Select(w => new string(w.OrderBy(c => c).ToArray())).Distinct().Count());

            Console.WriteLine($"Valid passphrases: {validPhrases2.Count()}");

            Console.WriteLine("");
            Console.ReadKey();
        }
    }
}
