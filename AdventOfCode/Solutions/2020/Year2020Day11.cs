using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace AdventOfCode.Solutions
{
    public class Year2020Day11 : Solution
    {
        public override string Part1(string input)
        {
            char[][] seats = input.Split('\n', StringSplitOptions.RemoveEmptyEntries)
                .Select(line => line.ToCharArray()).ToArray();

            bool hasChanged = true;
            while (hasChanged)
            {
                hasChanged = false;
                char[][] newSeats = new char[seats.Length][];
                for (int y = 0; y < seats.Length; y++)
                {
                    newSeats[y] = new char[seats[y].Length];
                    for (int x = 0; x < seats[y].Length; x++)
                    {
                        int adjacent = 0;
                        if (y != 0)
                            adjacent += seats[y - 1][x] == '#' ? 1 : 0;
                        if (y != 0 && x != seats[y].Length - 1)
                            adjacent += seats[y - 1][x + 1] == '#' ? 1 : 0;
                        if (x != seats[y].Length - 1)
                            adjacent += seats[y][x + 1] == '#' ? 1 : 0;
                        if (x != seats[y].Length - 1 && y != seats.Length - 1)
                            adjacent += seats[y + 1][x + 1] == '#' ? 1 : 0;
                        if (y != seats.Length - 1)
                            adjacent += seats[y + 1][x] == '#' ? 1 : 0;
                        if (y != seats.Length - 1 && x != 0)
                            adjacent += seats[y + 1][x - 1] == '#' ? 1 : 0;
                        if (x != 0)
                            adjacent += seats[y][x - 1] == '#' ? 1 : 0;
                        if (y != 0 && x != 0)
                            adjacent += seats[y - 1][x - 1] == '#' ? 1 : 0;

                        switch (seats[y][x])
                        {
                            case 'L' when adjacent == 0:
                                hasChanged = true;
                                newSeats[y][x] = '#';
                                break;
                            case '#' when adjacent >= 4:
                                hasChanged = true;
                                newSeats[y][x] = 'L';
                                break;
                            case 'L':
                            case '#':
                                newSeats[y][x] = seats[y][x];
                                break;
                        }
                    }
                }

                seats = newSeats;
            }

            return seats.SelectMany(row => row).Count(col => col == '#').ToString();
        }

        [SuppressMessage("ReSharper", "EmptyGeneralCatchClause")]
        public override string Part2(string input)
        {
            char[][] seats = input.Split('\n', StringSplitOptions.RemoveEmptyEntries)
                .Select(line => line.ToCharArray()).ToArray();

            bool hasChanged = true;
            while (hasChanged)
            {
                hasChanged = false;
                char[][] newSeats = new char[seats.Length][];
                for (int y = 0; y < seats.Length; y++)
                {
                    newSeats[y] = new char[seats[y].Length];
                    for (int x = 0; x < seats[y].Length; x++)
                    {
                        int adjacent = 0;

                        //north
                        int posY = y - 1, posX = x;
                        while (posY > 0 && seats[posY][posX] == '.') posY--;

                        try
                        {
                            adjacent += seats[posY][posX] == '#' ? 1 : 0;
                        }
                        catch (Exception)
                        {
                        }

                        //NE
                        posY = y - 1;
                        posX = x + 1;
                        while (posY > 0 && posX < seats[posY].Length - 1 && seats[posY][posX] == '.')
                        {
                            posY--;
                            posX++;
                        }

                        try
                        {
                            adjacent += seats[posY][posX] == '#' ? 1 : 0;
                        }
                        catch (Exception)
                        {
                        }

                        //E
                        posY = y;
                        posX = x + 1;
                        while (posX < seats[posY].Length - 1 && seats[posY][posX] == '.') posX++;

                        try
                        {
                            adjacent += seats[posY][posX] == '#' ? 1 : 0;
                        }
                        catch (Exception)
                        {
                        }

                        //SE
                        posY = y + 1;
                        posX = x + 1;
                        while (posY < seats.Length - 1 && posX < seats[posY].Length - 1 && seats[posY][posX] == '.')
                        {
                            posY++;
                            posX++;
                        }

                        try
                        {
                            adjacent += seats[posY][posX] == '#' ? 1 : 0;
                        }
                        catch (Exception)
                        {
                        }

                        //S
                        posY = y + 1;
                        posX = x;
                        while (posY < seats.Length - 1 && seats[posY][posX] == '.') posY++;

                        try
                        {
                            adjacent += seats[posY][posX] == '#' ? 1 : 0;
                        }
                        catch (Exception)
                        {
                        }

                        //SW
                        posY = y + 1;
                        posX = x - 1;
                        while (posY < seats.Length - 1 && posX > 0 && seats[posY][posX] == '.')
                        {
                            posY++;
                            posX--;
                        }

                        try
                        {
                            adjacent += seats[posY][posX] == '#' ? 1 : 0;
                        }
                        catch (Exception)
                        {
                        }

                        //W
                        posY = y;
                        posX = x - 1;
                        while (posX > 0 && seats[posY][posX] == '.') posX--;

                        try
                        {
                            adjacent += seats[posY][posX] == '#' ? 1 : 0;
                        }
                        catch (Exception)
                        {
                        }

                        //NW
                        posY = y - 1;
                        posX = x - 1;
                        while (posY > 0 && posX > 0 && seats[posY][posX] == '.')
                        {
                            posY--;
                            posX--;
                        }

                        try
                        {
                            adjacent += seats[posY][posX] == '#' ? 1 : 0;
                        }
                        catch (Exception)
                        {
                        }


                        switch (seats[y][x])
                        {
                            case 'L' when adjacent == 0:
                                hasChanged = true;
                                newSeats[y][x] = '#';
                                break;
                            case '#' when adjacent >= 5:
                                hasChanged = true;
                                newSeats[y][x] = 'L';
                                break;
                            case 'L':
                            case '#':
                                newSeats[y][x] = seats[y][x];
                                break;
                            default:
                                newSeats[y][x] = '.';
                                break;
                        }
                    }
                }

                seats = newSeats;
            }

            return seats.SelectMany(row => row).Count(col => col == '#').ToString();
        }
    }
}