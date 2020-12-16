using System;
using System.Linq;

namespace AdventOfCode.Solutions
{
    public class Year2020Day10 : Solution
    {
        public override string Part1(string input)
        {
            int[] adapters = input.Split('\n', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse)
                .OrderBy(i => i).Prepend(0).ToArray();

            //last adapter -> device
            int threeDiff = 1;
            int oneDiff = 0;

            for (int i = 1; i < adapters.Length; i++)
            {
                switch (adapters[i] - adapters[i - 1])
                {
                    case 1:
                        oneDiff++;
                        break;
                    case 3:
                        threeDiff++;
                        break;
                    case 2:
                        break;
                }
            }

            return (threeDiff * oneDiff).ToString();
        }

        public override string Part2(string input)
        {
            int[] adapters = input.Split('\n', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse)
                .OrderBy(i => i).ToArray();

            long[] cumulativeCombinations = new long[adapters[^1] + 1];
            cumulativeCombinations[0] = 1;
            cumulativeCombinations[1] = 1;
            cumulativeCombinations[2] = 2;
            
            for (int i = 0; i < adapters.Length; i++)
            {
                int j = adapters[i];

                long cum = cumulativeCombinations[j - 1];
                if (j >= 2)
                    cum += cumulativeCombinations[j - 2];
                if (j >= 3)
                    cum += cumulativeCombinations[j - 3];

                cumulativeCombinations[j] = cum;
            }

            return cumulativeCombinations[^1].ToString();
        }
    }
}
