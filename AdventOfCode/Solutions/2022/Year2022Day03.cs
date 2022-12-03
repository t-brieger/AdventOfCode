namespace AdventOfCode.Solutions;

public class Year2022Day03 : Solution
{
    public override string Part1(string input)
    {
        int sum = 0;
        foreach (string rucksack in input.Split('\n'))
        {
            string s1 = rucksack.Substring(0, rucksack.Length / 2);
            string s2 = rucksack.Substring(rucksack.Length / 2, rucksack.Length / 2);
            foreach (char c1 in s1)
                if (s2.Contains(c1))
                {
                    sum += (c1 is >= 'a' and <= 'z' ? (c1 - 'a' + 1) : (c1 - 'A' + 27));
                    break;
                }
        }

        return sum.ToString();
    }

    public override string Part2(string input)
    {
        int sum = 0;
        string[] lines = input.Split('\n');
        for (int i = 0; i < lines.Length; i += 3)
        {
            string r1 = lines[i];
            string r2 = lines[i + 1];
            string r3 = lines[i + 2];
            foreach (char c1 in r1)
                if (r2.Contains(c1) && r3.Contains(c1))
                {
                    sum += (c1 is >= 'a' and <= 'z' ? (c1 - 'a' + 1) : (c1 - 'A' + 27));
                    break;
                }
        }

        return sum.ToString();
    }
}