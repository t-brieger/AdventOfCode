using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions
{
    public class Year2020Day08 : Solution
    {
        private static (bool, int) run(string[][] instructions)
        {
            HashSet<int> linesVisited = new HashSet<int>();
            int accu = 0;

            for (int i = 0; i < instructions.Length; i++)
            {
                if (linesVisited.Contains(i))
                    return (false, accu);
                linesVisited.Add(i);

                switch (instructions[i][0])
                {
                    case "nop":
                        break;
                    case "jmp":
                        i += int.Parse(instructions[i][1]) - 1;
                        break;
                    case "acc":
                        accu += int.Parse(instructions[i][1]);
                        break;
                }
            }


            return (true, accu);
        }

        public override string Part1(string input)
        {
            string[][] instructions = input.Split('\n', StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Split(' ')).ToArray();

            return run(instructions).Item2.ToString();
        }

        public override string Part2(string input)
        {
            string[][] instructions = input.Split('\n', StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Split(' ')).ToArray();
            string[][] currentInstructions;

            for (int i = 0; i < instructions.Length; i++)
            {
                if (instructions[i][0] == "acc")
                    continue;
                currentInstructions = new string[instructions.Length][];
                for (int j = 0; j < instructions.Length; j++)
                {
                    currentInstructions[j] = new string[instructions[j].Length];
                    for (int k = 0; k < instructions[j].Length; k++)
                    {
                        currentInstructions[j][k] = instructions[j][k];
                    }
                }

                currentInstructions[i][0] = currentInstructions[i][0] == "nop" ? "jmp" : "nop";

                (bool exitGracefully, int acc) = run(currentInstructions);
                if (exitGracefully)
                    return acc.ToString();
            }

            return null;
        }
    }
}
