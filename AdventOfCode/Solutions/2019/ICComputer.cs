using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions._2019
{
    public abstract class ICComputer
    {
        public int[] memory;
        public int pc;

        public bool hasHalted = false;
        public bool isWaitingForInput = false;
        
        public Queue<int> input;

        public Queue<int> output = new Queue<int>();

        protected static Dictionary<int, ICInstruction> Opcodes = new Dictionary<int, ICInstruction>();

        static ICComputer()
        {
            // C = A + B
            Opcodes.Add(01, new ICInstruction((pc, mem, mode, _) =>
            {
                mem[mem[pc + 3]] = ModeHelper.getValue(mem, mode[0], mem[pc + 1]) +
                                              ModeHelper.getValue(mem, mode[1], mem[pc + 2]);
                return 0;
            }, 4));
            // C = A * B
            Opcodes.Add(02, new ICInstruction((pc, mem, mode, _) =>
            {
                mem[mem[pc + 3]] = ModeHelper.getValue(mem, mode[0], mem[pc + 1]) *
                                              ModeHelper.getValue(mem, mode[1], mem[pc + 2]);
                return 0;
            }, 4));

            //INPUT
            Opcodes.Add(03, new ICInstruction((pc, mem, __, instance) =>
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
            Opcodes.Add(04, new ICInstruction((pc, mem, mode, instance) =>
            {
                instance.output.Enqueue(ModeHelper.getValue(mem, mode[0], mem[pc + 1]));
                return 0;
            }, 2));

            //JUMP IF NONZERO
            Opcodes.Add(05, new ICInstruction((pc, mem, mode, instance) =>
            {
                if (ModeHelper.getValue(mem, mode[0], mem[pc + 1]) != 0)
                    return ModeHelper.getValue(mem, mode[1], mem[pc + 2]);
                return pc + 3;
            }, -3));
            //JUMP IF ZERO
            Opcodes.Add(06, new ICInstruction((pc, mem, mode, instance) =>
            {
                if (ModeHelper.getValue(mem, mode[0], mem[pc + 1]) == 0)
                    return ModeHelper.getValue(mem, mode[1], mem[pc + 2]);
                return pc + 3;
            }, -3));

            //LESS THAN
            Opcodes.Add(07, new ICInstruction((pc, mem, mode, instance) =>
            {
                mem[mem[pc + 3]] = ModeHelper.getValue(mem, mode[0], mem[pc + 1]) <
                              ModeHelper.getValue(mem, mode[1], mem[pc + 2])
                    ? 1
                    : 0;
                return 0;
            }, 4));
            //EQUALS
            Opcodes.Add(08, new ICInstruction((pc, mem, mode, instance) =>
                {
                    mem[mem[pc + 3]] = ModeHelper.getValue(mem, mode[0], mem[pc + 1]) ==
                              ModeHelper.getValue(mem, mode[1], mem[pc + 2])
                    ? 1
                    : 0;
                return 0;
            }, 4));

            //HALT
            Opcodes.Add(99, new ICInstruction((_, __, ___, instance) =>
            {
                instance.hasHalted = true;
                return 0;
            }, 1));
        }

        protected ICComputer(int[] mem, Queue<int> input)
        {
            this.memory = mem;
            this.input = input;
        }

        protected ICComputer(int[] mem) : this(mem, new Queue<int>())
        {
        }

        public abstract int? isDone();

        public void doInstruction()
        {
            if (Opcodes.ContainsKey(memory[pc] % 100))
            {
                var instruction = Opcodes[memory[pc] % 100];

                var modes = (memory[pc] / 100).ToString().PadLeft(Math.Abs(instruction.argc) - 1, '0').ToCharArray().Reverse()
                    .Select(c => (Modes) (c - '0')).ToArray();

                var x = instruction.action(pc, memory, modes, this);
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

    public class ICInstruction
    {
        public int argc;

        public Func<int, int[], Modes[], ICComputer, int> action;


        public ICInstruction(Func<int, int[], Modes[], ICComputer, int> action, int argc)
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
        public static int getValue(int[] mem, Modes mode, int value)
        {
            switch (mode)
            {
                case Modes.INDEX:
                    return mem[value];
                case Modes.DIRECT:
                    return value;
                default:
                    return -1;
            }
        }
    }
}