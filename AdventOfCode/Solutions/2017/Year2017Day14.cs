using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode.Solutions._2017
{
    public class Year2017Day14 : Solution
    {
        public override string Part1(string input)
        {
            int bits = 0;
            for (int i = 0; i < 128; i++)
            {
                var hash = Year2017Day10.getKnotHash(input + "-" + i);
                for (byte j = 0; j < hash.Length; j++)
                {
                    bits += (hash[j] & (1 << 0)) >> 0;
                    bits += (hash[j] & (1 << 1)) >> 1;
                    bits += (hash[j] & (1 << 2)) >> 2;
                    bits += (hash[j] & (1 << 3)) >> 3;
                    bits += (hash[j] & (1 << 4)) >> 4;
                    bits += (hash[j] & (1 << 5)) >> 5;
                    bits += (hash[j] & (1 << 6)) >> 6;
                    bits += (hash[j] & (1 << 7)) >> 7;
                }
            }

            return bits.ToString();
        }

        private static string HexToBinary(string hex)
        {
            StringBuilder sb = new StringBuilder();

            foreach (var c in hex.ToCharArray())
            {
                var intValue = int.Parse(c.ToString(), System.Globalization.NumberStyles.HexNumber);
                sb.Append(Convert.ToString(intValue, 2).PadLeft(4, '0'));
            }

            return sb.ToString();
        }

        private static void floodFill(HashSet<(byte, byte)> seen, bool[,] grid, byte x, byte y)
        {
            if (grid[x, y] || seen.Contains((x, y)))
                return;

            seen.Add((x, y));

            if (x != 0)
                floodFill(seen, grid, (byte) (x - 1), y);
            if (x != grid.GetLength(0) - 1)
                floodFill(seen, grid, (byte) (x + 1), y);
            if (y != 0)
                floodFill(seen, grid, x, (byte) (y - 1));
            if (y != grid.GetLength(1) - 1)
                floodFill(seen, grid, x, (byte) (y + 1));
        }

        public override string Part2(string input)
        {
            bool[,] grid = new bool[128, 128];

            for (int i = 0; i < 128; i++)
            {
                var hash = HexToBinary(BitConverter.ToString(Year2017Day10.getKnotHash($"{input}-{i}")).Replace("-", ""));
                for (int j = 0; j < hash.Length; j++)
                    grid[i, j] = hash[j] == '0';
            }

            HashSet<(byte, byte)> seen = new HashSet<(byte, byte)>();

            int regions = 0;

            for (byte i = 0; i < grid.GetLength(0); i++)
            {
                for (byte j = 0; j < grid.GetLength(1); j++)
                {
                    if (!grid[i, j] && !seen.Contains((i, j)))
                    {
                        regions++;
                        floodFill(seen, grid, i, j);
                    }
                }
            }

            return regions.ToString();
        }
    }
}