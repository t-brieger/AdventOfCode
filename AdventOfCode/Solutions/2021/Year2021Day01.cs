using System.Linq;

namespace AdventOfCode.Solutions
{
    public class Year2021Day01 : Solution
    {
        public override string Part1(string input)
        {
            int last = int.MaxValue;
            int increases = 0;
            foreach (int i in input.Split('\n').Select(int.Parse))
            {
                if (i > last)
                    increases++;
                last = i;
            }

            return increases.ToString();
        }

        public override string Part2(string input)
        {
            int[] measurements = input.Split('\n').Select(int.Parse).ToArray();

            int increases = 0;
            for (int i = 0; i < measurements.Length - 3; i++)
            {
                int windowASum = measurements[i] + measurements[i + 1] + measurements[i + 2];
                int windowBSum = measurements[i + 1] + measurements[i + 2] + measurements[i + 3];
                if (windowBSum > windowASum)
                    increases++;
            }

            return increases.ToString();
        }
    }
}