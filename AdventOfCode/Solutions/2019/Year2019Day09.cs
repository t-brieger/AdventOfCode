using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Solutions.IntCode;

namespace AdventOfCode.Solutions;

public class Year2019Day09 : Solution
{
    public override string Part1(string input)
    {
        int[] nums = input.Split(',').Select(int.Parse).ToArray();
        Dictionary<int, int> program = new();
        for (int i = 0; i < nums.Length; i++)
            program.Add(i, nums[i]);

        Computer c = new(program);
        c.EnqueueInput(1);
        c.RunUntilHalted();
        return c.output.Dequeue().ToString();
    }

    public override string Part2(string input)
    {
        int[] nums = input.Split(',').Select(int.Parse).ToArray();
        Dictionary<int, int> program = new();
        for (int i = 0; i < nums.Length; i++)
            program.Add(i, nums[i]);

        Computer c = new(program);
        c.EnqueueInput(2);
        c.RunUntilHalted();
        return c.output.Dequeue().ToString();
    }
}