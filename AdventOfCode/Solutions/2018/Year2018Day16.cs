using System;
using System.Linq;

namespace AdventOfCode.Solutions;

public class Year2018Day16 : Solution
{
    public enum Opcodes
    {
        ADDR,
        ADDI,
        MULR,
        MULI,
        BANR,
        BANI,
        BORR,
        BORI,
        SETR,
        SETI,
        GTIR,
        GTRI,
        GTRR,
        EQIR,
        EQRI,
        EQRR
    }

    public static int[] ExecuteOpCode(Opcodes op, int[] registers, int a, int b, int c)
    {
        int[] r = new int[registers.Length];
        Array.Copy(registers, 0, r, 0, registers.Length);

        r[c] = op switch
        {
            Opcodes.ADDR => r[a] + r[b],
            Opcodes.ADDI => r[a] + b,
            Opcodes.MULR => r[a] * r[b],
            Opcodes.MULI => r[a] * b,
            Opcodes.BANR => r[a] & r[b],
            Opcodes.BANI => r[a] & b,
            Opcodes.BORR => r[a] | r[b],
            Opcodes.BORI => r[a] | b,
            Opcodes.SETR => r[a],
            Opcodes.SETI => a,
            Opcodes.GTIR => a > r[b] ? 1 : 0,
            Opcodes.GTRI => r[a] > b ? 1 : 0,
            Opcodes.GTRR => r[a] > r[b] ? 1 : 0,
            Opcodes.EQIR => a == r[b] ? 1 : 0,
            Opcodes.EQRI => r[a] == b ? 1 : 0,
            Opcodes.EQRR => r[a] == r[b] ? 1 : 0,
            _ => r[c]
        };

        return r;
    }

    public override string Part1(string input)
    {
        {
            string[] tmp = input.Split("\n\n\n", 2);
            input = tmp[0];
        }

        int moreThanThreePossibilities = 0;

        //[NUMBER,OPCODE]
        bool[,] opCodesPossible = new bool[16, 16];
        for (int i = 0; i < opCodesPossible.GetLength(0); i++)
        for (int j = 0; j < opCodesPossible.GetLength(1); j++)
            opCodesPossible[i, j] = true;

        string[] samples = input.Split("\n\n");
        foreach (string sample in samples)
        {
            string[] lines = sample.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            int[] beforeRegisters = lines[0].Split(' ', 2)[1]
                .Split(new[] { '[', ']', ',', ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(Int32.Parse)
                .ToArray();
            int[] afterRegisters = lines[2].Split(' ', 2)[1]
                .Split(new[] { '[', ']', ',', ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(Int32.Parse)
                .ToArray();
            int[] instruction = lines[1].Split(' ').Select(Int32.Parse).ToArray();

            byte sum = 0;
            for (int i = 0; i < 16; i++)
                if (!ExecuteOpCode((Opcodes)i, beforeRegisters, instruction[1], instruction[2], instruction[3])
                        .SequenceEqual(afterRegisters))
                    opCodesPossible[instruction[0], i] = false;
                else
                    sum++;

            if (sum >= 3)
                moreThanThreePossibilities++;
        }

        return moreThanThreePossibilities.ToString();
    }

    private static bool IsUnambiguous(bool[,] matrix)
    {
        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            bool hasHadOne = false;
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                if (!matrix[i, j]) continue;
                if (!hasHadOne)
                    hasHadOne = true;
                else
                    return false;
            }
        }

        return true;
    }

    public override string Part2(string input)
    {
        string inputFirstPart;
        {
            string[] tmp = input.Split("\n\n\n", 2);
            inputFirstPart = tmp.Length == 1 ? input.Split("\n\n\n", 2)[0] : tmp[0];
        }

        //[NUMBER,OPCODE]
        bool[,] opCodesPossible = new bool[16, 16];
        for (int i = 0; i < opCodesPossible.GetLength(0); i++)
        for (int j = 0; j < opCodesPossible.GetLength(1); j++)
            opCodesPossible[i, j] = true;

        string[] samples = inputFirstPart.Split("\n\n");
        foreach (string sample in samples)
        {
            string[] lines = sample.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            int[] beforeRegisters = lines[0].Split(' ', 2)[1]
                .Split(new[] { '[', ']', ',', ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(Int32.Parse)
                .ToArray();
            int[] afterRegisters = lines[2].Split(' ', 2)[1]
                .Split(new[] { '[', ']', ',', ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(Int32.Parse)
                .ToArray();
            int[] instruction = lines[1].Split(' ').Select(Int32.Parse).ToArray();

            for (int i = 0; i < 16; i++)
                if (!ExecuteOpCode((Opcodes)i, beforeRegisters, instruction[1], instruction[2], instruction[3])
                        .SequenceEqual(afterRegisters))
                    opCodesPossible[instruction[0], i] = false;
        }

        while (!IsUnambiguous(opCodesPossible))
        {
            for (byte i = 0; i < opCodesPossible.GetLength(0); i++)
            {
                byte num = 0;
                byte index = 0;
                for (byte j = 0; j < opCodesPossible.GetLength(1); j++)
                {
                    if (!opCodesPossible[i, j]) continue;
                    num++;
                    index = j;
                }

                if (num != 1) continue;
                for (byte i2 = 0; i2 < opCodesPossible.GetLength(0); i2++)
                    if (i2 != i)
                        opCodesPossible[i2, index] = false;
            }

            for (byte i = 0; i < opCodesPossible.GetLength(1); i++)
            {
                byte num = 0;
                byte index = 0;
                for (byte j = 0; j < opCodesPossible.GetLength(0); j++)
                {
                    if (!opCodesPossible[i, j]) continue;
                    num++;
                    index = j;
                }

                if (num != 1) continue;
                for (byte i2 = 0; i2 < opCodesPossible.GetLength(1); i2++)
                    if (i2 != i)
                        opCodesPossible[i2, index] = false;
            }
        }

        Opcodes[] opCodes = new Opcodes[16];
        for (int i = 0; i < opCodesPossible.GetLength(0); i++)
        for (int j = 0; j < opCodesPossible.GetLength(1); j++)
            if (opCodesPossible[i, j])
                opCodes[i] = (Opcodes)j;

        string[] demoProgram = input.Split("\n\n\n\n", 2)[1].Split('\n', StringSplitOptions.RemoveEmptyEntries);

        int[] registers = new int[4];
        registers = demoProgram.Select(i => i.Split(' ').Select(Int32.Parse).ToArray()).Aggregate(registers,
            (current, instruction) => ExecuteOpCode(opCodes[instruction[0]], current, instruction[1],
                instruction[2], instruction[3]));

        return registers[0].ToString();
    }

    // ReSharper disable once InconsistentNaming
}