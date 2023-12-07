using System;
using System.Linq;

namespace AdventOfCode.Solutions;

public class Year2023Day06 : Solution
{
    public override string Part1(string input)
    {
        (string timeRow, string distanceRow) = input.Split('\n');
        int[] times = timeRow.Split(' ', StringSplitOptions.RemoveEmptyEntries)[1..].Select(int.Parse).ToArray();
        int[] distances = distanceRow.Split(' ', StringSplitOptions.RemoveEmptyEntries)[1..].Select(int.Parse)
            .ToArray();

        int product = 1;
        for (int i = 0; i < times.Length; i++)
        {
            int validWays = 0;
            for (int heldTime = 0; heldTime < times[i]; heldTime++)
            {
                if (heldTime * (times[i] - heldTime) > distances[i])
                    validWays++;
            }

            product *= validWays;
        }

        return product.ToString();
    }

    public override string Part2(string input)
    {
        (string timeRow, string distanceRow) = input.Split('\n');
        long time = long.Parse(timeRow["Time:".Length..].Replace(" ", ""));
        long distance = long.Parse(distanceRow["Distance:".Length..].Replace(" ", ""));

        int validWays = 0;
        for (int heldTime = 0; heldTime < time; heldTime++)
        {
            if (heldTime * (time - heldTime) > distance)
                validWays++;
        }


        return validWays.ToString();
    }
}
