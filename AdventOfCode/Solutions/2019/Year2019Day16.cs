using System.Linq;

namespace AdventOfCode.Solutions;

public class Year2019Day16 : Solution
{
    public override string Part1(string input)
    {        
        int[] multipliers = { 0, 1, 0, -1 };
        
        int[] list = input.ToCharArray().Select(c => c - '0').ToArray();
        
        for (int i = 0; i < 100; i++)
        {
            int[] newList = new int[list.Length];
            for (int j = 0; j < list.Length; j++)
            {
                int sum = 0;
                for (int k = 0; k < list.Length; k++)
                {
                    int mult = multipliers[((k + 1) / (j + 1)) % multipliers.Length];
                    sum += list[k] * mult;
                }

                newList[j] = (sum > 0 ? sum : -sum) % 10;
            }

            list = newList;
        }
        
        return new string(list.Take(8).Select(i => (char)(i + '0')).ToArray());
    }

    public override string Part2(string input)
    {
        return null;
    }
}