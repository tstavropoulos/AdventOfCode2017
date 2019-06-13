using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;

namespace Day19
{
    class Program
    {
        private const string inputFile = "..\\..\\..\\..\\input19.txt";

        static void Main(string[] args)
        {
            Console.WriteLine("Day 19");
            Console.WriteLine("Star 1");
            Console.WriteLine("");

            string[] lines = File.ReadAllLines(inputFile);
            int W = lines[0].Length;
            int H = lines.Length;

            //Find the entryPoint
            int x = lines[0].IndexOf("|");
            int y = -1;

            char[,] grid = new char[W, H];

            for (int i = 0; i < lines.Length; i++)
            {
                for (int j = 0; j < lines[i].Length; j++)
                {
                    grid[j, i] = lines[i][j];
                }
            }


            MoveState state = MoveState.Down;

            string collected = "";
            int steps = 0;
            bool running = true;

            while (running)
            {
                //Move
                switch (state)
                {
                    case MoveState.Down:
                        y++;
                        break;

                    case MoveState.Up:
                        y--;
                        break;

                    case MoveState.Left:
                        x--;
                        break;

                    case MoveState.Right:
                        x++;
                        break;

                    default: throw new Exception("No Direction");
                }

                steps++;

                if (x < 0 || x >= W || y < 0 || y >= H)
                {
                    //We're done
                    break;
                }

                switch (grid[x, y])
                {
                    case ' ':
                        //Fell off tracks
                        steps--;
                        running = false;
                        break;

                    case '+':
                        //Figure out what to do
                        if (state != MoveState.Down &&
                            y - 1 >= 0 &&
                            grid[x, y - 1] != ' ' &&
                            grid[x, y - 1] != '-')
                        {
                            state = MoveState.Up;
                            break;
                        }

                        if (state != MoveState.Up &&
                            y + 1 < H &&
                            grid[x, y + 1] != ' ' &&
                            grid[x, y + 1] != '-')
                        {
                            state = MoveState.Down;
                            break;
                        }

                        if (state != MoveState.Right &&
                            x - 1 >= 0 &&
                            grid[x - 1, y] != ' ' &&
                            grid[x - 1, y] != '|')
                        {
                            state = MoveState.Left;
                            break;
                        }

                        if (state != MoveState.Left &&
                            x + 1 < W &&
                            grid[x + 1, y] != ' ' &&
                            grid[x + 1, y] != '|')
                        {
                            state = MoveState.Right;
                            break;
                        }

                        throw new Exception("Nope!");

                    case '|':
                    case '-':
                        //Status quo
                        break;

                    default:
                        //Got a letter
                        collected += grid[x, y];
                        if (state == MoveState.Right || state == MoveState.Left)
                        {
                            grid[x, y] = '-';
                        }
                        else
                        {
                            grid[x, y] = '|';
                        }
                        break;
                }
            }

            Console.WriteLine($"Found: {collected}");


            Console.WriteLine("");
            Console.WriteLine("Star 2");
            Console.WriteLine("");

            Console.WriteLine($"Steps: {steps}");

            Console.WriteLine("");
            Console.ReadKey();
        }
    }

    enum MoveState
    {
        Down = 0,
        Up,
        Right,
        Left,
        MAX
    }
}
