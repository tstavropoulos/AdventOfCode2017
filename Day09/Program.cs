using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day09
{
    class Program
    {
        private const string inputFile = "..\\..\\..\\..\\input9.txt";

        static void Main(string[] args)
        {
            Console.WriteLine("Day 9");
            Console.WriteLine("Star 1");
            Console.WriteLine("");

            Group baseGroup = null;

            using (StreamReader reader = new StreamReader(inputFile))
            {
                char temp = (char)reader.Read();
                if (temp != '{')
                {
                    throw new Exception($"Unexpected opening character: {temp}");
                }

                baseGroup = new Group(reader, null);
            }


            Console.WriteLine($"Total Score: {baseGroup.GetScore()}");

            Console.WriteLine("");
            Console.WriteLine("Star 2");
            Console.WriteLine("");

            Console.WriteLine($"Total Garbage: {baseGroup.GetTotalGarbage()}");

            Console.WriteLine("");
            Console.ReadKey();
        }


        class Group
        {
            public readonly List<Group> children = new List<Group>();
            public readonly Group parent;
            public readonly int tier;
            public int garbageCharacters = 0;

            public Group(StreamReader reader, Group parent)
            {
                this.parent = parent;
                tier = 1 + (parent?.tier ?? 0);

                while (true)
                {
                    switch ((char)reader.Read())
                    {
                        case '<':
                            SkipGarbage(reader);
                            break;

                        case '{':
                            children.Add(new Group(reader, this));
                            break;

                        case '}':
                            return;

                        default:
                            break;
                    }
                }
            }

            private void SkipGarbage(StreamReader reader)
            {
                while (true)
                {
                    switch ((char)reader.Read())
                    {
                        case '!':
                            //Skip next character
                            reader.Read();
                            break;

                        case '>':
                            return;

                        default:
                            garbageCharacters++;
                            break;
                    }
                }
            }

            public int GetScore() =>
                tier + children.Select(x => x.GetScore()).Sum();

            public int GetTotalGarbage() =>
                garbageCharacters + children.Select(x => x.GetTotalGarbage()).Sum();

        }
    }
}
