using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions._2018
{
    public class Year2018Day03 : Solution
    {
        public override string Part1(string input)
        {
            string[] tmp2 = input.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

            List<short[]> tmpList = tmp2.Select(s => s.Split(new[] {'#', ' ', '@', ':', 'x', ','}, StringSplitOptions.RemoveEmptyEntries)).Select(splitStrings => new[] {Int16.Parse(splitStrings[0]), Int16.Parse(splitStrings[1]), Int16.Parse(splitStrings[2]), Int16.Parse(splitStrings[3]), Int16.Parse(splitStrings[4])}).ToList();


            short[,] inputs = new short[tmpList.Count, 5];
            for (short i = 0; i < tmpList.Count; i++)
            {
                for (byte j = 0; j < 5; j++)
                    inputs[i, j] = tmpList[i][j];
            }
            short[,] claimed = new short[1001, 1001];


            int dupes = 0;

            for (short i = 0; i < inputs.Length / 5; i++)
            {
                short xs = inputs[i, 1];
                short ys = inputs[i, 2];
                short xe = (short)(inputs[i, 3] + xs);
                short ye = (short)(inputs[i, 4] + ys);

                for (short j = (short)(xs + 1); j <= xe; j++)
                {
                    for (short j2 = (short)(ys + 1); j2 <= ye; j2++)
                    {
                        if (claimed[j, j2] == 1)
                            dupes++;
                        claimed[j, j2]++;
                    }
                }
            }

            return dupes.ToString();
        }
        
        public override string Part2(string input)
        {
            string[] tmp2 = input.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

            List<short[]> tmpList = tmp2.Select(s => s.Split(new[] {'#', ' ', '@', ':', 'x', ','}, StringSplitOptions.RemoveEmptyEntries)).Select(splitStrings => new[] {Int16.Parse(splitStrings[0]), Int16.Parse(splitStrings[1]), Int16.Parse(splitStrings[2]), Int16.Parse(splitStrings[3]), Int16.Parse(splitStrings[4])}).ToList();


            short[,] inputs = new short[tmpList.Count, 5];
            for (short i = 0; i < tmpList.Count; i++)
            {
                for (byte j = 0; j < 5; j++)
                    inputs[i, j] = tmpList[i][j];
            }

            short[,] gridNumClaims = new short[1001, 1001];
            List<short>[,] gridClaims = new List<short>[1001, 1001];
            for (int i = 0; i < 1001; i++)
            {
                for (int j = 0; j < 1001; j++)
                {
                    gridClaims[i, j] = new List<short>();
                }
            }

            for (short i = 0; i < inputs.Length / 5; i++) //inputs is a 2d array using the [*,*] syntax instead of [*][*], so it's a bit awkward to loop through
            {
                //inputs[i, 0] is the claim id again because I'm too lazy to remove it while parsing
                short xs = inputs[i, 1];    //= the x starting point
                short ys = inputs[i, 2];    //= the y starting point
                short xe = (short)(inputs[i, 3] + xs); //=the x ending point
                short ye = (short)(inputs[i, 4] + ys); //=the y ending point

                for (short j = (short)(xs + 1); j <= xe; j++)
                {
                    for (short j2 = (short)(ys + 1); j2 <= ye; j2++)
                    {
                        //loop through each square inch this claim contains
                        gridNumClaims[j, j2]++;
                        gridClaims[j, j2].Add(i);
                    }
                }
            }

            bool[] canBeIntact = Enumerable.Repeat(true, inputs.Length / 5).ToArray(); //number of claims

            for (short i = 0; i < 1001; i++)
            {
                for (short j = 0; j < 1001; j++)
                {
                    if (gridNumClaims[i, j] == 1) continue;
                    foreach (short claim in gridClaims[i, j])
                    {
                        canBeIntact[claim] = false;
                    }
                }
            }

            for (short i = 0; i < canBeIntact.Length; i++)
            {
                if (canBeIntact[i])
                    return (i + 1).ToString();
            }

            return ""; //shouldn't happen, assuming it's always possible to find a solution
        }
    }
}