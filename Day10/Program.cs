using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;

namespace Day10
{
    class Program
    {
        private const string inputFile = "..\\..\\..\\..\\input10.txt";

        static void Main(string[] args)
        {
            Console.WriteLine("Day 10");
            Console.WriteLine("Star 1");
            Console.WriteLine("");


            IEnumerable<int> inputLengths = File.ReadAllText(inputFile).Split(',').Select(int.Parse).ToArray();

            CircularList list = new CircularList(256);

            foreach (int length in inputLengths)
            {
                list.Twist(length);
            }

            Console.WriteLine($"Product of first two positions: {list[0] * list[1]}");

            Console.WriteLine("");
            Console.WriteLine("Star 2");
            Console.WriteLine("");

            //Load data as ascii
            inputLengths = Encoding.UTF8.GetBytes(File.ReadAllText(inputFile))
                    .Select(x => (int)x)
                    .Concat(new int[] { 17, 31, 73, 47, 23 })
                .ToArray();


            //refresh list
            list = new CircularList(256);

            //Run 64 rounds of hashing
            for (int i = 0; i < 64; i++)
            {
                foreach (int length in inputLengths)
                {
                    list.Twist(length);
                }
            }

            Console.WriteLine($"Knot hash is: {list.GetDenseHash()}");

            Console.WriteLine("");
            Console.ReadKey();
        }
    }

    public class CircularList
    {
        private readonly int length;
        public readonly int[] data;

        public int currentIndex = 0;
        public int skipCount = 0;

        public CircularList(int length)
        {
            this.length = length;

            data = new int[length];
            for (int i = 0; i < length; i++)
            {
                data[i] = i;
            }
        }

        public int this[int i]
        {
            get => data[i % length];
            set => data[i % length] = value;
        }

        public void Twist(int loopSize)
        {
            if (loopSize >= length)
            {
                throw new ArgumentException($"Unexpected loopSize: {loopSize}");
            }

            int startIndex = currentIndex;
            int endIndex = currentIndex + loopSize - 1;
            int steps = loopSize / 2;

            for (int offset = 0; offset < steps; offset++)
            {
                int temp = this[startIndex + offset];
                this[startIndex + offset] = this[endIndex - offset];
                this[endIndex - offset] = temp;
            }

            currentIndex += loopSize + skipCount++;

            skipCount %= length;
            currentIndex %= length;
        }

        public string GetDenseHash()
        {
            int denseLength = length / 16;
            int[] hash = new int[denseLength];

            for (int i = 0; i < denseLength; i++)
            {
                for (int j = 0; j < 16; j++)
                {
                    hash[i] ^= data[16 * i + j];
                }
            }

            return string.Concat(hash.Select(x => x.ToString("X2")));
        }

        public byte[] GetDenseHashBytes()
        {
            int denseLength = length / 16;
            byte[] hash = new byte[denseLength];

            for (int i = 0; i < denseLength; i++)
            {
                for (int j = 0; j < 16; j++)
                {
                    hash[i] ^= (byte)data[16 * i + j];
                }
            }

            return hash;
        }
    }
}
