using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;

namespace Day08
{
    class Program
    {
        private const string inputFile = "..\\..\\..\\..\\input8.txt";

        static void Main(string[] args)
        {
            Console.WriteLine("Day 8");
            Console.WriteLine("Star 1");
            Console.WriteLine("");

            Computer computer = new Computer();

            IEnumerable<Operation> operations = File.ReadAllLines(inputFile).Select(x => new Operation(x)).ToArray();

            foreach (Operation operation in operations)
            {
                computer.Execute(operation);
            }

            Console.WriteLine($"The largest register value is: {computer.Registers.Values.Max()}");

            Console.WriteLine("");
            Console.WriteLine("Star 2");
            Console.WriteLine("");

            Console.WriteLine($"The largest value ever held in a register is: {computer.continuousMax}");

            Console.WriteLine("");
            Console.ReadKey();
        }
    }

    enum Comparator
    {
        Equal = 0,
        NotEqual,
        GreaterThan,
        GreaterThanOrEqual,
        LessThan,
        LessThanOrEqual,
        MAX
    }

    static class ComparatorExt
    {
        public static string Print(this Comparator comparator)
        {
            switch (comparator)
            {
                case Comparator.Equal: return "==";
                case Comparator.NotEqual: return "!=";
                case Comparator.GreaterThan: return ">";
                case Comparator.GreaterThanOrEqual: return ">=";
                case Comparator.LessThan: return "<";
                case Comparator.LessThanOrEqual: return "<=";

                default:
                    throw new Exception($"Undefined Operation: {comparator}");
            }
        }
    }

    readonly struct Operation
    {
        public readonly string register;
        public readonly int value;

        public readonly string testReg;
        public readonly Comparator Comparator;
        public readonly int testValue;

        public Operation(string line)
        {
            string[] splitLine = line.Split(' ');
            register = splitLine[0];
            value = int.Parse(splitLine[2]);

            if (splitLine[1] == "dec")
            {
                value *= -1;
            }

            testReg = splitLine[4];
            testValue = int.Parse(splitLine[6]);

            switch (splitLine[5])
            {
                case "==":
                    Comparator = Comparator.Equal;
                    break;

                case "!=":
                    Comparator = Comparator.NotEqual;
                    break;

                case ">":
                    Comparator = Comparator.GreaterThan;
                    break;

                case ">=":
                    Comparator = Comparator.GreaterThanOrEqual;
                    break;

                case "<":
                    Comparator = Comparator.LessThan;
                    break;

                case "<=":
                    Comparator = Comparator.LessThanOrEqual;
                    break;

                default:
                    throw new Exception($"Undefined Operation: {splitLine[5]}");
            }
        }

        public bool TestCondition(Computer computer)
        {
            switch (Comparator)
            {
                case Comparator.Equal: return computer[testReg] == testValue;
                case Comparator.NotEqual: return computer[testReg] != testValue;
                case Comparator.GreaterThan: return computer[testReg] > testValue;
                case Comparator.GreaterThanOrEqual: return computer[testReg] >= testValue;
                case Comparator.LessThan: return computer[testReg] < testValue;
                case Comparator.LessThanOrEqual: return computer[testReg] <= testValue;

                default:
                    throw new Exception($"Undefined Operation: {Comparator}");
            }
        }

        public int ApplyOperation(Computer computer)
        {
            return (computer[register] += value);
        }

        public override string ToString() => $"IF {testReg,6}{Comparator.Print(),2}{testValue,8}   {register,6} += {value}";
    }

    class Computer
    {
        public int continuousMax = 0;

        public Dictionary<string, int> Registers = new Dictionary<string, int>();

        public Computer() { }

        public int this[string register]
        {
            get
            {
                if (Registers.ContainsKey(register))
                {
                    return Registers[register];
                }
                return 0;
            }

            set
            {
                if (Registers.ContainsKey(register))
                {
                    Registers[register] = value;
                }
                else
                {
                    Registers.Add(register, value);
                }
            }
        }

        public void Execute(in Operation operation)
        {
            if (operation.TestCondition(this))
            {
                continuousMax = Math.Max(operation.ApplyOperation(this), continuousMax);
            }
        }

        public override string ToString() =>
            $"[{string.Join(", ", Registers.Select(x => $"{x.Key}={x.Value}"))}]";
    }
}
