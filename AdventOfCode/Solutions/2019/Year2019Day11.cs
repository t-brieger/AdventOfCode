using System.Collections.Generic;
using System.Linq;
using System.Text;
using AdventOfCode.Solutions.IntCode;
using AngleSharp.Common;

namespace AdventOfCode.Solutions
{
    public class Year2019Day11 : Solution
    {
        public override string Part1(string input)
        {
            HashSet<(int, int)> whitePanels = new HashSet<(int, int)>();
            HashSet<(int, int)> painted = new HashSet<(int, int)>();
            //0 = UP
            //1 = RIGHT
            //2 = DOWN
            //3 = LEFT
            byte facing = 0;
            int posX = 0;
            int posY = 0;
            Computer c = new Computer(input);

            while (true)
            {
                c.input.Enqueue(whitePanels.Contains((posX, posY)) ? 1 : 0);
                while (!c.hasHalted && c.output.Count != 2)
                    c.Step();
                if (c.hasHalted)
                    break;
                try
                {
                    if (c.output.Dequeue() == 0)
                        whitePanels.Remove((posX, posY));
                    else
                        whitePanels.Add((posX, posY));

                    painted.Add((posX, posY));
                }
                catch
                {
                    // ignored - we want the panel black/white regardless if it already is, which might throw
                }

                if (c.output.Dequeue() == 0)
                    facing--;
                else
                    facing++;
                facing %= 4;

                if (facing == 0)
                    posY--;
                else if (facing == 1)
                    posX++;
                else if (facing == 2)
                    posY++;
                else
                    posX--;
            }
            
            return painted.Count.ToString();
        }

        public override string Part2(string input)
        {
            HashSet<(int, int)> whitePanels = new HashSet<(int, int)>();
            whitePanels.Add((0, 0));
            //0 = UP
            //1 = RIGHT
            //2 = DOWN
            //3 = LEFT
            byte facing = 0;
            int posX = 0;
            int posY = 0;
            Computer c = new Computer(input);

            while (true)
            {
                c.input.Enqueue(whitePanels.Contains((posX, posY)) ? 1 : 0);
                while (!c.hasHalted && c.output.Count != 2)
                    c.Step();
                if (c.hasHalted)
                    break;
                try
                {
                    if (c.output.Dequeue() == 0)
                        whitePanels.Remove((posX, posY));
                    else
                        whitePanels.Add((posX, posY));
                }
                catch
                {
                    // ignored - we want the panel black/white regardless if it already is, which might throw
                }

                if (c.output.Dequeue() == 0)
                    facing--;
                else
                    facing++;
                facing %= 4;

                if (facing == 0)
                    posY--;
                else if (facing == 1)
                    posX++;
                else if (facing == 2)
                    posY++;
                else
                    posX--;
            }

            int minX = whitePanels.Min(x => x.Item1);
            int minY = whitePanels.Min(x => x.Item2);
            int maxX = whitePanels.Max(x => x.Item1);
            int maxY = whitePanels.Max(x => x.Item2);

            StringBuilder sb = new StringBuilder();
            sb.Append("\n");
            for (int y = minY; y <= maxY; y++)
            {
                for (int x = minX; x <= maxX; x++)
                {
                    sb.Append(whitePanels.Contains((x, y)) ? '#' : ' ');
                }

                sb.Append('\n');
            }

            return sb.ToString();
        }
    }
}