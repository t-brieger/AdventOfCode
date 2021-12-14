namespace AdventOfCode.Solutions;

public class Year2017Day09 : Solution
{
    public override string Part1(string input)
    {
        int totalScore = 0;
        int nestingLevel = 0;

        bool nextEscaped = false;
        bool isInGarbage = false;

        foreach (char c in input)
        {
            if (nextEscaped)
            {
                nextEscaped = false;
                continue;
            }

            if (c == '!')
                nextEscaped = true;

            if (isInGarbage)
                if (c == '>')
                    isInGarbage = false;
                else
                    continue;
            switch (c)
            {
                case '<':
                    isInGarbage = true;
                    break;
                case '{':
                    nestingLevel++;
                    break;
                case '}':
                    totalScore += nestingLevel--;
                    break;
            }
        }

        return totalScore.ToString();
    }

    public override string Part2(string input)
    {
        int trashChars = 0;

        bool nextEscaped = false;
        bool isInGarbage = false;

        foreach (char c in input)
        {
            if (nextEscaped)
            {
                nextEscaped = false;
                continue;
            }

            if (c == '!')
                nextEscaped = true;

            else if (isInGarbage)
                if (c == '>')
                {
                    isInGarbage = false;
                }
                else
                {
                    trashChars++;
                    continue;
                }

            switch (c)
            {
                case '<':
                    isInGarbage = true;
                    break;
                case '{':
                    break;
                case '}':
                    break;
            }
        }

        return trashChars.ToString();
    }
}