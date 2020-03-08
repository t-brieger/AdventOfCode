using System;
using System.Linq;

namespace AdventOfCode.Solutions._2019
{
    public class Year2019Day01 : Solution
    {
        public override string Part1(string input)
        {
            int sum = input.Split(new[] {'\r', '\n'}, StringSplitOptions.RemoveEmptyEntries).Sum(line => Int32.Parse(line) / 3 - 2);
            return sum.ToString();
        }

        public override string Part2(string input)
        {
            int sum = 0;
            foreach (int tmpSum in input.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).Select(line => Int32.Parse(line) / 3 - 2))
            {
                sum += tmpSum;
                while (true)
                {
                    if (tmpSum / 3 - 2 > 0)
                        sum += tmpSum / 3 - 2;
                    else
                        break;
                }
            }
            return sum.ToString();
        }
    }
}