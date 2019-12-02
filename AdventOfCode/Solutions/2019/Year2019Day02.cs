using System.Linq;

namespace AdventOfCode.Solutions._2019
{
    public class Year2019Day02 : Solution
    {
        public override string Part1(string input)
        {
            int[] mem = input.Split(',').Select(int.Parse).ToArray();

            mem[1] = 12;
            mem[2] = 02;

            for (int ip = 0; ip < mem.Length; ip++)
            {
                if (mem[ip] == 1)
                {
                    mem[mem[ip + 3]] = mem[mem[ip + 1]] + mem[mem[ip + 2]];
                    ip += 3;
                }
                else if (mem[ip] == 2)
                {
                    mem[mem[ip + 3]] = mem[mem[ip + 1]] * mem[mem[ip + 2]];
                    ip += 3;
                }
                else if (mem[ip] == 99)
                    break;
            }

            return mem[0].ToString();
        }

        // not the most elegant solution, but still completes in 0.1 seconds on my machine, so it's not a giant problem
        public override string Part2(string input)
        {
            for (int noun = 0; noun < 100; noun++)
            {
                for (int verb = 0; verb < 100; verb++)
                {
                    int[] mem = input.Split(',').Select(int.Parse).ToArray();

                    mem[1] = noun;
                    mem[2] = verb;

                    for (int ip = 0; ip < mem.Length; ip++)
                    {
                        if (mem[ip] == 1)
                        {
                            mem[mem[ip + 3]] = mem[mem[ip + 1]] + mem[mem[ip + 2]];
                            ip += 3;
                        }
                        else if (mem[ip] == 2)
                        {
                            mem[mem[ip + 3]] = mem[mem[ip + 1]] * mem[mem[ip + 2]];
                            ip += 3;
                        }
                        else if (mem[ip] == 99)
                            break;
                    }

                    if (mem[0] == 19690720)
                        return (100 * noun + verb).ToString();
                }
            }

            return "";
        }
    }
}