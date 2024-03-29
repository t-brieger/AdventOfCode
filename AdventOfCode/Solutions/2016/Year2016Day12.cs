﻿using System.Linq;

namespace AdventOfCode.Solutions;

public class Year2016Day12 : Solution
{
    public override string Part1(string input)
    {
        string[][] instructions = input.Split('\n').Select(l => l.Split(' ')).ToArray();
        long[] registers = new long[] {0, 0, 0, 0};

        Assembunny.Run(registers, instructions);
        
        return registers[0].ToString();
    }

    public override string Part2(string input)
    {
        string[][] instructions = input.Split('\n').Select(l => l.Split(' ')).ToArray();
        long[] registers = new long[] {0, 0, 1, 0};

        Assembunny.Run(registers, instructions);
        
        return registers[0].ToString();
    }
}
