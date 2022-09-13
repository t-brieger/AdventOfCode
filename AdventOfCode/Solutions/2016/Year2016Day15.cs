using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions;

public class Year2016Day15 : Solution
{
    public override string Part1(string input)
    {
        (int num, int posCount, int initial)[] disks = input.Split('\n')
            .Select(l =>
                l.Replace("#", "")
                    .Replace(".", "")
                    .Split(' '))
            .Select(l => (l[1][0] - '1', int.Parse(l[3]), int.Parse(l[11])))
            .ToArray();

        for (int i = 0;; i++)
        {
            bool allMatch = true;
            for (int j = 0; j < disks.Length; j++)
            {
                (int num, int posCount, int initial) = disks[j];

                int timeToContact = i + num + 1;
                int posAtTime = (initial + timeToContact) % posCount;

                if (posAtTime != 0)
                {
                    allMatch = false;
                    break;
                }
            }

            if (allMatch)
                return i.ToString();
        }
    }

    public override string Part2(string input)
    {
        input += "\na 7 b 11 c d e f g h i 0";
        
        (int num, int posCount, int initial)[] disks = input.Split('\n')
            .Select(l =>
                l.Replace("#", "")
                    .Replace(".", "")
                    .Split(' '))
            .Select(l => (l[1][0] - '1', int.Parse(l[3]), int.Parse(l[11])))
            .ToArray();

        for (long i = 0;; i++)
        {
            bool allMatch = true;
            for (int j = 0; j < disks.Length; j++)
            {
                (int num, int posCount, int initial) = disks[j];

                long timeToContact = i + num + 1;
                long posAtTime = (initial + timeToContact) % posCount;

                if (posAtTime != 0)
                {
                    allMatch = false;
                    break;
                }
            }

            if (allMatch)
                return i.ToString();
        }
    }
}
