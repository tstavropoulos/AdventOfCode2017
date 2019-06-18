using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day21
{
    class Program
    {
        private const string inputFile = "..\\..\\..\\..\\input21.txt";
        private static Dictionary<string, string> imageTranslator;

        static void Main(string[] args)
        {
            Console.WriteLine("Day 21");
            Console.WriteLine("Star 1");
            Console.WriteLine("");

            //Initial pattern is always:

            //  .#.
            //  ..#
            //  ###

            //Note: While Patterns will match any rotation or flip, but don't rotate/flip the output


            imageTranslator = new Dictionary<string, string>();

            foreach ((string baseInput, string output) in File.ReadAllLines(inputFile).Select(SplitInput))
            {
                foreach (string input in baseInput.GetAllRotations())
                {
                    if (!imageTranslator.ContainsKey(input))
                    {
                        imageTranslator.Add(input, output);
                    }
                }
            }

            string[] image = new string[] { ".#.", "..#", "###" };

            for (int i = 0; i < 5; i++)
            {
                image = Enhance(image);
            }

            Console.WriteLine($"Pixels on after 5 iterations: {string.Concat(image).Count(x => x == '#')}");

            Console.WriteLine("");
            Console.WriteLine("Star 2");
            Console.WriteLine("");


            for (int i = 5; i < 18; i++)
            {
                image = Enhance(image);
            }

            Console.WriteLine($"Pixels on after 18 iterations: {string.Concat(image).Count(x => x == '#')}");

            Console.WriteLine("");
            Console.ReadKey();

        }

        private static string[] Enhance(string[] image)
        {
            int size = image.Length;
            string[] output;
            int stepCount;
            int stepSize;

            if (size % 2 == 0)
            {
                //2x2 -> 3x3
                output = new string[3 * size / 2];
                stepCount = size / 2;
                stepSize = 2;

            }
            else //size % 3 == 0
            {
                //3x3 -> 4x4
                output = new string[4 * size / 3];
                stepCount = size / 3;
                stepSize = 3;
            }

            for (int i = 0; i < output.Length; i++)
            {
                output[i] = "";
            }


            int y1 = 0;
            for (int y0 = 0; y0 < size; y0 += stepSize, y1 += stepSize + 1)
            {
                IEnumerable<string> rows = image.Skip(y0).Take(stepSize);

                for (int x0 = 0; x0 < size; x0 += stepSize)
                {
                    IEnumerable<string> segment = rows.Select(x => x.Substring(x0, stepSize));

                    string pattern = string.Join('/', segment);

                    string translatedString = imageTranslator[pattern];
                    string[] splitTranslated = translatedString.Split('/');

                    for (int i = 0; i < stepSize + 1; i++)
                    {
                        output[i + y1] += splitTranslated[i];
                    }
                }
            }

            return output;
        }

        private static (string input, string output) SplitInput(string input)
        {
            string[] splitLine = input.Split(" => ");

            return (splitLine[0], splitLine[1]);
        }
    }

    public static class Day21Extensions
    {
        public static IEnumerable<string> GetAllRotations(this string input)
        {
            IEnumerable<string> splitInput = input.Split('/');

            yield return input;
            yield return string.Join('/', splitInput.Rotate());
            yield return string.Join('/', splitInput.Rotate().Rotate());
            yield return string.Join('/', splitInput.Rotate().Rotate().Rotate());

            IEnumerable<string> flipped = splitInput.Mirror().ToArray();
            yield return string.Join('/', flipped);
            yield return string.Join('/', flipped.Rotate());
            yield return string.Join('/', flipped.Rotate().Rotate());
            yield return string.Join('/', flipped.Rotate().Rotate().Rotate());
        }

        public static IEnumerable<string> Rotate(this IEnumerable<string> input)
        {
            int count = input.First().Length;

            for (int i = 0; i < count; i++)
            {
                yield return string.Concat(input.Reverse().Select(x => x[i]));
            }
        }

        public static IEnumerable<string> Mirror(this IEnumerable<string> input) => input.Reverse();
    }
}
