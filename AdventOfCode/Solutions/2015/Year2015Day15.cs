using System;
using System.Linq;

namespace AdventOfCode.Solutions
{
    public class Year2015Day15 : Solution
    {
        public override string Part1(string input)
        {
            (int, int, int, int)[] ingredients = input.Split("\n", StringSplitOptions.RemoveEmptyEntries)
                .Select(line => line.Replace(",", "").Split(' ')).Select(lineArr => (int.Parse(lineArr[2]),
                    int.Parse(lineArr[4]), int.Parse(lineArr[6]), int.Parse(lineArr[8]))).ToArray();

            int maxScore = int.MinValue;

            //I tried for a solid hour now to make this work for a different amount of ingredient types, and I can't
            //seem to do it - this may be due to it being 2am, me being a bad programmer, or a combination of the two.
            for (int i1 = 0; i1 < 100; i1++)
            for (int i2 = 0; i2 < 100 - i1; i2++)
            for (int i3 = 0; i3 < 100 - i1 - i2; i3++)
            {
                int i4 = 100 - i1 - i2 - i3;
                int capacity = i1 * ingredients[0].Item1 + i2 * ingredients[1].Item1 +
                               i3 * ingredients[2].Item1 + i4 * ingredients[3].Item1;
                int durability = i1 * ingredients[0].Item2 + i2 * ingredients[1].Item2 +
                                 i3 * ingredients[2].Item2 + i4 * ingredients[3].Item2;
                int flavor = i1 * ingredients[0].Item3 + i2 * ingredients[1].Item3 +
                             i3 * ingredients[2].Item3 + i4 * ingredients[3].Item3;
                int texture = i1 * ingredients[0].Item4 + i2 * ingredients[1].Item4 +
                              i3 * ingredients[2].Item4 + i4 * ingredients[3].Item4;

                int score = capacity * durability * flavor * texture;
                if (capacity <= 0 || durability <= 0 || flavor <= 0 || texture <= 0)
                    score = 0;

                maxScore = Math.Max(maxScore, score);
            }

            return maxScore.ToString();
        }

        public override string Part2(string input)
        {
            (int, int, int, int, int)[] ingredients = input.Split("\n", StringSplitOptions.RemoveEmptyEntries)
                .Select(line => line.Replace(",", "").Split(' ')).Select(lineArr => (int.Parse(lineArr[2]),
                    int.Parse(lineArr[4]), int.Parse(lineArr[6]), int.Parse(lineArr[8]), int.Parse(lineArr[10])))
                .ToArray();

            int maxScore = int.MinValue;

            for (int i1 = 0; i1 < 100; i1++)
            for (int i2 = 0; i2 < 100 - i1; i2++)
            for (int i3 = 0; i3 < 100 - i1 - i2; i3++)
            {
                int i4 = 100 - i1 - i2 - i3;
                int calories = i1 * ingredients[0].Item5 + i2 * ingredients[1].Item5 +
                               i3 * ingredients[2].Item5 + i4 * ingredients[3].Item5;
                if (calories != 500)
                    continue;

                int capacity = i1 * ingredients[0].Item1 + i2 * ingredients[1].Item1 +
                               i3 * ingredients[2].Item1 + i4 * ingredients[3].Item1;
                int durability = i1 * ingredients[0].Item2 + i2 * ingredients[1].Item2 +
                                 i3 * ingredients[2].Item2 + i4 * ingredients[3].Item2;
                int flavor = i1 * ingredients[0].Item3 + i2 * ingredients[1].Item3 +
                             i3 * ingredients[2].Item3 + i4 * ingredients[3].Item3;
                int texture = i1 * ingredients[0].Item4 + i2 * ingredients[1].Item4 +
                              i3 * ingredients[2].Item4 + i4 * ingredients[3].Item4;

                int score = capacity * durability * flavor * texture;
                if (capacity <= 0 || durability <= 0 || flavor <= 0 || texture <= 0)
                    score = 0;

                maxScore = Math.Max(maxScore, score);
            }

            return maxScore.ToString();
        }
    }
}