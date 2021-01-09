using System;
using System.Collections.Generic;
using System.ComponentModel;
using static AdventOfCode.Solutions.IntCode.ModeHelper;

namespace AdventOfCode.Solutions.IntCode
{
    public class Computer
    {
        private readonly Dictionary<int, (string desc, int argNum, Action<Dictionary<int, int>, int[], int[]>
            implementation)> opcodes;
        private bool hasHalted = false;
        private Dictionary<int, int> memory;
        public Queue<int> output;
        public Queue<int> input;

        private int ip;
        
        public Computer(Dictionary<int, int> memory)
        {
            this.memory = memory;
            this.ip = 0;
            this.input = new Queue<int>();
            this.output = new Queue<int>();

            opcodes = new();

            opcodes.Add(1, ("ADD", 3, (mem, args, modes) => mem[getValue(args[2], modes[2], mem, true)] =
                getValue(args[0], modes[0], mem) +
                getValue(args[1], modes[1], mem)));
            opcodes.Add(2, ("MUL", 3, (mem, args, modes) => mem[getValue(args[2], modes[2], mem, true)] =
                getValue(args[0], modes[0], mem) *
                getValue(args[1], modes[1], mem)));
            
            opcodes.Add(99, ("HLT", 0, (_, _, _) => this.hasHalted = true));
        }

        public int GetMemoryAt(int pos)
        {
            return memory.ContainsKey(pos) ? this.memory[pos] : 0;
        }

        /// <summary>
        /// Steps the program forward one instruction
        /// </summary>
        public void Step()
        {
            int opcode = this.GetMemoryAt(ip++);
            (string desc, int argNum, Action<Dictionary<int, int>, int[], int[]> implementation) = opcodes[opcode % 100];

            opcode /= 100;
            int[] modes = new int[argNum];
            for (int i = 0; i < modes.Length; i++)
            {
                modes[i] = opcode % 10;
                opcode /= 10;
            }

            int[] arguments = new int[argNum];
            for (int i = 0; i < arguments.Length; i++)
            {
                arguments[i] = this.GetMemoryAt(ip++);
            }
            
            try
            {
                implementation(this.memory, arguments, modes);
            }
            catch (Exception e)
            {
                Console.WriteLine($"IntCodeComputer failed with exception: {e.GetType().Name} at {ip}, instruction:");
                Console.WriteLine($"{desc} {string.Join(' ', arguments)}");
                Console.WriteLine(e.Message);
            }
        }

        public void RunUntilHalted()
        {
            while (!this.hasHalted)
                Step();
        }
    }
}