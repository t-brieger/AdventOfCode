namespace AdventOfCode.Solutions;

public class Year2022Day02 : Solution
{
    public override string Part1(string input)
    {
        int score = 0;
        foreach (string line in input.Split('\n'))
        {
            if (line is "A Y" or "B Z" or "C X")
                score += 6;
            if (line is "A X" or "B Y" or "C Z")
                score += 3;

            if (line.Contains('X'))
                score += 1;
            if (line.Contains('Y'))
                score += 2;
            if (line.Contains('Z'))
                score += 3;
        }
        return score.ToString();
    }

    public override string Part2(string input)
    {
        int score = 0;
        foreach (string line in input.Split('\n'))
        {
            if (line.Contains('X'))
            {
                score += line[0] == 'A' ? 3 : line[0] == 'B' ? 1 : 2;
            }
            if (line.Contains('Y'))
            {
                score += line[0] == 'A' ? 4 : line[0] == 'B' ? 5 : 6;
            }
            if (line.Contains('Z'))
            {
                score += line[0] == 'A' ? 8 : line[0] == 'B' ? 9 : 7;
            }
        }
        return score.ToString();
    }
}
