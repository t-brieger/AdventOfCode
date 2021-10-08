using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions
{
    public class Year2019Day10 : Solution
    {
        private static ((long, long)[], int, int) findResearchStation(HashSet<(int, int)> asteroids)
        {
            ((long, long)[], int, int)[] x = asteroids.Select(researchStation =>
                (asteroids.Where(x =>
                        x.Item1 != researchStation.Item1 || x.Item2 != researchStation.Item2
                    //Map each asteroid to an array of all asteroids except iself
                ).Select(x =>
                        (x.Item1 - researchStation.Item1, x.Item2 - researchStation.Item2)
                    //Make the arrays of asteroids vectors from our possible research station
                ).Select(x =>
                        (x.Item1 / Util.Gcd(x.Item1, x.Item2), x.Item2 / Util.Gcd(x.Item1, x.Item2))
                    //simplify the vectors (by dividing by the gcd of x and y), then take the unique ones only - this
                    //gives us all directions we can see meteorites in
                ).ToArray(), researchStation.Item1, researchStation.Item2)
            ).ToArray();

            return x.MaxBy(e => e.Item1.Distinct().Count());
        }

        public override string Part1(string input)
        {
            string[] lines = input.Split("\n");

            HashSet<(int, int)> asteroids = new();

            for (int i = 0; i < lines.Length; i++)
            for (int j = 0; j < lines[i].Length; j++)
                if (lines[i][j] == '#')
                    asteroids.Add((j, i));

            return findResearchStation(asteroids).Item1.Distinct().Count().ToString();
        }

        public override string Part2(string input)
        {
            string[] lines = input.Split("\n");

            HashSet<(int, int)> asteroids = new();

            for (int i = 0; i < lines.Length; i++)
            for (int j = 0; j < lines[i].Length; j++)
                if (lines[i][j] == '#')
                    asteroids.Add((j, i));

            ((long, long)[], int, int) ourStation = findResearchStation(asteroids);

            Dictionary<(long, long), PriorityQueue<(long, long), long>> fractionsToAsteroidQueues = new();

            foreach ((long x, long y) in ourStation.Item1)
            {
                int gcd = (int)Util.Gcd(x, y);
                (long, long) dividedTuple = (x / gcd, y / gcd);
                if (!fractionsToAsteroidQueues.ContainsKey(dividedTuple))
                    fractionsToAsteroidQueues.Add(dividedTuple, new PriorityQueue<(long, long), long>());

                fractionsToAsteroidQueues[dividedTuple].Enqueue((x + ourStation.Item2, y + ourStation.Item3), gcd);
            }

            (double, (long, long))[] keyOrder = fractionsToAsteroidQueues.Keys
                .Select(t => (Math.Atan2(t.Item1, t.Item2), t)).OrderByDescending(t => t.Item1).ToArray();

            return fractionsToAsteroidQueues[keyOrder[199].Item2].Dequeue().ToString();
        }
    }
}