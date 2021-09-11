using System.Collections.Generic;

namespace AdventOfCode.Solutions
{
    class Year2015Day03 : Solution
    {
        public override string Part1(string input)
        {
            int posX = 0;
            int posY = 0;

            HashSet<(int x, int y)> visited = new() { (0, 0) };


            foreach (char direction in input)
            {
                switch (direction)
                {
                    case '<':
                        posX--;
                        break;
                    case '>':
                        posX++;
                        break;
                    case 'v':
                        posY++;
                        break;
                    case '^':
                        posY--;
                        break;
                }

                if (!visited.Contains((posX, posY)))
                    visited.Add((posX, posY));
            }

            return visited.Count.ToString();
        }

        public override string Part2(string input)
        {
            int posX = 0;
            int posY = 0;

            int posX1 = 0;
            int posY1 = 0;

            bool isRobosTurn = false;

            HashSet<(int x, int y)> visited = new() { (0, 0) };


            foreach (char direction in input)
            {
                switch (direction)
                {
                    case '<' when isRobosTurn:
                        posX1--;
                        break;
                    case '<':
                        posX--;
                        break;
                    case '>' when isRobosTurn:
                        posX1++;
                        break;
                    case '>':
                        posX++;
                        break;
                    case 'v' when isRobosTurn:
                        posY1++;
                        break;
                    case 'v':
                        posY++;
                        break;
                    case '^' when isRobosTurn:
                        posY1--;
                        break;
                    case '^':
                        posY--;
                        break;
                }

                if (isRobosTurn)
                {
                    if (!visited.Contains((posX1, posY1)))
                        visited.Add((posX1, posY1));
                }
                else
                {
                    if (!visited.Contains((posX, posY)))
                        visited.Add((posX, posY));
                }

                isRobosTurn = !isRobosTurn;
            }

            return visited.Count.ToString();
        }
    }
}