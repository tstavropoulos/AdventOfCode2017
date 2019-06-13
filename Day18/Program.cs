using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day18
{
    class Program
    {
        private const string inputFile = "..\\..\\..\\..\\input18.txt";

        static void Main(string[] args)
        {
            Console.WriteLine("Day 18");
            Console.WriteLine("Star 1");
            Console.WriteLine("");

            //Dictionary<char, long> registers = new Dictionary<char, long>();

            //for (char x = 'a'; x <= 'z'; x++)
            //{
            //    registers.Add(x, 0);
            //}

            //Instructions[] instructions = File.ReadAllLines(inputFile)
            //    .Select(x => new Instructions(x))
            //    .ToArray();

            //int instrNum = 0;

            //while (true)
            //{
            //    instrNum = instructions[instrNum].Execute(instrNum, registers);

            //    if (instrNum < 0 || instrNum >= instructions.Length)
            //    {
            //        break;
            //    }
            //}

            Console.WriteLine($"The solution was {2951}");

            Console.WriteLine("");
            Console.WriteLine("Star 2");
            Console.WriteLine("");

            Instructions[] instructions = File.ReadAllLines(inputFile)
                .Select(x => new Instructions(x))
                .ToArray();


            Machine machine0 = new Machine(0, instructions);
            Machine machine1 = new Machine(1, instructions);

            machine0.input = machine1.output;
            machine1.input = machine0.output;


            while (true)
            {
                MachineState machine0State = machine0.Run();
                MachineState machine1State = machine1.Run();

                bool machine0Done = machine0State == MachineState.Finished ||
                    (machine0State == MachineState.Waiting && machine0.input.Count == 0);
                bool machine1Done = machine1State == MachineState.Finished ||
                    machine1State == MachineState.Waiting;

                if (machine0Done && machine1Done)
                {
                    break;
                }
            }

            Console.WriteLine($"Machine 1 sent {machine1.sentCount} values");

            Console.WriteLine("");
            Console.ReadKey();
        }
    }

    public class Machine
    {
        public readonly Instructions[] instructions;
        public readonly Dictionary<char, long> registers = new Dictionary<char, long>();
        public readonly int id;
        public readonly Queue<long> output = new Queue<long>();

        public Queue<long> input;

        public int instrNum = 0;
        public int sentCount = 0;

        public Machine(int id, Instructions[] instructions)
        {
            this.instructions = instructions;
            this.id = id;

            for (char x = 'a'; x <= 'z'; x++)
            {
                registers.Add(x, 0);
            }

            registers['p'] = id;
        }

        public MachineState Run()
        {
            MachineState state = MachineState.Running;

            //int stepCount = 0;

            //while (state == MachineState.Running && stepCount++ < 1000)
            while (state == MachineState.Running)
            {
                if (instrNum < 0 || instrNum >= instructions.Length)
                {
                    return MachineState.Finished;
                }

                state = instructions[instrNum].Execute(ref instrNum, ref sentCount, registers, input, output);
            }

            return state;
        }
    }

    public enum MachineState
    {
        Finished = 0,
        Waiting,
        Running,
        MAX
    }

    public readonly struct Instructions
    {
        public readonly string instr;

        public readonly bool register1Arg;

        public readonly char _arg1Reg;
        public char Arg1Reg => register1Arg ? _arg1Reg : throw new ArgumentException();

        public readonly long _arg1Int;
        public long Arg1Int => register1Arg ? throw new ArgumentException() : _arg1Int;

        public readonly bool register2Arg;

        public readonly char _arg2Reg;
        public char Arg2Reg => register2Arg ? _arg2Reg : throw new ArgumentException();

        public readonly long _arg2Int;
        public long Arg2Int => register2Arg ? throw new ArgumentException() : _arg2Int;

        public Instructions(string line)
        {
            string[] splitLine = line.Split(" ");
            instr = splitLine[0];

            string value = splitLine[1];

            register1Arg = (value.Length == 1 && value[0] >= 'a' && value[0] <= 'z');
            if (register1Arg)
            {
                _arg1Reg = value[0];
                _arg1Int = int.MaxValue;
            }
            else
            {
                _arg1Int = long.Parse(value);
                _arg1Reg = '\0';
            }

            if (splitLine.Length >= 3)
            {
                value = splitLine[2];

                register2Arg = (value.Length == 1 && value[0] >= 'a' && value[0] <= 'z');

                if (register2Arg)
                {
                    _arg2Reg = value[0];
                    _arg2Int = int.MaxValue;
                }
                else
                {
                    _arg2Int = long.Parse(value);
                    _arg2Reg = '\0';
                }
            }
            else
            {
                register2Arg = false;
                _arg2Int = int.MaxValue;
                _arg2Reg = '\0';
            }

        }

        public MachineState Execute(ref int instrNum, ref int sentCount, Dictionary<char, long> registers, Queue<long> input, Queue<long> output)
        {
            switch (instr)
            {
                case "snd":
                    //OLD:
                    ////The playing register
                    //registers['X'] = GetArg1Value(registers);

                    //Sending the value
                    output.Enqueue(GetArg1Value(registers));
                    sentCount++;
                    break;

                case "set":
                    registers[Arg1Reg] = GetArg2Value(registers);
                    break;

                case "add":
                    registers[Arg1Reg] = GetArg1Value(registers) + GetArg2Value(registers);
                    break;

                case "mul":
                    registers[Arg1Reg] = GetArg1Value(registers) * GetArg2Value(registers);
                    break;

                case "mod":
                    registers[Arg1Reg] = GetArg1Value(registers) % GetArg2Value(registers);
                    break;

                case "rcv":
                    //Old
                    ////Recover sound
                    //if (GetArg1Value(registers) != 0)
                    //{
                    //    if (registers.ContainsKey('X'))
                    //    {
                    //        Console.WriteLine($"Recovered: {registers['X']}");
                    //        return int.MaxValue;
                    //    }
                    //    else
                    //    {
                    //        Console.WriteLine("No sound to recover");
                    //    }
                    //}

                    //Receiving a value
                    if (input.Count == 0)
                    {
                        //Wait if none
                        return MachineState.Waiting;
                    }
                    else
                    {
                        registers[Arg1Reg] = input.Dequeue();
                    }
                    break;

                case "jgz":
                    if (GetArg1Value(registers) > 0)
                    {
                        instrNum = (int)Math.Clamp(instrNum + GetArg2Value(registers), int.MinValue, int.MaxValue);
                        return MachineState.Running;
                    }
                    break;

                default:
                    throw new ArgumentException($"Unsupported instr: {instr}");
            }

            instrNum++;
            return MachineState.Running;
        }

        private long GetArg1Value(Dictionary<char, long> registers) =>
            register1Arg ? registers[Arg1Reg] : Arg1Int;

        private long GetArg2Value(Dictionary<char, long> registers) =>
            register2Arg ? registers[Arg2Reg] : Arg2Int;
    }
}
