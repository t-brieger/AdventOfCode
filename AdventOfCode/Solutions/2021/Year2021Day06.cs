using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions
{
    public class Year2021Day06 : Solution
    {
        public override string Part1(string input)
        {
            Dictionary<int, int> fishTimers = new Dictionary<int, int>();

            for (int i = 0; i <= 8; i++)
            {
                fishTimers[i] = 0;
            }

            foreach (int i in input.Split(',').Select(int.Parse))
                fishTimers[i]++;

            for (int i = 0; i < 80; i++)
            {
                Dictionary<int, int> newTimers = new Dictionary<int, int>
                {
                    [8] = fishTimers[0],
                    [7] = fishTimers[8],
                    [6] = fishTimers[7] + fishTimers[0],
                    [5] = fishTimers[6],
                    [4] = fishTimers[5],
                    [3] = fishTimers[4],
                    [2] = fishTimers[3],
                    [1] = fishTimers[2],
                    [0] = fishTimers[1]
                };
                fishTimers = newTimers;
            }

            return fishTimers.Sum(kvp => kvp.Value).ToString();
        }

        public override string Part2(string input)
        {
            Dictionary<int, long> fishTimers = new Dictionary<int, long>();

            for (int i = 0; i <= 8; i++)
            {
                fishTimers[i] = 0;
            }

            foreach (int i in input.Split(',').Select(int.Parse))
                fishTimers[i]++;

            for (int i = 0; i < 256; i++)
            {
                Dictionary<int, long> newTimers = new Dictionary<int, long>
                {
                    [8] = fishTimers[0],
                    [7] = fishTimers[8],
                    [6] = fishTimers[7] + fishTimers[0],
                    [5] = fishTimers[6],
                    [4] = fishTimers[5],
                    [3] = fishTimers[4],
                    [2] = fishTimers[3],
                    [1] = fishTimers[2],
                    [0] = fishTimers[1]
                };
                fishTimers = newTimers;
            }

            return fishTimers.Sum(kvp => kvp.Value).ToString();
        }
    }
}