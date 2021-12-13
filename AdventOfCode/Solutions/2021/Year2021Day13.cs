using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode.Solutions;

public class Year2021Day13 : Solution
{
    public override string Part1(string input)
    {
        string[] splitInput = input.Split("\n\n");
        HashSet<(int, int)> points = splitInput[0].Split('\n').Select(l => l.Split(',').Select(int.Parse).ToArray())
            .Select(a => (a[0], a[1])).ToHashSet();

        string firstFold = splitInput[1].Split('\n')[0][11..];
        (char, int) parsedFold = (firstFold[0], int.Parse(firstFold[2..]));

        HashSet<(int, int)> newPoints = new();
        foreach ((int x, int y) in points)
        {
            if (parsedFold.Item1 == 'x')
            {
                int i = parsedFold.Item2 - x;
                newPoints.Add((i < 0 ? parsedFold.Item2 + i : x, y));
            }

            if (parsedFold.Item1 == 'y')
            {
                int i = parsedFold.Item2 - y;
                newPoints.Add((x, i < 0 ? parsedFold.Item2 + i : y));
            }
        }

        points = newPoints;

        return newPoints.Count.ToString();
    }

    public override string Part2(string input)
    {
        string[] splitInput = input.Split("\n\n");
        HashSet<(int, int)> points = splitInput[0].Split('\n').Select(l => l.Split(',').Select(int.Parse).ToArray())
            .Select(a => (a[0], a[1])).ToHashSet();

        (char, int)[] folds = splitInput[1].Split('\n').Select(l => l[11..]).Select(l => (l[0], int.Parse(l[2..])))
            .ToArray();
        
        foreach ((char, int) fold in folds)
        {
            HashSet<(int, int)> newPoints = new();
            foreach ((int x, int y) in points)
            {
                if (fold.Item1 == 'x')
                {
                    int i = fold.Item2 - x;
                    newPoints.Add((i < 0 ? fold.Item2 + i : x, y));
                }
                else
                {
                    int i = fold.Item2 - y;
                    newPoints.Add((x, i < 0 ? fold.Item2 + i : y));
                }
            }

            points = newPoints;
        }

        StringBuilder sb = new();
        sb.Append('\n');
        for (int y = points.Min(p => p.Item2); y <= points.Max(p => p.Item2); y++)
        {
            for (int x = points.Min(p => p.Item1); x <= points.Max(p => p.Item1); x++)
            {
                sb.Append(points.Contains((x, y)) ? '#' : ' ');
            }

            sb.Append('\n');
        }
        
        return sb.ToString();
    }
}