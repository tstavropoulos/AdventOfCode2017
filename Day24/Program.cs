using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day24
{
    class Program
    {
        private const string inputFile = "..\\..\\..\\..\\input24.txt";

        static void Main(string[] args)
        {
            Console.WriteLine("Day 24");
            Console.WriteLine("Star 1");
            Console.WriteLine("");

            List<(int a, int b)> connectors = File.ReadAllLines(inputFile).Select(Parse).ToList();

            int lastConnection = 0;

            int maxStrength = FindMaxConnection(connectors, 0);

            Console.WriteLine($"Max total strength: {maxStrength}");

            Console.WriteLine("");
            Console.WriteLine("Star 2");
            Console.WriteLine("");

            maxStrength = 0;
            int maxDepth = 0;
            FindMaxLengthConnection(connectors, 0, 0, 0, ref maxDepth, ref maxStrength);

            Console.WriteLine($"Max strength of max length: {maxStrength}");

            Console.WriteLine("");
            Console.ReadKey();
        }

        private static int FindMaxConnection(List<(int a, int b)> connectors, int lastConnection)
        {
            int max = 0;
            foreach ((int a, int b) fit in connectors
                .Where(x => x.a == lastConnection || x.b == lastConnection)
                .ToArray())
            {
                connectors.Remove(fit);
                if (fit.a == lastConnection)
                {
                    max = Math.Max(max, fit.a + fit.b + FindMaxConnection(connectors, fit.b));
                }
                else
                {
                    max = Math.Max(max, fit.a + fit.b + FindMaxConnection(connectors, fit.a));
                }
                connectors.Add(fit);
            }

            return max;
        }

        private static void FindMaxLengthConnection(
            List<(int a, int b)> connectors,
            int lastConnection,
            int depth,
            int strength,
            ref int maxDepth,
            ref int maxStrength)
        {
            if (depth > maxDepth)
            {
                maxDepth = depth;
            }

            if (depth == maxDepth && strength > maxStrength)
            {
                maxStrength = strength;
            }


            foreach ((int a, int b) fit in connectors
                .Where(x => x.a == lastConnection || x.b == lastConnection)
                .ToArray())
            {
                connectors.Remove(fit);
                if (fit.a == lastConnection)
                {
                    FindMaxLengthConnection(connectors, fit.b, depth + 1, strength + fit.a + fit.b, ref maxDepth, ref maxStrength);
                }
                else
                {
                    FindMaxLengthConnection(connectors, fit.a, depth + 1, strength + fit.a + fit.b, ref maxDepth, ref maxStrength);
                }
                connectors.Add(fit);
            }
        }

        private static (int a, int b) Parse(string line)
        {
            string[] splitLine = line.Split('/');

            return (int.Parse(splitLine[0]), int.Parse(splitLine[1]));
        }
    }
}
