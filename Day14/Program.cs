using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using CircularList = Day10.CircularList;

namespace Day14
{
    class Program
    {
        private const string inputString = "nbysizxe";
        private const string testString = "flqrgnkx";
        private static CircularList[] circularLists;

        static void Main(string[] args)
        {
            Console.WriteLine("Day 14");
            Console.WriteLine("Star 1");
            Console.WriteLine("");

            //Disk Defragmentation

            //128 x 128 grid
            //Disk state tracked as Knot Hashes
            //One knot hash for each row  (Day 10)
            //0 is free, 1 is used

            circularLists = new CircularList[128];
            for (int i = 0; i < 128; i++)
            {
                circularLists[i] = new CircularList(256);
            }

            //First row is nbysizxe-0
            //onward

            int usedBits = 0;

            for (int i = 0; i < 128; i++)
            {
                byte[] bytes = Encode(i, inputString);
                usedBits += bytes.Select(x => Convert.ToString(x, 2).Count(c => c == '1')).Sum();
            }
            //Count the ones

            Console.WriteLine($"Used bits: {usedBits}");

            Console.WriteLine("");
            Console.WriteLine("Star 2");
            Console.WriteLine("");

            //Now identify contiguous regions
            //Floodfill algorithm

            bool[,] grid = new bool[128, 128];

            for (int y = 0; y < 128; y++)
            {
                bool[] row = circularLists[y].GetDenseHashBytes().SelectMany(GetStates).ToArray();
                for (int x = 0; x < 128; x++)
                {
                    grid[x, y] = row[x];
                }
            }

            Queue<(int, int)> pendingPoints = new Queue<(int, int)>();
            int regions = 0;

            while (true)
            {
                //Find next point to start with
                for (int y = 0; y < 128; y++)
                {
                    for (int x = 0; x < 128; x++)
                    {
                        if (grid[x, y])
                        {
                            pendingPoints.Enqueue((x, y));
                            regions++;
                            goto doublebreak;
                        }
                    }
                }

                //Nothing found
                break;

            doublebreak:
                while (pendingPoints.Count > 0)
                {
                    (int x, int y) = pendingPoints.Dequeue();
                    grid[x, y] = false;

                    if (IsValid(x + 1, y, grid, pendingPoints))
                    {
                        pendingPoints.Enqueue((x + 1, y));
                    }

                    if (IsValid(x - 1, y, grid, pendingPoints))
                    {
                        pendingPoints.Enqueue((x - 1, y));
                    }

                    if (IsValid(x, y + 1, grid, pendingPoints))
                    {
                        pendingPoints.Enqueue((x, y + 1));
                    }

                    if (IsValid(x, y - 1, grid, pendingPoints))
                    {
                        pendingPoints.Enqueue((x, y - 1));
                    }
                }
            }

            Console.WriteLine($"Found {regions} regions.");

            Console.WriteLine("");
            Console.ReadKey();
        }

        private static byte[] Encode(int index, string input)
        {
            IEnumerable<int> inputValues = Encoding.UTF8.GetBytes($"{input}-{index}")
                .Select(x => (int)x)
                .Concat(new int[] { 17, 31, 73, 47, 23 })
                .ToArray();

            for (int i = 0; i < 64; i++)
            {
                foreach (int length in inputValues)
                {
                    circularLists[index].Twist(length);
                }
            }

            return circularLists[index].GetDenseHashBytes();
        }

        private static IEnumerable<bool> GetStates(byte input)
        {
            int filter = 1 << 7;
            for (int i = 0; i < 8; i++)
            {
                yield return (input & (filter >> i)) != 0;
            }
        }

        private static bool IsValid(int x, int y, bool[,] grid, Queue<(int, int)> pendingPoints)
        {
            if (x >= 0 && x < 128 && y >= 0 && y < 128)
            {
                if (!pendingPoints.Contains((x, y)))
                {
                    return grid[x, y];
                }
            }

            return false;
        }
    }
}
