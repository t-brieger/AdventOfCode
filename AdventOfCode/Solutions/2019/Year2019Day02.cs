using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Solutions.IntCode;

namespace AdventOfCode.Solutions
{
    public class Year2019Day02 : Solution
    {
        public override string Part1(string input)
        {
            int[] nums = input.Split(',').Select(int.Parse).ToArray();
            Dictionary<int, int> program = new Dictionary<int, int>();
            for (int i = 0; i < nums.Length; i++)
                program.Add(i, nums[i]);

            program[1] = 12;
            program[2] = 2;
            
            Computer c = new Computer(program);
            c.RunUntilHalted();
            return c.GetMemoryAt(0).ToString();
        }

        public override string Part2(string input)
        {
            int[] nums = input.Split(',').Select(int.Parse).ToArray();
            Dictionary<int, int> program = new Dictionary<int, int>();
            for (int i = 0; i < nums.Length; i++)
                program.Add(i, nums[i]);

            for (int verb = 0; verb < 100; verb++)
            {
                for (int noun = 0; noun < 100; noun++)
                {
                    Dictionary<int, int> newProgram = new Dictionary<int, int>(program.Count);
                    foreach (KeyValuePair<int, int> kvp in program)
                        newProgram.Add(kvp.Key, kvp.Value);

                    newProgram[1] = noun;
                    newProgram[2] = verb;

                    Computer c = new Computer(newProgram);
                    c.RunUntilHalted();
                    if (c.GetMemoryAt(0) == 19690720)
                        return (noun * 100 + verb).ToString();
                }
            }

            return (-1).ToString();
        }
    }
}