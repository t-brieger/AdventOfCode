using System;
using System.Collections.Generic;

namespace AdventOfCode.Solutions.IntCode
{
    public class Computer
    {
        private ModeHelper mh = new ModeHelper();
        private Dictionary<long, (string desc, int argNum, Action<long[], long[]> implementation)> opcodes;

        public bool hasHalted = false;
        public Dictionary<long, long> memory;
        public Queue<long> output;
        public Queue<long> input;
        public bool waitingForInput = false;

        private long ip;

        private void init()
        {
            this.ip = 0;
            this.input = new Queue<long>();
            this.output = new Queue<long>();

            opcodes = new();

            //ARITHMETIC
            opcodes.Add(1, ("ADD", 3, (args, modes) => this.memory[mh.getValue(args[2], modes[2], this.memory, true)] =
                mh.getValue(args[0], modes[0], this.memory) +
                mh.getValue(args[1], modes[1], this.memory)));
            opcodes.Add(2, ("MUL", 3, (args, modes) => this.memory[mh.getValue(args[2], modes[2], this.memory, true)] =
                mh.getValue(args[0], modes[0], this.memory) *
                mh.getValue(args[1], modes[1], this.memory)));
            opcodes.Add(7, ("LT", 3, (args, modes) => this.memory[mh.getValue(args[2], modes[2], this.memory, true)] =
                mh.getValue(args[0], modes[0], this.memory) < mh.getValue(args[1], modes[1], this.memory) ? 1 : 0));
            opcodes.Add(8, ("EQ", 3, (args, modes) => this.memory[mh.getValue(args[2], modes[2], this.memory, true)] =
                mh.getValue(args[0], modes[0], this.memory) == mh.getValue(args[1], modes[1], this.memory) ? 1 : 0));

            //I/O
            opcodes.Add(3, ("IN", 1, (args, modes) =>
                this.memory[mh.getValue(args[0], modes[0], this.memory, true)] = input.Dequeue()));
            opcodes.Add(4, ("OUT", 1, (args, modes) =>
                output.Enqueue(mh.getValue(args[0], modes[0], this.memory))));

            //JUMPS
            opcodes.Add(5, ("JT", 2, (args, modes) =>
            {
                if (mh.getValue(args[0], modes[0], this.memory) != 0)
                    ip = mh.getValue(args[1], modes[1], this.memory);
            }));
            opcodes.Add(6, ("JF", 2, (args, modes) =>
            {
                if (mh.getValue(args[0], modes[0], this.memory) == 0)
                    ip = mh.getValue(args[1], modes[1], this.memory);
            }));

            //?
            opcodes.Add(9, ("REL", 1, (args, modes) =>
                this.mh.relativeBase += this.mh.getValue(args[0], modes[0], this.memory)));

            //HALT
            opcodes.Add(99, ("HLT", 0, (_, _) => this.hasHalted = true));
        }

        public Computer(Dictionary<long, long> memory)
        {
            this.memory = new Dictionary<long, long>(memory.Count);
            foreach (KeyValuePair<long, long> kvp in memory)
                this.memory.Add(kvp.Key, kvp.Value);

            this.init();
        }

        public Computer(Dictionary<int, int> memory)
        {
            this.memory = new Dictionary<long, long>(memory.Count);
            foreach (KeyValuePair<int, int> kvp in memory)
                this.memory.Add(kvp.Key, kvp.Value);

            this.init();
        }

        public long GetMemoryAt(long pos)
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
            long opcode = this.GetMemoryAt(ip++);
            (string desc, int argNum, Action<long[], long[]> implementation) =
                opcodes[opcode % 100];

            opcode /= 100;
            long[] modes = new long[argNum];
            for (int i = 0; i < modes.Length; i++)
            {
                modes[i] = opcode % 10;
                opcode /= 10;
            }

            long[] arguments = new long[argNum];
            for (int i = 0; i < arguments.Length; i++)
            {
                arguments[i] = this.GetMemoryAt(ip++);
            }

            try
            {
                implementation(arguments, modes);
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