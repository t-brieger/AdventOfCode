using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode.Solutions._2015
{
    class Year2015Day03 : Solution
    {
        public override string Part1(string input)
        {
            int posX = 0;
            int posY = 0;

            HashSet<(int x, int y)> visited = new HashSet<(int x, int y)>();

            visited.Add((0, 0));

            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] == '<')
                    posX--;
                else if (input[i] == '>')
                    posX++;
                else if (input[i] == 'v')
                    posY++;
                else if (input[i] == '^')
                    posY--;

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

            HashSet<(int x, int y)> visited = new HashSet<(int x, int y)>();

            visited.Add((0, 0));

            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] == '<')
                    if (isRobosTurn)
                        posX1--;
                    else
                        posX--;
                else if (input[i] == '>')
                    if (isRobosTurn)
                        posX1++;
                    else
                        posX++;
                else if (input[i] == 'v')
                    if (isRobosTurn)
                        posY1++;
                    else
                        posY++;
                else if (input[i] == '^')
                    if (isRobosTurn)
                        posY1--;
                    else
                        posY--;

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
