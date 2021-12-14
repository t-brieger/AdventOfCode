namespace AdventOfCode.Solutions;

public class Year2015Day20 : Solution
{
    public override string Part1(string input)
    {
        int[] houses = new int[int.Parse(input) / 10];
        for (int i = 1; i < houses.Length; i++)
        {
            for (int j = i; j < houses.Length; j += i)
            {
                houses[j] += i;
            }
        }
            
        for (int i = 0; i < houses.Length; i++)
            if (houses[i] >= houses.Length)
                return i.ToString();
        return null;
    }

    public override string Part2(string input)
    {
        int[] houses = new int[int.Parse(input)];
        for (int i = 1; i < houses.Length; i++)
        {
            for (int j = 1; j <= 50; j++)
            {
                if (j * i >= houses.Length)
                    break;
                houses[j * i] += i * 11;
            }
        }
            
        for (int i = 0; i < houses.Length; i++)
            if (houses[i] >= houses.Length)
                return i.ToString();
        return null;
    }
}