using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day13
{
    class Program
    {
        private const string inputFile = "..\\..\\..\\..\\input13.txt";

        static void Main(string[] args)
        {
            Console.WriteLine("Day 13");
            Console.WriteLine("Star 1");
            Console.WriteLine("");

            List<Scanner> scanners = File.ReadLines(inputFile).Select(x => new Scanner(x)).ToList();

            int maxLayer = scanners.Select(x => x.layer).Max();
            int totalSeverity = 0;

            for (int playerLayer = 0; playerLayer < maxLayer + 1; playerLayer++)
            {
                totalSeverity += scanners.Select(x => x.GetSeverity(playerLayer)).Sum();
                scanners.ForEach(x => x.Update());
            }

            Console.WriteLine($"Total Severity: {totalSeverity}");

            Console.WriteLine("");
            Console.WriteLine("Star 2");
            Console.WriteLine("");

            long offset = -1;
            int report = 100;
            List<Attempt> activeAttempts = new List<Attempt>();
            scanners.ForEach(x => x.Reset());

            while (!activeAttempts.Any(x=>x.layer > maxLayer))
            {
                offset++;

                if (offset == report)
                {
                    report *= 2;
                    Console.WriteLine($"  Delay: {offset}");
                }

                activeAttempts.Add(new Attempt(offset));

                for (int i = activeAttempts.Count - 1; i >= 0; i--)
                {
                    if (scanners.Any(x => x.IsCaught(activeAttempts[i].layer)))
                    {
                        activeAttempts.RemoveAt(i);
                    }
                }

                scanners.ForEach(x => x.Update());
                activeAttempts.ForEach(x => x.Update());
            }

            Console.WriteLine($"Minimum Delay: {activeAttempts.Where(x=>x.layer > maxLayer).First().delay}");

            Console.WriteLine("");
            Console.ReadKey();
        }

        class Attempt
        {
            public long delay;
            public int layer;

            public Attempt(long delay)
            {
                this.delay = delay;
                layer = 0;
            }

            public void Update()
            {
                layer++;
            }
        }

        class Scanner
        {
            public readonly int depth;
            public readonly int layer;

            public int scan;

            int delta;

            public Scanner(string line)
            {
                string[] splitLine = line.Split(": ");

                layer = int.Parse(splitLine[0]);
                depth = int.Parse(splitLine[1]);

                scan = 0;
                delta = 1;
            }

            public Scanner(int depth, int layer)
            {
                this.depth = depth;
                this.layer = layer;

                scan = 0;
                delta = 1;
            }

            public void Update()
            {
                scan += delta;

                if (scan == 0)
                {
                    delta = 1;
                }
                else if (scan == depth - 1)
                {
                    delta = -1;
                }
            }

            public int GetSeverity(int playerLayer)
            {
                if (playerLayer != layer)
                {
                    return 0;
                }

                if (scan != 0)
                {
                    return 0;
                }

                return depth * layer;
            }

            public bool IsCaught(int playerLayer)
            {
                if (playerLayer != layer)
                {
                    return false;
                }

                return scan == 0;
            }

            public void Reset()
            {
                scan = 0;
                delta = 1;
            }
        }
    }
}
