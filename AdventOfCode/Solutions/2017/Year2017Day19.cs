using System;

namespace AdventOfCode.Solutions
{
    //Please dont touch this - it can and will break
    public class Year2017Day19 : Solution
    {
        public override string Part1(string input)
        {
            bool[,] grid = new bool[input.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)[0].Length, input.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries).Length];
            char[,] extraChars = new char[grid.GetLength(0), grid.GetLength(1)];

            for (int i = 0; i < input.Split(new[] {'\n', '\r'}, StringSplitOptions.RemoveEmptyEntries).Length; i++)
            {
                string line = input.Split(new[] {'\n', '\r'}, StringSplitOptions.RemoveEmptyEntries)[i];

                for (int j = 0; j < line.Length; j++)
                {
                    char c = line[j];

                    grid[j, i] = !Char.IsWhiteSpace(c);
                    if (c >= 'A' && c <= 'Z')
                        extraChars[j, i] = c;
                }
            }

            int positionX = 0;
            int positionY = 0;

            for (int i = 0; i < grid.GetLength(0); i++)
                if (grid[i, 0])
                    positionX = i;

            string chars = "";

            byte direction = 1; //0 - RIGHT, 1 - DOWN, 2 - LEFT, 3 - UP

            while (true)
            {
                if (extraChars[positionX, positionY] != '\0')
                    chars += extraChars[positionX, positionY];

                do
                {
                    if (direction != 0)
                        if (positionX - 1 >= 0 && grid[positionX - 1, positionY])
                            break;
                    if (direction != 2)
                        if (positionX + 1 < grid.GetLength(0) && grid[positionX + 1, positionY])
                            break;
                    if (direction != 1)
                        if (positionY - 1 >= 0 && grid[positionX, positionY - 1])
                            break;
                    if (direction == 3) return chars;
                    if (positionY + 1 < grid.GetLength(1) && grid[positionX, positionY + 1])
                        break;
                    return chars;
#pragma warning disable 162
                } while (false);
#pragma warning restore 162

                switch (direction)
                {
                    case 0:
                        if (positionX < grid.GetLength(0) - 1 && grid[positionX + 1, positionY])
                        {
                            positionX += 1;
                            direction = 0;
                        }else if (positionY > 0 && grid[positionX, positionY - 1])
                        {
                            positionY -= 1;
                            direction = 3;
                        }else if (positionY < grid.GetLength(1) - 1 && grid[positionX, positionY + 1])
                        {
                            positionY += 1;
                            direction = 1;
                        }

                        break;
                    case 1:
                        if (positionY < grid.GetLength(1) - 1 && grid[positionX, positionY + 1])
                        {
                            positionY += 1;
                            direction = 1;
                        }else if (positionX > 0 && grid[positionX - 1, positionY])
                        {
                            positionX -= 1;
                            direction = 2;
                        }else if (positionX < grid.GetLength(0) - 1 && grid[positionX + 1, positionY])
                        {
                            positionX += 1;
                            direction = 0;
                        }
                        break;
                    case 2:
                        if (positionX > 0 && grid[positionX - 1, positionY])
                        {
                            positionX -= 1;
                            direction = 2;
                        }else if (positionY > 0 && grid[positionX, positionY - 1])
                        {
                            positionY -= 1;
                            direction = 3;
                        }else if (positionY < grid.GetLength(1) - 1 && grid[positionX, positionY + 1])
                        {
                            positionY += 1;
                            direction = 1;
                        }

                        break;
                    case 3:
                        if (positionY > 0 && grid[positionX, positionY - 1])
                        {
                            positionY -= 1;
                            direction = 3;
                        }else if (positionX < grid.GetLength(0) - 1 && grid[positionX + 1, positionY])
                        {
                            positionX += 1;
                            direction = 0;
                        }else if (positionX > 0 && grid[positionX - 1, positionY])
                        {
                            positionX -= 1;
                            direction = 2;
                        }

                        break;
                }
            }
        }

        public override string Part2(string input)
        {
            bool[,] grid = new bool[input.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)[0].Length, input.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries).Length];

            for (int i = 0; i < input.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries).Length; i++)
            {
                string line = input.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)[i];

                for (int j = 0; j < line.Length; j++)
                {
                    char c = line[j];

                    grid[j, i] = !Char.IsWhiteSpace(c);
                }
            }

            int positionX = 0;
            int positionY = 0;

            for (int i = 0; i < grid.GetLength(0); i++)
                if (grid[i, 0])
                    positionX = i;

            int steps = 1; //being on the first field apparently is a move already

            byte direction = 1; //0 - RIGHT, 1 - DOWN, 2 - LEFT, 3 - UP

            while (true)
            {

                do
                {
                    if (direction != 0)
                        if (positionX - 1 >= 0 && grid[positionX - 1, positionY])
                            break;
                    if (direction != 2)
                        if (positionX + 1 < grid.GetLength(0) && grid[positionX + 1, positionY])
                            break;
                    if (direction != 1)
                        if (positionY - 1 >= 0 && grid[positionX, positionY - 1])
                            break;
                    if (direction == 3) return steps.ToString();
                    if (positionY + 1 < grid.GetLength(1) && grid[positionX, positionY + 1])
                        break;
                    return steps.ToString();
#pragma warning disable 162
                } while (false);
#pragma warning restore 162

                switch (direction)
                {
                    case 0:
                        if (positionX < grid.GetLength(0) - 1 && grid[positionX + 1, positionY])
                        {
                            positionX += 1;
                            direction = 0;
                        }
                        else if (positionY > 0 && grid[positionX, positionY - 1])
                        {
                            positionY -= 1;
                            direction = 3;
                        }
                        else if (positionY < grid.GetLength(1) - 1 && grid[positionX, positionY + 1])
                        {
                            positionY += 1;
                            direction = 1;
                        }

                        break;
                    case 1:
                        if (positionY < grid.GetLength(1) - 1 && grid[positionX, positionY + 1])
                        {
                            positionY += 1;
                            direction = 1;
                        }
                        else if (positionX > 0 && grid[positionX - 1, positionY])
                        {
                            positionX -= 1;
                            direction = 2;
                        }
                        else if (positionX < grid.GetLength(0) - 1 && grid[positionX + 1, positionY])
                        {
                            positionX += 1;
                            direction = 0;
                        }
                        break;
                    case 2:
                        if (positionX > 0 && grid[positionX - 1, positionY])
                        {
                            positionX -= 1;
                            direction = 2;
                        }
                        else if (positionY > 0 && grid[positionX, positionY - 1])
                        {
                            positionY -= 1;
                            direction = 3;
                        }
                        else if (positionY < grid.GetLength(1) - 1 && grid[positionX, positionY + 1])
                        {
                            positionY += 1;
                            direction = 1;
                        }

                        break;
                    case 3:
                        if (positionY > 0 && grid[positionX, positionY - 1])
                        {
                            positionY -= 1;
                            direction = 3;
                        }
                        else if (positionX < grid.GetLength(0) - 1 && grid[positionX + 1, positionY])
                        {
                            positionX += 1;
                            direction = 0;
                        }
                        else if (positionX > 0 && grid[positionX - 1, positionY])
                        {
                            positionX -= 1;
                            direction = 2;
                        }

                        break;
                }

                steps++;
            }
        }
    }
}