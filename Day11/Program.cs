using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day11
{
    class Program
    {
        private const string inputFile = "..\\..\\..\\..\\input11.txt";

        static void Main(string[] args)
        {
            Console.WriteLine("Day 11");
            Console.WriteLine("Star 1");
            Console.WriteLine("");

            IEnumerable<Direction> directions = File.ReadAllText(inputFile).Split(',').Select(ParseDirection).ToArray();

            //N:  (+0,+1)
            //NE: (+1,+1)
            //SE: (+1,+0)
            //S:  (+0,-1)
            //SW: (-1,-1)
            //NW: (-1,+0)

            int childX = 0;
            int childY = 0;

            int maxDistance = 0;

            foreach (Direction direction in directions)
            {
                switch (direction)
                {
                    case Direction.N:
                        childY++;
                        break;

                    case Direction.NE:
                        childX++;
                        childY++;
                        break;

                    case Direction.SE:
                        childX++;
                        break;

                    case Direction.S:
                        childY--;
                        break;

                    case Direction.SW:
                        childX--;
                        childY--;
                        break;

                    case Direction.NW:
                        childX--;
                        break;

                    default:
                        throw new Exception($"Unexpected Direction: {direction}");
                }

                maxDistance = Math.Max(maxDistance, Math.Abs(childX));
                maxDistance = Math.Max(maxDistance, Math.Abs(childY));
                maxDistance = Math.Max(maxDistance, Math.Abs(childX - childY));
            }

            //Distance formula is:  MAX(|dx|, |dy|, |dx-dy|)

            int distance = Math.Max(Math.Abs(childX), Math.Abs(childY));
            distance = Math.Max(distance, Math.Abs(childX - childY));

            Console.WriteLine($"Steps to child: {distance}");


            Console.WriteLine("");
            Console.WriteLine("Star 2");
            Console.WriteLine("");


            Console.WriteLine($"child Max distance: {maxDistance}");

            Console.WriteLine("");
            Console.ReadKey();
        }

        public enum Direction
        {
            N = 0,
            NE,
            SE,
            S,
            SW,
            NW
        }

        public static Direction ParseDirection(string direction)
        {
            switch (direction)
            {
                case "n": return Direction.N;
                case "ne": return Direction.NE;
                case "se": return Direction.SE;
                case "s": return Direction.S;
                case "sw": return Direction.SW;
                case "nw": return Direction.NW;
                default:
                    throw new Exception($"Unexpected direction: {direction}");
            }
        }

    }
}
