using System;
using System.Linq;

namespace AdventOfCode.Solutions;

public class Year2016Day12 : Solution
{
    private void Run(long[] registers, string[][] instructions)
    {
        for (int i = 0; i < instructions.Length; i++)
        {
            string[] instruction = instructions[i];

            switch (instruction[0])
            {
                case "cpy":
                    long valueA = instruction[1][0] is >= 'a' and <= 'd'
                        ? registers[instruction[1][0] - 'a']
                        : int.Parse(instruction[1]);
                    registers[instruction[2][0] - 'a'] = valueA;
                    break;
                case "inc":
                    registers[instruction[1][0] - 'a']++;
                    break;
                case "dec":
                    registers[instruction[1][0] - 'a']--;
                    break;
                case "jnz":
                    // assumes that, in "jnz x y", y is always a register, not a number literal.
                    // also, "-1" because we are also incrementing i at every iteration of the loop
                    int jumpAmount = int.Parse(instruction[2]) - 1;
                    
                    bool jump = (instruction[1][0] is >= 'a' and <= 'd'
                        ? registers[instruction[1][0] - 'a']
                        : int.Parse(instruction[1])) != 0;
                    
                    if (jump)
                        i += jumpAmount;
                    break;
                default:
                    throw new ArgumentException();
            }
        }
    }
    
    public override string Part1(string input)
    {
        string[][] instructions = input.Split('\n').Select(l => l.Split(' ')).ToArray();
        long[] registers = new long[] {0, 0, 0, 0};

        Run(registers, instructions);
        
        return registers[0].ToString();
    }

    public override string Part2(string input)
    {
        string[][] instructions = input.Split('\n').Select(l => l.Split(' ')).ToArray();
        long[] registers = new long[] {0, 0, 1, 0};

        Run(registers, instructions);
        
        return registers[0].ToString();
    }
}
