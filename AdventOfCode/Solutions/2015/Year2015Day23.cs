using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions;

public class Year2015Day23 : Solution
{
    public override string Part1(string input)
    {
        (string, string)[] instructions = input.Split('\n').Select(l => (l[..3], l[4..])).ToArray();
        int ip = 0;

        Dictionary<char, uint> registers = new()
        {
            ['a'] = 0,
            ['b'] = 0
        };

        while (ip < instructions.Length && ip >= 0)
        {
            (string op, string args) = instructions[ip];
            string[] argv = args.Split(", ");
            switch (op)
            {
                case "hlf":
                    registers[args[0]] /= 2;
                    break;
                case "tpl":
                    registers[args[0]] *= 3;
                    break;
                case "inc":
                    registers[args[0]]++;
                    break;
                case "jmp":
                    ip += int.Parse(args) - 1;
                    break;
                case "jie":
                    if (registers[argv[0][0]] % 2 == 0)
                        ip += int.Parse(argv[1]) - 1;
                    break;
                case "jio":
                    if (registers[argv[0][0]] == 1)
                        ip += int.Parse(argv[1]) - 1;
                    break;
            }

            ip++;
        }

        return registers['b'].ToString();
    }

    public override string Part2(string input)
    {
        (string, string)[] instructions = input.Split('\n').Select(l => (l[..3], l[4..])).ToArray();
        int ip = 0;

        Dictionary<char, uint> registers = new()
        {
            ['a'] = 1,
            ['b'] = 0
        };

        while (ip < instructions.Length && ip >= 0)
        {
            (string op, string args) = instructions[ip];
            string[] argv = args.Split(", ");
            switch (op)
            {
                case "hlf":
                    registers[args[0]] /= 2;
                    break;
                case "tpl":
                    registers[args[0]] *= 3;
                    break;
                case "inc":
                    registers[args[0]]++;
                    break;
                case "jmp":
                    ip += int.Parse(args) - 1;
                    break;
                case "jie":
                    if (registers[argv[0][0]] % 2 == 0)
                        ip += int.Parse(argv[1]) - 1;
                    break;
                case "jio":
                    if (registers[argv[0][0]] == 1)
                        ip += int.Parse(argv[1]) - 1;
                    break;
            }

            ip++;
        }

        return registers['b'].ToString();        }
}