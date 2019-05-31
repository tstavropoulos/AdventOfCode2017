using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day07
{
    class Program
    {
        private const string inputFile = "..\\..\\..\\..\\input7.txt";

        static void Main(string[] args)
        {
            Console.WriteLine("Day 7");
            Console.WriteLine("Star 1");
            Console.WriteLine("");

            IEnumerable<ProgramNode> allPrograms = File.ReadAllLines(inputFile).Select(x => new ProgramNode(x)).ToArray();

            Dictionary<string, ProgramNode> programDictionary = new Dictionary<string, ProgramNode>();

            foreach (ProgramNode node in allPrograms)
            {
                programDictionary.Add(node.name, node);
            }

            foreach (ProgramNode node in allPrograms)
            {
                node.PopulateChildren(programDictionary);
            }

            foreach (ProgramNode node in allPrograms.Where(x => x.parent == null))
            {
                Console.WriteLine($"Root Node: {node.name}");
            }

            Console.WriteLine("");
            Console.WriteLine("Star 2");
            Console.WriteLine("");

            foreach (ProgramNode node in allPrograms.Where(x => !x.Balanced))
            {
                Console.WriteLine($"Unbalanced Node: {node}");
            }

            Console.WriteLine("");

            //The highest unbalanced node will be the one with the lighest deepweight

            ProgramNode blamedNode = allPrograms.Where(x => !x.Balanced).OrderBy(x => x.DeepWeight).First();
            Console.WriteLine($"Deepest Unbalanced Node: {blamedNode.ToBlameString()}");

            //Manual input value to prevent silly coding
            string badNode = "drjmjug";

            Console.WriteLine($"The bad node: {programDictionary[badNode]}");
            Console.WriteLine($"New bad node weight: {programDictionary[badNode].weight - 8}");

            Console.WriteLine("");
            Console.ReadKey();
        }
    }

    class ProgramNode
    {
        public readonly string name;
        public readonly int weight;
        public readonly List<ProgramNode> children = new List<ProgramNode>();
        public ProgramNode parent = null;
        private readonly string[] childNames;

        public int DeepWeight => weight + children.Select(x => x.DeepWeight).Sum();
        public bool Balanced => children.Select(x => x.DeepWeight).Distinct().Count() <= 1;

        public static readonly char[] separators = new char[]
        {
            ' ',
            '-',
            '>',
            '(',
            ')',
            ','
        };

        public ProgramNode(string line)
        {
            IEnumerable<string> splitLine = line.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            name = splitLine.First();
            weight = int.Parse(splitLine.Skip(1).First());
            childNames = splitLine.Skip(2).ToArray();
        }

        public void PopulateChildren(Dictionary<string, ProgramNode> programDictionary)
        {
            foreach (string child in childNames)
            {
                children.Add(programDictionary[child]);
                programDictionary[child].parent = this;
            }
        }

        public override string ToString() =>
            $"{name} ({weight}){(children.Count > 0 ? " -> " : "")}{string.Join(", ", children.Select(x => x.name))}";

        public string ToBlameString() =>
            string.Join(", ", children.Select(x => $"{x.name} ({x.DeepWeight})"));
    }
}
