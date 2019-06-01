using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day12
{
    class Program
    {
        private const string inputFile = "..\\..\\..\\..\\input12.txt";

        static void Main(string[] args)
        {
            Console.WriteLine("Day 12");
            Console.WriteLine("Star 1");
            Console.WriteLine("");

            IEnumerable<string> lines = File.ReadAllLines(inputFile);

            Dictionary<int, PipeProgram> pipePrograms = new Dictionary<int, PipeProgram>();

            foreach (string line in lines)
            {
                PipeProgram newProgram = new PipeProgram(line);

                pipePrograms.Add(newProgram.id, newProgram);
            }

            HashSet<int> allPrograms = new HashSet<int>(pipePrograms.Keys);
            HashSet<int> connectedPrograms = new HashSet<int>() { 0 };

            Queue<int> programsToProcess = new Queue<int>();
            programsToProcess.Enqueue(0);
            allPrograms.Remove(0);


            while (programsToProcess.Count > 0)
            {
                PipeProgram newProgram = pipePrograms[programsToProcess.Dequeue()];

                foreach (int newConnection in newProgram.connections)
                {
                    if (!connectedPrograms.Contains(newConnection))
                    {
                        programsToProcess.Enqueue(newConnection);
                        connectedPrograms.Add(newConnection);
                        allPrograms.Remove(newConnection);
                    }
                }
            }

            Console.WriteLine($"Programs connected to 0: {connectedPrograms.Count}");

            Console.WriteLine("");
            Console.WriteLine("Star 2");
            Console.WriteLine("");


            int groups = 1;

            while (allPrograms.Count > 0)
            {
                groups++;
                connectedPrograms.Clear();

                int startingValue = allPrograms.First();
                programsToProcess.Enqueue(startingValue);
                allPrograms.Remove(startingValue);

                while (programsToProcess.Count > 0)
                {
                    PipeProgram newProgram = pipePrograms[programsToProcess.Dequeue()];

                    foreach (int newConnection in newProgram.connections)
                    {
                        if (!connectedPrograms.Contains(newConnection))
                        {
                            programsToProcess.Enqueue(newConnection);
                            connectedPrograms.Add(newConnection);
                            allPrograms.Remove(newConnection);
                        }
                    }
                }
            }

            Console.WriteLine($"There are {groups} groups");

            Console.WriteLine("");
            Console.ReadKey();
        }
    }

    public class PipeProgram
    {
        public readonly int id;
        public readonly int[] connections;

        public PipeProgram(string line)
        {
            string[] splitLine = line.Split(" <-> ");

            id = int.Parse(splitLine[0]);
            connections = splitLine[1].Split(", ").Select(int.Parse).ToArray();
        }
    }
}
