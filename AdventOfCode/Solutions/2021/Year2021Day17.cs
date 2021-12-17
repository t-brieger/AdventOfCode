using System;
using System.Linq;

namespace AdventOfCode.Solutions;

public class Year2021Day17 : Solution
{
    public override string Part1(string input)
    {
        int y = int.Parse(input["target area: x=".Length..].Split(", y=")[1].Split("..")[0]);

        // y is negative, so this works - really what we want though is just the (-y - 1)th triangle number, so for
        // comprehension it might be better to do:
        // (-y * (-y + 1)) / 2
        // also this only works if the minimum y value is negative.
        return (y * (y + 1) / 2).ToString();
    }

    public override string Part2(string input)
    {
        int[] numbers = input["target area: x=".Length..]
            .Split(new[] {'.', ',', ' ', 'y', '='}, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();

        int maxHeight = -numbers[2] - 1;
        int validCount = 0;

        for (int y = numbers[2]; y <= maxHeight; y++)
        {
            for (int x = 0; x <= numbers[1]; x++)
            {
                // even after our x velocity reaches 0, we won't be past the start of the target
                if (x * (x + 1) / 2 < numbers[0])
                    continue;
                
                int xPos = 0;
                int yPos = 0;
                int xVel = x;
                int yVel = y;
                
                while ((xVel != 0 || (xPos >= numbers[0] && xPos <= numbers[1])) && yPos >= numbers[2])
                {
                    xPos += xVel;
                    yPos += yVel;
                    xVel -= Math.Sign(xVel);
                    yVel -= 1;

                    if (xPos < numbers[0] || xPos > numbers[1] || yPos < numbers[2] || yPos > numbers[3]) continue;
                    validCount++;
                    break;
                }
            }
        }


        return validCount.ToString();
    }
}