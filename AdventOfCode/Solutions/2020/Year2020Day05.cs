using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions
{
    public class Year2020Day05 : Solution
    {
        public override string Part1(string input)
        {
            char[][] passes = input.Split('\n', StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.ToCharArray()).ToArray();

            int highestId = -1;

            foreach (char[] pass in passes)
            {
                byte min = 0;
                for (int i = 0; i < 7; i++)
                {
                    if (pass[i] == 'B')
                        min += (byte)(1 << (6 - i));
                }

                int id = min * 8;
                min = 0;

                for (int i = 7; i < 10; i++)
                {
                    if (pass[i] == 'R')
                        min += (byte)(1 << (9 - i));
                }

                id += min;
                highestId = highestId < id ? id : highestId;
            }
            
            return highestId.ToString();
        }

        public override string Part2(string input)
        {
            char[][] passes = input.Split('\n', StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.ToCharArray()).ToArray();

            HashSet<int> possibleRows = new HashSet<int>();
            Dictionary<int, byte> rowsToAvailableSeats = new Dictionary<int, byte>();

            foreach (char[] pass in passes)
            {
                byte min = 0;
                for (int i = 0; i < 7; i++)
                {
                    if (pass[i] == 'B')
                        min += (byte)(1 << (6 - i));
                }

                int row = min;
                
                possibleRows.Add(row);
                if (!rowsToAvailableSeats.ContainsKey(row))
                    rowsToAvailableSeats.Add(row, 0);
                
                min = 0;

                for (int i = 7; i < 10; i++)
                {
                    if (pass[i] == 'R')
                        min += (byte)(1 << (9 - i));
                }

                rowsToAvailableSeats[row] |= (byte)(1 << min);


            }

            foreach (int row in possibleRows)
            {
                if (!possibleRows.Contains(row - 1) || !possibleRows.Contains(row + 1))
                    continue;
                if (rowsToAvailableSeats[row] != 255)
                {
                    byte b = rowsToAvailableSeats[row];

                    if ((b & 0b00000001) == 0)
                        return (row * 8 + 0).ToString();
                    if ((b & 0b00000010) == 0)
                        return (row * 8 + 1).ToString();
                    if ((b & 0b00000100) == 0)
                        return (row * 8 + 2).ToString();
                    if ((b & 0b00001000) == 0)
                        return (row * 8 + 3).ToString();
                    if ((b & 0b00010000) == 0)
                        return (row * 8 + 4).ToString();
                    if ((b & 0b00100000) == 0)
                        return (row * 8 + 5).ToString();
                    if ((b & 0b01000000) == 0)
                        return (row * 8 + 6).ToString();
                    if ((b & 0b10000000) == 0)
                        return (row * 8 + 7).ToString();
                }
                    
            }
            
            return "not found?";
        }
    }
}
