using System;
using System.Collections.Generic;

namespace AdventOfCode.Solutions;

public class Year2018Day19 : Solution
{
    public override string Part1(string input)
    {
        int ipRegister = -1;
        string[] lines = input.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        List<(Year2018Day16.Opcodes op, int a, int b, int c)> instructions = new(lines.Length - 1);
        for (int i = 0; i < lines.Length; i++)
        {
            if (lines[i].StartsWith("#ip "))
            {
                ipRegister = lines[i]["#ip ".Length] - '0';
                continue;
            }

            string[] parts = lines[i].Split(' ');
            Year2018Day16.Opcodes op = Enum.Parse<Year2018Day16.Opcodes>(parts[0].ToUpper());
            int a = int.Parse(parts[1]);
            int b = int.Parse(parts[2]);
            int c = int.Parse(parts[3]);
            instructions.Add((op, a, b, c));
        }

        int ip = 0;
        int[] registers = new int[6];
        while (ip >= 0 && ip < instructions.Count)
        {
            registers[ipRegister] = ip;

            (Year2018Day16.Opcodes op, int a, int b, int c) = instructions[ip];
            registers = Year2018Day16.ExecuteOpCode(op, registers, a, b, c);
            
            ip = registers[ipRegister];
            ip++;
        }

        return registers[0].ToString();
    }

    public override string Part2(string input)
    {
        int ipRegister = -1;
        string[] lines = input.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        List<(Year2018Day16.Opcodes op, int a, int b, int c)> instructions = new(lines.Length - 1);
        for (int i = 0; i < lines.Length; i++)
        {
            if (lines[i].StartsWith("#ip "))
            {
                ipRegister = lines[i]["#ip ".Length] - '0';
                continue;
            }

            string[] parts = lines[i].Split(' ');
            Year2018Day16.Opcodes op = Enum.Parse<Year2018Day16.Opcodes>(parts[0].ToUpper());
            int a = int.Parse(parts[1]);
            int b = int.Parse(parts[2]);
            int c = int.Parse(parts[3]);
            instructions.Add((op, a, b, c));
        }

        int ip = 0;
        int[] registers = new int[6];
        registers[0] = 1;
        while (ip >= 0 && ip < instructions.Count)
        {
            registers[ipRegister] = ip;

            (Year2018Day16.Opcodes op, int a, int b, int c) = instructions[ip];
            registers = Year2018Day16.ExecuteOpCode(op, registers, a, b, c);
            
            ip = registers[ipRegister];
            ip++;
        }

        return registers[0].ToString();
    }
}