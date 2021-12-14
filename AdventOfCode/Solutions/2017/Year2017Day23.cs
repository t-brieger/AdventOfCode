using System;
using System.Collections.Generic;

namespace AdventOfCode.Solutions;

class Year2017Day23 : Solution
{
    private static int GetValue(string s, IReadOnlyDictionary<char, int> registers)
    {
        if (s[0] >= 'a' && s[0] <= 'z')
            return registers[s[0]];
        return Int32.Parse(s);
    }

    public override string Part1(string input)
    {
        string[] instructions = input.Split('\n', StringSplitOptions.RemoveEmptyEntries);

        Dictionary<char, int> registers = new(8);
        for (char c = 'a'; c <= 'h'; c++)
            registers[c] = 0;

        int mulCount = 0;

        for (int i = 0; i < instructions.Length; i++)
        {
            string instruction = instructions[i];
            string[] split = instruction.Split(' ');

            switch (split[0])
            {
                case "set":
                    registers[split[1][0]] = GetValue(split[2], registers);
                    break;
                case "sub":
                    registers[split[1][0]] -= GetValue(split[2], registers);
                    break;
                case "mul":
                    mulCount++;
                    registers[split[1][0]] *= GetValue(split[2], registers);
                    break;
                case "jnz":
                    if (GetValue(split[1], registers) != 0)
                        i += GetValue(split[2], registers) - 1;
                    break;
                default:
                    throw new Exception("invalid instruction: " + split[0]);
            }
        }

        return mulCount.ToString();
    }

    public override string Part2(string input)
    {
        // this doesnt really solve the general version of the puzzle (optimize intCode), which I'd
        // like to do eventually too, but it works for all 3 inputs that I tried it with: i assumed 
        // that inputs only differ in the second operand to the first "set" instruction, and that 
        // it is always a 2-digit number.

        int b = Int32.Parse(input.Substring(6, 2)) * 100 + 100000;
        int c = b + 17000;

        //# of composites from b to c
        int h = 0;

        for (; b <= c; b += 17)
        for (int i = 2; i * i <= b; i++)
        {
            if (b % i != 0) continue;
            h++;
            break;
        }

        return h.ToString();
    }
}