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

        public bool hasHalted = false;
        public Dictionary<int, int> memory;
        public Queue<int> output;
        public Queue<int> input;
        public bool waitingForInput = false;
        
        private int ip;

        public Computer(Dictionary<int, int> memory)
        {
            //clone dict
            this.memory = new Dictionary<int, int>(memory.Count);
            foreach (KeyValuePair<int, int> kvp in memory)
                this.memory.Add(kvp.Key, kvp.Value);
            this.ip = 0;
            this.input = new Queue<int>();
            this.output = new Queue<int>();

            opcodes = new();

            //ARITHMETIC
            opcodes.Add(1, ("ADD", 3, (mem, args, modes) => mem[getValue(args[2], modes[2], mem, true)] =
                getValue(args[0], modes[0], mem) +
                getValue(args[1], modes[1], mem)));
            opcodes.Add(2, ("MUL", 3, (mem, args, modes) => mem[getValue(args[2], modes[2], mem, true)] =
                getValue(args[0], modes[0], mem) *
                getValue(args[1], modes[1], mem)));
            opcodes.Add(7, ("LT", 3, (mem, args, modes) => mem[getValue(args[2], modes[2], mem, true)] =
                getValue(args[0], modes[0], mem) < getValue(args[1], modes[1], mem) ? 1 : 0));
            opcodes.Add(8, ("EQ", 3, (mem, args, modes) => mem[getValue(args[2], modes[2], mem, true)] =
                getValue(args[0], modes[0], mem) == getValue(args[1], modes[1], mem) ? 1 : 0));

            //I/O
            opcodes.Add(3, ("IN", 1, (mem, args, modes) =>
                mem[getValue(args[0], modes[0], mem, true)] = input.Dequeue()));
            opcodes.Add(4, ("OUT", 1, (mem, args, modes) =>
                output.Enqueue(getValue(args[0], modes[0], mem))));

            //JUMPS
            opcodes.Add(5, ("JT", 2, (mem, args, modes) =>
            {
                if (getValue(args[0], modes[0], mem) != 0)
                    ip = getValue(args[1], modes[1], mem);
            }));
            opcodes.Add(6, ("JF", 2, (mem, args, modes) =>
            {
                if (getValue(args[0], modes[0], mem) == 0)
                    ip = getValue(args[1], modes[1], mem);
            }));

            //HALT
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
            if (this.hasHalted)
                return;
            
            this.waitingForInput = false;
            int opcode = this.GetMemoryAt(ip++);
            (string desc, int argNum, Action<Dictionary<int, int>, int[], int[]> implementation) =
                opcodes[opcode % 100];

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
            catch (InvalidOperationException e)
            {
                //input queue is empty
                this.waitingForInput = true;
                ip -= argNum + 1;
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