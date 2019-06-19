using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day25
{
    enum StateValue
    {
        A = 0,
        B,
        C,
        D,
        E,
        F,
        MAX
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Day 25");
            Console.WriteLine("Star 1");
            Console.WriteLine("");

            //Sorry this solution is boring.  I couldn't be bothered to parse it when it was
            //faster to hand-code it.

            State[] states = new State[]
            {
                new State(StateValue.A, true, 1, StateValue.B, false, -1, StateValue.C),
                new State(StateValue.B, true, -1, StateValue.A, true, +1, StateValue.C),
                new State(StateValue.C, true, +1, StateValue.A, false, -1, StateValue.D),
                new State(StateValue.D, true, -1, StateValue.E, true, -1, StateValue.C),
                new State(StateValue.E, true, +1, StateValue.F, true, +1, StateValue.A),
                new State(StateValue.F, true, +1, StateValue.A, true, +1, StateValue.E)
            };

            Dictionary<StateValue, State> stateDict = new Dictionary<StateValue, State>();

            foreach (State state in states)
            {
                stateDict.Add(state.stateValue, state);
            }

            //Since this tape is just boolean, I'll just use a hashset.
            HashSet<int> tape = new HashSet<int>();


            StateValue currentState = StateValue.A;
            int currentIndex = 0;

            for (int step = 0; step < 12134527; step++)
            {
                if (tape.Contains(currentIndex))
                {
                    //Execute o
                    if (!stateDict[currentState].oWrite)
                    {
                        tape.Remove(currentIndex);
                    }
                    currentIndex += stateDict[currentState].oShift;
                    currentState = stateDict[currentState].oState;
                }
                else
                {
                    //Execute z
                    if (stateDict[currentState].zWrite)
                    {
                        tape.Add(currentIndex);
                    }
                    currentIndex += stateDict[currentState].zShift;
                    currentState = stateDict[currentState].zState;
                }
            }

            Console.WriteLine($"Diagnostic Checksum: {tape.Count}");

            Console.WriteLine("");
            Console.WriteLine("Star 2");
            Console.WriteLine("");

            Console.WriteLine("DONE!");

            Console.WriteLine("");
            Console.ReadKey();
        }
    }

    readonly struct State
    {
        public readonly StateValue stateValue;

        public readonly bool zWrite;
        public readonly int zShift;
        public readonly StateValue zState;

        public readonly bool oWrite;
        public readonly int oShift;
        public readonly StateValue oState;

        public State(
            StateValue stateValue,
            bool zWrite,
            int zShift,
            StateValue zState,
            bool oWrite,
            int oShift,
            StateValue oState)
        {
            this.stateValue = stateValue;
            this.zWrite = zWrite;
            this.zShift = zShift;
            this.zState = zState;

            this.oWrite = oWrite;
            this.oShift = oShift;
            this.oState = oState;
        }

    }
}
