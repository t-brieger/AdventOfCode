using System;

namespace AdventOfCode.Solutions
{
    public class Year2018Day11 : Solution
    {
        public static int BuildSaTable(int[,] a, int x, int y)
        {
            if (x < 0 || y < 0)
                return 0;
            if (x >= 1 && y == 0)
                return a[x, y] + a[x - 1, y];
            if (x == 0 && y >= 1)
                return a[x, y] + a[x, y - 1];
            if (x == 0 && y == 0)
                return a[x, y];
            return a[x, y] + a[x, y - 1] + a[x - 1, y] - a[x - 1, y - 1];
        }

        public override string Part1(string input)
        {
            int serialNumber = Int32.Parse(input);
            sbyte[,] powerLevel = new sbyte[300, 300];
            for (short i = 0; i < 300; i++)
            {
                for (short j = 0; j < 300; j++)
                {
                    short rackId = (short)(i + 10);
                    int tmpPowerLevel = rackId * j;
                    tmpPowerLevel += serialNumber;
                    tmpPowerLevel *= rackId;
                    tmpPowerLevel = tmpPowerLevel % 1000 / 100;

                    powerLevel[i, j] = (sbyte)(tmpPowerLevel - 5);
                }
            }

            sbyte maxCombinedLvl = -1;
            short xcoord = -1;
            short ycoord = -1;
            for (short i = 0; i < 297; i++)
            {
                for (short j = 0; j < 297; j++)
                {
                    sbyte tmp = 0;
                    for (int i2 = 0; i2 < 3; i2++)
                    {
                        for (int j2 = 0; j2 < 3; j2++)
                        {
                            tmp += powerLevel[i + i2, j + j2];
                        }
                    }

                    if (maxCombinedLvl > tmp) continue;
                    maxCombinedLvl = tmp;
                    xcoord = i;
                    ycoord = j;
                }
            }

            return $"{xcoord},{ycoord}";
        }

        public override string Part2(string input)
        {
            int serialNumber = Int32.Parse(input);

            int[,] powerLevel = new int[300, 300];
            for (short i = 0; i < 300; i++)
            {
                for (short j = 0; j < 300; j++)
                {
                    short rackId = (short)(i + 10);
                    int tmpPowerLevel = rackId * j;
                    tmpPowerLevel += serialNumber;
                    tmpPowerLevel *= rackId;
                    tmpPowerLevel = tmpPowerLevel % 1000 / 100;

                    powerLevel[i, j] = tmpPowerLevel - 5;
                }
            }

            for (int i = 0; i < 300; i++)
            {
                for (int j = 0; j < 300; j++)
                {
                    powerLevel[i, j] = BuildSaTable(powerLevel, i, j);
                }
            }

            short maxCombinedLvl = -1;
            short xcoord = -1;
            short ycoord = -1;
            short maxSize = -1;
            for (short size = 1; size < 300; size++)
            {
                for (short i = 0; i < 300 - size; i++)
                {
                    for (short j = 0; j < 300 - size; j++)
                    {
                        int a = powerLevel[i + size, j + size] - powerLevel[i + size, j] - powerLevel[i, j + size] + powerLevel[i, j];
                        if (a <= maxCombinedLvl) continue;
                        maxCombinedLvl = (short)a;
                        xcoord = i;
                        ycoord = j;
                        maxSize = size;
                    }
                }
            }

            return $"{xcoord+1},{ycoord+1},{maxSize}";
        }
    }
}
