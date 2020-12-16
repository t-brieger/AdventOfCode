using System;
using System.Linq;

namespace AdventOfCode.Solutions
{
    public class Year2020Day01 : Solution
    {
        public override string Part1(string input)
        {
            int[] nums = input.Split('\n', StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse).ToArray();
            for (int i = 0; i < nums.Length; i++)
            {
                for (int j = i + 1; j < nums.Length; j++)
                {
                    if (nums[i] + nums[j] == 2020)
                    {
                        return (nums[i] * nums[j]).ToString();
                    }
                }
            }

            return "No Solution found?";
        }

        public override string Part2(string input)
        {
            int[] nums = input.Split('\n', StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse).ToArray();
            for (int i = 0; i < nums.Length; i++)
            {
                for (int j = i + 1; j < nums.Length; j++)
                {
                    for (int k = j + 1; k < nums.Length; k++)
                    {
                        if (nums[i] + nums[j] + nums[k] == 2020)
                        {
                            return (nums[i] * nums[j] * nums[k]).ToString();
                        }
                    }
                }
            }

            return null;
        }
    }
}
