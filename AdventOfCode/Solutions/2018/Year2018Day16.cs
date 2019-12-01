using System;
using System.Linq;

namespace AdventOfCode.Solutions
{
    public class Year2018Day16 : Solution
    {
        private enum OPCODES
        {
            ADDR, ADDI, MULR, MULI, BANR, BANI, BORR, BORI, SETR, SETI, GTIR, GTRI, GTRR, EQIR, EQRI, EQRR
        }

        private static int[] executeOPCode(OPCODES op, int[] registers, int A, int B, int C)
        {
            int[] r = new int[registers.Length];
            Array.Copy(registers, 0, r, 0, registers.Length);

            switch (op)
            {
                case OPCODES.ADDR:
                    r[C] = r[A] + r[B];
                    break;
                case OPCODES.ADDI:
                    r[C] = r[A] + B;
                    break;
                case OPCODES.MULR:
                    r[C] = r[A] * r[B];
                    break;
                case OPCODES.MULI:
                    r[C] = r[A] * B;
                    break;
                case OPCODES.BANR:
                    r[C] = r[A] & r[B];
                    break;
                case OPCODES.BANI:
                    r[C] = r[A] & B;
                    break;
                case OPCODES.BORR:
                    r[C] = r[A] | r[B];
                    break;
                case OPCODES.BORI:
                    r[C] = r[A] | B;
                    break;
                case OPCODES.SETR:
                    r[C] = r[A];
                    break;
                case OPCODES.SETI:
                    r[C] = A;
                    break;
                case OPCODES.GTIR:
                    r[C] = A > r[B] ? 1 : 0;
                    break;
                case OPCODES.GTRI:
                    r[C] = r[A] > B ? 1 : 0;
                    break;
                case OPCODES.GTRR:
                    r[C] = r[A] > r[B] ? 1 : 0;
                    break;
                case OPCODES.EQIR:
                    r[C] = A == r[B] ? 1 : 0;
                    break;
                case OPCODES.EQRI:
                    r[C] = r[A] == B ? 1 : 0;
                    break;
                case OPCODES.EQRR:
                    r[C] = r[A] == r[B] ? 1 : 0;
                    break;
            }

            return r;
        }

        public override string Part1(string input)
        {
            {
                string[] tmp = input.Split("\r\n\r\n\r\n", 2);
                if (tmp.Length == 1) //unix line endings
                    input = input.Split("\n\n\n", 2)[0];
                else
                    input = tmp[0];
            }

            int moreThanThreePossibilities = 0;

            //[NUMBER,OPCODE]
            bool[,] opCodesPossible = new bool[16, 16];
            for (int i = 0; i < opCodesPossible.GetLength(0); i++)
                for (int j = 0; j < opCodesPossible.GetLength(1); j++)
                    opCodesPossible[i, j] = true;

            string[] samples = input.Split("\r\n\r\n"); //TODO unix line ending support
            foreach (string sample in samples)
            {
                string[] lines = sample.Split(new[] {'\r', '\n'}, StringSplitOptions.RemoveEmptyEntries);
                int[] beforeRegisters = lines[0].Split(' ', 2)[1]
                    .Split(new[] { '[', ']', ',', ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
                int[] afterRegisters = lines[2].Split(' ', 2)[1]
                    .Split(new[] { '[', ']', ',', ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
                int[] instruction = lines[1].Split(' ').Select(int.Parse).ToArray();

                byte sum = 0; 
                for (int i = 0; i < 16; i++)
                {
                    if (!executeOPCode((OPCODES) i, beforeRegisters, instruction[1], instruction[2], instruction[3])
                        .SequenceEqual(afterRegisters))
                        opCodesPossible[instruction[0], i] = false;
                    else
                        sum++;
                }

                if (sum >= 3)
                    moreThanThreePossibilities++;
            }

            return moreThanThreePossibilities.ToString();
        }

        private static bool isUnambiguous(bool[,] matrix)
        {
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                bool hasHadOne = false;
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    if (matrix[i, j])
                    {
                        if (!hasHadOne)
                            hasHadOne = true;
                        else
                            return false;
                    }
                }
            }

            return true;
        }

        public override string Part2(string input)
        {
            string inputFirstPart;
            {
                string[] tmp = input.Split("\r\n\r\n\r\n", 2);
                if (tmp.Length == 1) //unix line endings
                    inputFirstPart = input.Split("\n\n\n", 2)[0];
                else
                    inputFirstPart = tmp[0];
            }

            //[NUMBER,OPCODE]
            bool[,] opCodesPossible = new bool[16, 16];
            for (int i = 0; i < opCodesPossible.GetLength(0); i++)
                for (int j = 0; j < opCodesPossible.GetLength(1); j++)
                    opCodesPossible[i, j] = true;

            string[] samples = inputFirstPart.Split("\r\n\r\n"); //TODO unix line ending support
            foreach (string sample in samples)
            {
                string[] lines = sample.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                int[] beforeRegisters = lines[0].Split(' ', 2)[1]
                    .Split(new[] { '[', ']', ',', ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
                int[] afterRegisters = lines[2].Split(' ', 2)[1]
                    .Split(new[] { '[', ']', ',', ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
                int[] instruction = lines[1].Split(' ').Select(int.Parse).ToArray();
                
                for (int i = 0; i < 16; i++)
                {
                    if (!executeOPCode((OPCODES)i, beforeRegisters, instruction[1], instruction[2], instruction[3])
                        .SequenceEqual(afterRegisters))
                        opCodesPossible[instruction[0], i] = false;
                }
            }

            while (!isUnambiguous(opCodesPossible))
            {
                for (byte i = 0; i < opCodesPossible.GetLength(0); i++)
                {
                    byte num = 0;
                    byte index = 0;
                    for (byte j = 0; j < opCodesPossible.GetLength(1); j++)
                    {
                        if (opCodesPossible[i, j])
                        {
                            num++;
                            index = j;
                        }
                    }

                    if (num == 1)
                    {
                        for (byte i2 = 0; i2 < opCodesPossible.GetLength(0); i2++)
                            if (i2 != i)
                                opCodesPossible[i2, index] = false;
                    }
                }

                for (byte i = 0; i < opCodesPossible.GetLength(1); i++)
                {
                    byte num = 0;
                    byte index = 0;
                    for (byte j = 0; j < opCodesPossible.GetLength(0); j++)
                    {
                        if (opCodesPossible[i, j])
                        {
                            num++;
                            index = j;
                        }
                    }

                    if (num == 1)
                    {
                        for (byte i2 = 0; i2 < opCodesPossible.GetLength(1); i2++)
                            if (i2 != i)
                                opCodesPossible[i2, index] = false;
                    }
                }
            }

            OPCODES[] opCodes = new OPCODES[16];
            for (int i = 0; i < opCodesPossible.GetLength(0); i++)
            {
                for (int j = 0; j < opCodesPossible.GetLength(1); j++)
                {
                    if (opCodesPossible[i, j])
                        opCodes[i] = (OPCODES) j;
                }
            }

            string[] demoProgram;
            {
                string[] tmp = input.Split("\r\n\r\n\r\n\r\n", 2);
                if (tmp.Length == 1) //unix line endings
                    demoProgram = input.Split("\n\n\n\n", 2)[1].Split(new []{'\r', '\n'}, StringSplitOptions.RemoveEmptyEntries);
                else
                    demoProgram = tmp[1].Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            }

            int[] registers = new int[4];
            foreach (string i in demoProgram)
            {
                int[] instruction = i.Split(' ').Select(int.Parse).ToArray();
                registers = executeOPCode(opCodes[instruction[0]], registers, instruction[1], instruction[2],
                    instruction[3]);
            }

            return registers[0].ToString();
        }
    }
}