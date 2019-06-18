using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day22
{
    class Program
    {
        private const string inputFile = "..\\..\\..\\..\\input22.txt";
        private static Dictionary<Vector2, bool> grid;
        private static Dictionary<Vector2, int> grid2;
        static void Main(string[] args)
        {
            Console.WriteLine("Day 22");
            Console.WriteLine("Star 1");
            Console.WriteLine("");

            grid = new Dictionary<Vector2, bool>();
            grid2 = new Dictionary<Vector2, int>();

            string[] lines = File.ReadAllLines(inputFile);

            int offset = lines.Length / -2;

            for (int y = 0; y < lines.Length; y++)
            {
                for (int x = 0; x < lines[y].Length; x++)
                {
                    grid.Add(new Vector2(x + offset, y + offset), lines[y][x] == '#');
                    grid2.Add(new Vector2(x + offset, y + offset), lines[y][x] == '#' ? 2 : 0);
                }
            }

            Vector2 position = new Vector2(0, 0);
            Vector2 facing = new Vector2(0, -1);

            int infectionSteps = 0;

            for (int step = 0; step < 10000; step++)
            {
                if (GetPosition(position))
                {
                    facing = facing.Right();
                }
                else
                {
                    infectionSteps++;
                    facing = facing.Left();
                }

                grid[position] = !grid[position];

                position += facing;

                //Dump(position);
            }

            Console.WriteLine($"Infection Steps: {infectionSteps}");

            Console.WriteLine("");
            Console.WriteLine("Star 2");
            Console.WriteLine("");

            position = new Vector2(0, 0);
            facing = new Vector2(0, -1);

            infectionSteps = 0;

            for (int step = 0; step < 10_000_000; step++)
            {
                switch (GetPosition2(position))
                {
                    case 0:
                        facing = facing.Left();
                        break;

                    case 1:
                        infectionSteps++;
                        break;

                    case 2:
                        facing = facing.Right();
                        break;

                    case 3:
                        facing = new Vector2(-facing.x, -facing.y);
                        break;
                }


                grid2[position] = (grid2[position] + 1 ) % 4;

                position += facing;
            }

            Console.WriteLine($"Infection Steps: {infectionSteps}");

            Console.WriteLine("");
            Console.ReadKey();
        }

        private static void Dump(in Vector2 position)
        {
            Console.WriteLine();
            Console.WriteLine();
            string line = "";
            for (int y = -10; y < 10; y++)
            {
                for (int x = -10; x < 10; x++)
                {
                    if (GetPosition(new Vector2(x, y)))
                    {
                        line += "#";
                    }
                    else
                    {
                        line += ".";
                    }

                    if (position == new Vector2(x, y))
                    {
                        line += "]";
                    }
                    else if (position == new Vector2(x+1,y))
                    {
                        line += "[";
                    }
                    else
                    {
                        line += " ";
                    }
                }
                line += "\n";
            }
            Console.WriteLine(line);
            Console.WriteLine();
            Console.WriteLine();
        }

        private static bool GetPosition(in Vector2 position)
        {
            if (!grid.ContainsKey(position))
            {
                grid.Add(position, false);
            }

            return grid[position];
        }

        private static int GetPosition2(in Vector2 position)
        {
            if (!grid2.ContainsKey(position))
            {
                grid2.Add(position, 0);
            }

            return grid2[position];
        }
    }

    public readonly struct Vector2
    {
        public readonly long x;
        public readonly long y;

        public Vector2(long x, long y)
        {
            this.x = x;
            this.y = y;
        }

        public static readonly Vector2 Zero = new Vector2(0, 0);
        public static readonly Vector2 XAxis = new Vector2(1, 0);
        public static readonly Vector2 YAxis = new Vector2(0, 1);

        public override string ToString() => $"<{x},{y}>";

        public override bool Equals(object obj) => (obj is Vector2 vector) && Equals(vector);

        public bool Equals(in Vector2 value) =>
            x == value.x && y == value.y;
        public override int GetHashCode() => HashCode.Combine(x, y);

        public static Vector2 operator +(in Vector2 left, in Vector2 right) =>
            new Vector2(left.x + right.x, left.y + right.y);

        public static Vector2 operator -(in Vector2 left, in Vector2 right) =>
            new Vector2(left.x - right.x, left.y - right.y);

        public static Vector2 operator *(in Vector2 left, in Vector2 right) =>
            new Vector2(left.x * right.x, left.y * right.y);

        public static Vector2 operator *(in Vector2 left, long right) =>
            new Vector2(left.x * right, left.y * right);

        public static Vector2 operator *(long left, in Vector2 right) =>
            new Vector2(left * right.x, left * right.y);

        //0,-1  -> 1,0 R


        public Vector2 Right() => new Vector2(-y, x);
        public Vector2 Left() => new Vector2(y, -x);

        public static bool operator ==(in Vector2 left, in Vector2 right) =>
            left.Equals(right);

        public static bool operator !=(in Vector2 left, in Vector2 right) =>
            !left.Equals(right);
    }
}
