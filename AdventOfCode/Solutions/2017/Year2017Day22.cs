using System;
using System.Collections.Generic;

namespace AdventOfCode.Solutions
{
    //ant day
    class Year2017Day22 : Solution
    {
        public override string Part1(string input)
        {
            string[] lines = input.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            int startingSize = lines[0].Length / 2;

            HashSet<(int x, int y)> infected = new();

            for (int i = 0; i < lines.Length; i++)
            for (int j = 0; j < lines[i].Length; j++)
                if (lines[i][j] == '#')
                    infected.Add((j - startingSize, i - startingSize));


            int infectionCount = 0;
            (int x, int y) virusPos = (0, 0);
            //NORTH=0,SOUTH=2,EAST=1,WEST=3
            byte virusFacing = 0;
            for (int i = 0; i < 10000; i++)
            {
                if (infected.Contains(virusPos))
                {
                    virusFacing++;
                    infected.Remove(virusPos);
                }
                else
                {
                    //0-- -> 255 % 4 -> 3
                    virusFacing--;
                    infected.Add(virusPos);
                    infectionCount++;
                }

                virusFacing %= 4;
                virusPos = virusFacing switch
                {
                    0 => (virusPos.x, virusPos.y - 1),
                    1 => (virusPos.x + 1, virusPos.y),
                    2 => (virusPos.x, virusPos.y + 1),
                    3 => (virusPos.x - 1, virusPos.y),
                    _ => throw new Exception("virusPos was not 1-3")
                };
            }

            return infectionCount.ToString();
        }

        //this isnt perfect, but im ok with 1.5 seconds
        public override string Part2(string input)
        {
            string[] lines = input.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            int startingSize = lines[0].Length / 2;

            //0: weakened, 2: flagged, 1: infected
            Dictionary<(int x, int y), byte> infected = new();

            for (int i = 0; i < lines.Length; i++)
            for (int j = 0; j < lines[i].Length; j++)
                if (lines[i][j] == '#')
                    infected.Add((j - startingSize, i - startingSize), 1);


            int infectionCount = 0;
            (int x, int y) virusPos = (0, 0);
            //NORTH=0,SOUTH=2,EAST=1,WEST=3
            byte virusFacing = 0;
            for (int i = 0; i < 10000000; i++)
            {
                if (infected.ContainsKey(virusPos))
                {
                    if (infected[virusPos] < 2)
                    {
                        if (infected[virusPos] == 1)
                            virusFacing++;
                        else
                            infectionCount++;
                        infected[virusPos]++;
                    }
                    else
                    {
                        infected.Remove(virusPos);
                        virusFacing += 2;
                    }
                }
                else
                {
                    //0-- -> 255 % 4 -> 3
                    virusFacing--;
                    infected.Add(virusPos, 0);
                }

                virusFacing %= 4;
                virusPos = virusFacing switch
                {
                    0 => (virusPos.x, virusPos.y - 1),
                    1 => (virusPos.x + 1, virusPos.y),
                    2 => (virusPos.x, virusPos.y + 1),
                    3 => (virusPos.x - 1, virusPos.y),
                    _ => throw new Exception("virusPos was not 1-3")
                };
            }

            return infectionCount.ToString();
        }
    }
}