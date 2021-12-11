using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions.IntCode
{
    // ReSharper disable once InconsistentNaming
    public class Computer
    {
        private readonly Dictionary<long, long> memory;
        private readonly ModeHelper mh = new();

        public bool hasHalted;
        public Queue<long> input;

        private long ip;
        private Dictionary<long, (string desc, int argNum, Action<long[], long[]> implementation)> opcodes;
        public Queue<long> output;
        public bool waitingForInput;

        public Computer(string program) : this(program.Split(',').Select(long.Parse))
        {
        }

        private Computer(IEnumerable<long> program) : this(program.Select((x, i) => (i, x))
            .ToDictionary(t => (long)t.Item1, t => t.Item2))
        {
        }

        private Computer(Dictionary<long, long> memory)
        {
            this.memory = new Dictionary<long, long>(memory.Count);
            foreach ((long key, long value) in memory)
                this.memory.Add(key, value);

            this.Init();
        }

        public Computer(Dictionary<int, int> memory)
        {
            this.memory = new Dictionary<long, long>(memory.Count);
            foreach ((int key, int value) in memory)
                this.memory.Add(key, value);

            this.Init();
        }

        private void Init()
        {
            this.ip = 0;
            this.input = new Queue<long>();
            this.output = new Queue<long>();

            this.opcodes = new Dictionary<long, (string desc, int argNum, Action<long[], long[]> implementation)>
            {
                //ARITHMETIC
                {
                    1, ("ADD", 3, (args, modes) => this.memory[this.mh.GetValue(args[2], modes[2], this.memory, true)] =
                        this.mh.GetValue(args[0], modes[0], this.memory) +
                        this.mh.GetValue(args[1], modes[1], this.memory))
                },
                {
                    2, ("MUL", 3, (args, modes) => this.memory[this.mh.GetValue(args[2], modes[2], this.memory, true)] =
                        this.mh.GetValue(args[0], modes[0], this.memory) *
                        this.mh.GetValue(args[1], modes[1], this.memory))
                },
                {
                    7, ("LT", 3, (args, modes) => this.memory[this.mh.GetValue(args[2], modes[2], this.memory, true)] =
                        this.mh.GetValue(args[0], modes[0], this.memory) <
                        this.mh.GetValue(args[1], modes[1], this.memory)
                            ? 1
                            : 0)
                },
                {
                    8, ("EQ", 3, (args, modes) => this.memory[this.mh.GetValue(args[2], modes[2], this.memory, true)] =
                        this.mh.GetValue(args[0], modes[0], this.memory) ==
                        this.mh.GetValue(args[1], modes[1], this.memory)
                            ? 1
                            : 0)
                },
                //I/O
                {
                    3, ("IN", 1, (args, modes) =>
                        this.memory[this.mh.GetValue(args[0], modes[0], this.memory, true)] = this.input.Dequeue())
                },
                {
                    4, ("OUT", 1, (args, modes) =>
                        this.output.Enqueue(this.mh.GetValue(args[0], modes[0], this.memory)))
                },
                //JUMPS
                {
                    5, ("JT", 2, (args, modes) =>
                    {
                        if (this.mh.GetValue(args[0], modes[0], this.memory) != 0)
                            this.ip = this.mh.GetValue(args[1], modes[1], this.memory);
                    })
                },
                {
                    6, ("JF", 2, (args, modes) =>
                    {
                        if (this.mh.GetValue(args[0], modes[0], this.memory) == 0)
                            this.ip = this.mh.GetValue(args[1], modes[1], this.memory);
                    })
                },
                //?
                {
                    9, ("REL", 1, (args, modes) =>
                        this.mh.relativeBase += this.mh.GetValue(args[0], modes[0], this.memory))
                },
                //HALT
                { 99, ("HLT", 0, (_, _) => this.hasHalted = true) }
            };
        }

        public long GetMemoryAt(long pos)
        {
            return this.memory.ContainsKey(pos) ? this.memory[pos] : 0;
        }
        
        public void SetMemoryAt(long pos, long val)
        {
            this.memory[pos] = val;
        }

        public void EnqueueInput(long newInput)
        {
            this.input.Enqueue(newInput);
            this.waitingForInput = false;
        }

        /// <summary>
        ///     Steps the program forward one instruction
        /// </summary>
        public void Step()
        {
            if (this.hasHalted)
                return;

            this.waitingForInput = false;
            long opcode = this.GetMemoryAt(this.ip++);
            (string desc, int argNum, Action<long[], long[]> implementation) = this.opcodes[opcode % 100];

            opcode /= 100;
            long[] modes = new long[argNum];
            for (int i = 0; i < modes.Length; i++)
            {
                modes[i] = opcode % 10;
                opcode /= 10;
            }

            long[] arguments = new long[argNum];
            for (int i = 0; i < arguments.Length; i++) arguments[i] = this.GetMemoryAt(this.ip++);

            try
            {
                implementation(arguments, modes);
            }
            catch (InvalidOperationException)
            {
                //input queue is empty
                this.waitingForInput = true;
                this.ip -= argNum + 1;
            }
            catch (Exception e)
            {
                Console.WriteLine(
                    $"IntCodeComputer failed with exception: {e.GetType().Name} at {this.ip}, instruction:");
                Console.WriteLine($"{desc} {string.Join(' ', arguments)}");
                Console.WriteLine(e.Message);
            }
        }

        public void RunUntilHalted()
        {
            while (!this.hasHalted && !this.waitingForInput) this.Step();
        }
    }
}