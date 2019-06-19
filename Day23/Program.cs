using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day23
{
    class Program
    {
        private const string inputFile = "..\\..\\..\\..\\input23.txt";

        static void Main(string[] args)
        {
            Console.WriteLine("Day 23");
            Console.WriteLine("Star 1");
            Console.WriteLine("");

            //Instructions:

            //  set x y - Sets register X to the value of Y
            //  sub x y - x = x - y
            //  mul x y - x = x * y
            //  jnz x y - jumps with an offset of y, but only if x is not zero

            //8 registers, a through h


            //How Many Times is Mul invoked?

            Instruction[] instructions = File.ReadAllLines(inputFile)
                .Select(x => new Instruction(x))
                .ToArray();

            Machine machine = new Machine(instructions);
            MachineState machineState;

            while ((machineState = machine.Run()) == MachineState.Running)
            {
                //Gogogo
            }

            Console.WriteLine($"Mul invoke count: {machine.mulCount}");

            Console.WriteLine("");
            Console.WriteLine("Star 2");
            Console.WriteLine("");

            //The algorithm seems to count non-primes between b and c incrementing by 17
            //With a = 1, b is 107,900 and c is 124,900

            //Refresh The Machine
            machine = new Machine(instructions);
            //Set the value
            machine.registers['a'] = 1;

            //Take 8 steps to init values
            for (int i = 0; i < 8; i++)
            {
                machine.Step();
            }

            IEnumerable<int> primes = PrimesUpTo((int)machine.registers['c']);

            HashSet<int> relevantPrimes = new HashSet<int>(primes.Where(x => x >= (int)machine.registers['b']));

            int count = 0;
            for (int i = (int)machine.registers['b']; i <= (int)machine.registers['c']; i += 17)
            {
                if (!relevantPrimes.Contains(i))
                {
                    count++;
                }
            }

            Console.WriteLine($"Value of h register: {count}");

            Console.WriteLine("");
            Console.ReadKey();
        }

        //Taken from code I've written for the BrainGameCenter:
        //  https://github.com/UCRBrainGameCenter/BGC_Tools
        private static IEnumerable<int> PrimesUpTo(int number)
        {
            if (number < 2)
            {
                yield break;
            }

            //Include the boundary
            number++;

            BitArray primeField = new BitArray(number, true);
            primeField.Set(0, false);
            primeField.Set(1, false);
            yield return 2;

            int i;
            //Clear Others
            for (i = 3; i * i < number; i += 2)
            {
                if (primeField.Get(i))
                {
                    //i Is Prime
                    yield return i;

                    //Clear new odd factors
                    //All our primes are now odd, as are our primes Squared.
                    //This maens the numbers we need to clear start at i*i, and advance by 2*i
                    //For example j=3:  9 is the first odd composite, 15 is the next odd composite 
                    //  that's a factor of 3
                    for (int j = i * i; j < number; j += 2 * i)
                    {
                        primeField.Set(j, false);
                    }
                }
            }

            for (; i < number; i += 2)
            {
                if (primeField.Get(i))
                {
                    //i Is Prime
                    yield return i;
                }
            }
        }
    }

    public enum Operation
    {
        SET = 0,
        SUB,
        MUL,
        JNZ,
        MAX
    }

    public enum MachineState
    {
        Finished = 0,
        Running,
        MAX
    }

    public class Machine
    {
        public readonly Instruction[] instructions;
        public readonly Dictionary<char, long> registers = new Dictionary<char, long>();

        public int instrNum = 0;
        public int mulCount = 0;

        public Machine(Instruction[] instructions)
        {
            this.instructions = instructions;

            for (char x = 'a'; x <= 'h'; x++)
            {
                registers.Add(x, 0);
            }
        }

        public MachineState Run()
        {
            MachineState state = MachineState.Running;

            while (state == MachineState.Running)
            {
                if (instrNum < 0 || instrNum >= instructions.Length)
                {
                    return MachineState.Finished;
                }

                state = instructions[instrNum].Execute(ref instrNum, ref mulCount, registers);
            }

            return state;
        }

        public MachineState Step()
        {
            if (instrNum < 0 || instrNum >= instructions.Length)
            {
                return MachineState.Finished;
            }

            return instructions[instrNum].Execute(ref instrNum, ref mulCount, registers);
        }
    }

    public readonly struct Instruction
    {
        public readonly Operation Operation;

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

        public Instruction(string line)
        {
            string[] splitLine = line.Split(" ");


            switch (splitLine[0])
            {
                case "set":
                    Operation = Operation.SET;
                    break;

                case "sub":
                    Operation = Operation.SUB;
                    break;

                case "mul":
                    Operation = Operation.MUL;
                    break;

                case "jnz":
                    Operation = Operation.JNZ;
                    break;

                default:
                    throw new Exception($"Unrecognized instr: {splitLine[0]}");
            }


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

        public MachineState Execute(ref int instrNum, ref int mulCount, Dictionary<char, long> registers)
        {
            switch (Operation)
            {
                case Operation.SET:
                    registers[Arg1Reg] = GetArg2Value(registers);
                    break;

                case Operation.SUB:
                    registers[Arg1Reg] = GetArg1Value(registers) - GetArg2Value(registers);
                    break;

                case Operation.MUL:
                    mulCount++;
                    registers[Arg1Reg] = GetArg1Value(registers) * GetArg2Value(registers);
                    break;

                case Operation.JNZ:
                    if (GetArg1Value(registers) != 0)
                    {
                        instrNum = (int)Math.Clamp(instrNum + GetArg2Value(registers), int.MinValue, int.MaxValue);
                        return MachineState.Running;
                    }
                    break;

                default:
                    throw new ArgumentException($"Unsupported instr: {Operation}");
            }

            instrNum++;
            return MachineState.Running;
        }

        private long GetArg1Value(Dictionary<char, long> registers) =>
            register1Arg ? registers[Arg1Reg] : Arg1Int;

        private long GetArg2Value(Dictionary<char, long> registers) =>
            register2Arg ? registers[Arg2Reg] : Arg2Int;

        private string OpString()
        {
            switch (Operation)
            {
                case Operation.SET: return "SET";
                case Operation.SUB: return "SUB";
                case Operation.MUL: return "MUL";
                case Operation.JNZ: return "JNZ";

                default: throw new Exception($"Unexpected Op: {Operation}");
            }
        }

        public override string ToString() =>
            $"{OpString()} {(register1Arg ? Arg1Reg.ToString() : Arg1Int.ToString())} {(register2Arg ? Arg2Reg.ToString() : Arg2Int.ToString())}";
    }

}
