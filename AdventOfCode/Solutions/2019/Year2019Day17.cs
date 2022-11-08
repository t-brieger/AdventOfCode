using System.Collections.Generic;
using AdventOfCode.Solutions.IntCode;

namespace AdventOfCode.Solutions;

public class Year2019Day17 : Solution
{
    public override string Part1(string input)
    {
        Computer camera = new Computer(input);

        HashSet<(int, int)> gridPoints = new HashSet<(int, int)>();
        camera.RunUntilHalted();
        int x = 0, y = 0;
        while (camera.output.Count != 0)
        {
            char pixel = (char) camera.output.Dequeue();
            if (pixel is '#' or '^' or '>' or 'v' or '<')
            {
                gridPoints.Add((x, y));
            }


            if (pixel == '\n')
            {
                x = 0;
                y++;
                continue;
            }

            x++;
        }

        int alignmentSum = 0;
        
        // inefficient but it doesn't matter much; there are only around 3000 pixels in total in my input (including those that aren't on the grid)
        foreach ((int gridX, int gridY) in gridPoints)
        {
            if (gridPoints.Contains((gridX + 1, gridY)) && gridPoints.Contains((gridX - 1, gridY)) &&
                gridPoints.Contains((gridX, gridY + 1)) && gridPoints.Contains((gridX, gridY - 1)))
            {
                alignmentSum += gridX * gridY;
            }
        }

        return alignmentSum.ToString();
    }

    public override string Part2(string input)
    {
        Computer vacuum = new Computer(input);
        vacuum.SetMemoryAt(0, 2);
        
        //TODO what the fuck
        
        return null;
    }
}
