using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions
{
    public abstract class IcComputer
    {
        public int[] memory;
        public int pc;

        public bool hasHalted;
        public bool isWaitingForInput;
        
        public Queue<int> input;

        public Queue<int> output = new Queue<int>();

        protected static Dictionary<int, IcInstruction> opcodes = new Dictionary<int, IcInstruction>();

        static IcComputer()
        {
            // C = A + B
            opcodes.Add(01, new IcInstruction((pc, mem, mode, _) =>
            {
                mem[mem[pc + 3]] = ModeHelper.GetValue(mem, mode[0], mem[pc + 1]) +
                                              ModeHelper.GetValue(mem, mode[1], mem[pc + 2]);
                return 0;
            }, 4));
            // C = A * B
            opcodes.Add(02, new IcInstruction((pc, mem, mode, _) =>
            {
                mem[mem[pc + 3]] = ModeHelper.GetValue(mem, mode[0], mem[pc + 1]) *
                                              ModeHelper.GetValue(mem, mode[1], mem[pc + 2]);
                return 0;
            }, 4));

            //INPUT
            opcodes.Add(03, new IcInstruction((pc, mem, __, instance) =>
            {
                if (instance.input.Count != 0)
                {
                    instance.isWaitingForInput = false;
                    mem[mem[pc + 1]] = instance.input.Dequeue();
                    return pc + 2;
                }
                instance.isWaitingForInput = true;
                return 0;
            }, -2));
            //OUTPUT
            opcodes.Add(04, new IcInstruction((pc, mem, mode, instance) =>
            {
                instance.output.Enqueue(ModeHelper.GetValue(mem, mode[0], mem[pc + 1]));
                return 0;
            }, 2));

            //JUMP IF NONZERO
            opcodes.Add(05, new IcInstruction((pc, mem, mode, instance) =>
            {
                if (ModeHelper.GetValue(mem, mode[0], mem[pc + 1]) != 0)
                    return ModeHelper.GetValue(mem, mode[1], mem[pc + 2]);
                return pc + 3;
            }, -3));
            //JUMP IF ZERO
            opcodes.Add(06, new IcInstruction((pc, mem, mode, instance) =>
            {
                if (ModeHelper.GetValue(mem, mode[0], mem[pc + 1]) == 0)
                    return ModeHelper.GetValue(mem, mode[1], mem[pc + 2]);
                return pc + 3;
            }, -3));

            //LESS THAN
            opcodes.Add(07, new IcInstruction((pc, mem, mode, instance) =>
            {
                mem[mem[pc + 3]] = ModeHelper.GetValue(mem, mode[0], mem[pc + 1]) <
                              ModeHelper.GetValue(mem, mode[1], mem[pc + 2])
                    ? 1
                    : 0;
                return 0;
            }, 4));
            //EQUALS
            opcodes.Add(08, new IcInstruction((pc, mem, mode, instance) =>
                {
                    mem[mem[pc + 3]] = ModeHelper.GetValue(mem, mode[0], mem[pc + 1]) ==
                              ModeHelper.GetValue(mem, mode[1], mem[pc + 2])
                    ? 1
                    : 0;
                return 0;
            }, 4));

            //HALT
            opcodes.Add(99, new IcInstruction((_, __, ___, instance) =>
            {
                instance.hasHalted = true;
                return 0;
            }, 1));
        }

        protected IcComputer(int[] mem, Queue<int> input)
        {
            this.memory = mem;
            this.input = input;
        }

        protected IcComputer(int[] mem) : this(mem, new Queue<int>())
        {
        }

        public abstract int? IsDone();

        public void DoInstruction()
        {
            if (opcodes.ContainsKey(memory[pc] % 100))
            {
                IcInstruction instruction = opcodes[memory[pc] % 100];

                Modes[] modes = (memory[pc] / 100).ToString().PadLeft(Math.Abs(instruction.argc) - 1, '0').ToCharArray().Reverse()
                    .Select(c => (Modes) (c - '0')).ToArray();

                int x = instruction.action(pc, memory, modes, this);
                if (instruction.argc > 0)
                    pc += instruction.argc;
                else
                    pc = x;
            }
            else
            {
                throw new Exception("Unknown Opcode: " + memory[pc]);
            }
        }
    }

    public class IcInstruction
    {
        public int argc;

        public Func<int, int[], Modes[], IcComputer, int> action;


        public IcInstruction(Func<int, int[], Modes[], IcComputer, int> action, int argc)
        {
            this.action = action;
            this.argc = argc;
        }
    }

    public enum Modes
    {
        INDEX = 0, DIRECT = 1
    }

    public static class ModeHelper
    {
        public static int GetValue(int[] mem, Modes mode, int value)
        {
            return mode switch
            {
                Modes.INDEX => mem[value],
                Modes.DIRECT => value,
                _ => -1
            };
        }
    }
}