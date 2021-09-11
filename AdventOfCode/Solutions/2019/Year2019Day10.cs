using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions
{
    public class Year2019Day10 : Solution
    {
        public override string Part1(string input)
        {
            string[] lines = input.Split("\n");

            HashSet<(int, int)> asteroids = new();

            for (int i = 0; i < lines.Length; i++)
            for (int j = 0; j < lines[i].Length; j++)
                if (lines[i][j] == '#')
                    asteroids.Add((j, i));

            (long, long)[][] x = asteroids.Select(researchStation =>
                asteroids.Where(x =>
                        x.Item1 != researchStation.Item1 || x.Item2 != researchStation.Item2
                    //Map each asteroid to an array of all asteroids except iself
                ).Select(x =>
                        (x.Item1 - researchStation.Item1, x.Item2 - researchStation.Item2)
                    //Make the arrays of asteroids vectors from our possible research station
                ).Select(x =>
                        (x.Item1 / Util.Gcd(x.Item1, x.Item2), x.Item2 / Util.Gcd(x.Item1, x.Item2))
                    //simplify the vectors (by dividing by the gcd of x and y), then take the unique ones only - this
                    //gives us all directions we can see meteorites in
                ).Distinct().ToArray()
            ).ToArray();

            return x.Max(y => y.Length).ToString();
        }

        public override string Part2(string input)
        {
            //TODO
            return null;
        }
    }
}