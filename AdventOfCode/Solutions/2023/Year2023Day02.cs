using System;

namespace AdventOfCode.Solutions;

public class Year2023Day02 : Solution
{
    public override string Part1(string input)
    {
        string[] lines = input.Split('\n');

        int idsum = 0;
        
        foreach (string line in lines)
        {
            int id = int.Parse(line.Split(':')[0].Split(' ')[1]);
            string[] reveals = line.Split(':', 2)[1].Split(';');
            bool possible = true;
            foreach (string reveal in reveals)
            {
                string[] individual = reveal.Split(", ");
                foreach (string indiv in individual)
                {
                    string[] split = indiv.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    int c = int.Parse(split[0]);
                    switch (split[1])
                    {
                        case "red":
                            if (c > 12)
                                possible = false;
                            break;
                        case "blue":
                            if (c > 14)
                                possible = false;
                            break;
                        case "green":
                            if (c > 13)
                                possible = false;
                            break;
                    }
                }
            }

            if (possible)
                idsum += id;
        }

        return idsum.ToString();
    }

    public override string Part2(string input)
    {
        string[] lines = input.Split('\n');

        int sum = 0;
        
        foreach (string line in lines)
        {
            int id = int.Parse(line.Split(':')[0].Split(' ')[1]);
            string[] reveals = line.Split(':', 2)[1].Split(';');
            int r = 0, g = 0, b = 0;
            foreach (string reveal in reveals)
            {
                string[] individual = reveal.Split(", ");
                foreach (string indiv in individual)
                {
                    string[] split = indiv.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    int c = int.Parse(split[0]);
                    switch (split[1])
                    {
                        case "red":
                            r = Math.Max(c, r);
                            break;
                        case "blue":
                            b = Math.Max(c, b);
                            break;
                        case "green":
                            g = Math.Max(c, g);
                            break;
                    }
                }
            }

            sum += r * g * b;
        }

        return sum.ToString();
    }
}
