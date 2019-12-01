using System;

namespace AdventOfCode.Solutions._2019
{
    public class Year2019Day01 : Solution
    {
        public override string Part1(string input)
        {
            int sum = 0;
            foreach (string line in input.Split(new[] {'\r', '\n'}, StringSplitOptions.RemoveEmptyEntries))
            {
                sum += int.Parse(line) / 3 - 2;
            }
            return sum.ToString();
        }

        public override string Part2(string input)
        {
            int sum = 0;
            foreach (string line in input.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries))
            {
                int tmpSum = int.Parse(line) / 3 - 2;
                sum += tmpSum;
                while (true)
                {
                    tmpSum = tmpSum / 3 - 2;
                    if (tmpSum > 0)
                        sum += tmpSum;
                    else
                        break;
                }
            }
            return sum.ToString();
        }
    }
}