using System.Linq;

namespace AdventOfCode.Solutions;

public class Year2016Day23 : Solution
{
    public override string Part1(string input)
    {
        string[][] instructions = input.Split('\n').Select(l => l.Split(' ')).ToArray();
        long[] registers = new long[] {7, 0, 0, 0};

        Assembunny.Run(registers, instructions);
    
        return registers[0].ToString();
    }

    public override string Part2(string input)
    {
        // TODO:
        // adding optimisation to interpreters/compilers *sucks*, and this runs in around 2 minutes on my machine
        // (in debug mode) - it is absolutely not good, but I want the optimisation, if I do add it, to be generally
        // applicable, instead of assuming some things about the input program.
        string[][] instructions = input.Split('\n').Select(l => l.Split(' ')).ToArray();
        long[] registers = new long[] {12, 0, 0, 0};

        Assembunny.Run(registers, instructions);
    
        return registers[0].ToString();
    }
}
