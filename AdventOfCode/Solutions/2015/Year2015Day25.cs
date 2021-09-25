using System.Linq;

namespace AdventOfCode.Solutions
{
    public class Year2015Day25 : Solution
    {
        public override string Part1(string input)
        {
            input = input["To continue, please consult the code grid in the manual.  Enter the code at row ".Length..];
            input = input[..^1];
            int[] numbers = input.Split(", column ").Select(int.Parse).ToArray();
            int codeIx = Enumerable.Range(0, numbers[0] + numbers[1] - 1).Sum() + numbers[1];
            long currentCode = 20151125;
            for (int i = 0; i < codeIx - 1; i++)
            {
                currentCode *= 252533;
                currentCode %= 33554393;
            }

            return currentCode.ToString();
        }

        public override string Part2(string input)
        {
            return "";
        }
    }
}