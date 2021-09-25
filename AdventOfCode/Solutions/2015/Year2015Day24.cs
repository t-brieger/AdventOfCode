using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions
{
    public class Year2015Day24 : Solution
    {
        public override string Part1(string input)
        {
            HashSet<int> nums = new(input.Split('\n').Select(int.Parse));
            int weightPerPart = nums.Sum() / 3;

            for (int len = 1; len < nums.Count; len++)
            {
                foreach (int[] x in Util.GetPermutations(nums, len).Where(x => x.Sum() == weightPerPart))
                {
                    return x.Aggregate(1L, (y, z) => z * y).ToString();
                }
            }

            return "no solution found";
        }

        public override string Part2(string input)
        {
            HashSet<int> nums = new(input.Split('\n').Select(int.Parse));
            int weightPerPart = nums.Sum() / 4;
            
            for (int len = 1; len < nums.Count; len++)
            {
                foreach (int[] x in Util.GetPermutations(nums, len).Where(x => x.Sum() == weightPerPart))
                {
                    // apparently "exceptWith" (set subtraction) always modifies the set it's called on, so we have to
                    // make a new one
                    HashSet<int> subList = new(nums);
                    subList.ExceptWith(x);
                    for (int lenSub = 1; lenSub < subList.Count; lenSub++)
                    {
                        if (Util.GetPermutations(subList, lenSub).Any(y => y.Sum() == weightPerPart))
                        {
                            return x.Aggregate(1L, (y, z) => z * y).ToString();
                        }
                    }
                }
            }

            return "no solution found";
        }
    }
}