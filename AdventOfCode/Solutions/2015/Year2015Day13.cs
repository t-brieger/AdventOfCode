using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions
{
    public class Year2015Day13 : Solution
    {
        //quite exactly day 9 pt 2 - reverse TSP
        public override string Part1(string input)
        {
            Dictionary<(string, string), int> likeScores = input.Split('\n', StringSplitOptions.RemoveEmptyEntries)
                .Select(line => line.Split(' ')).ToDictionary(arr => (arr[0], arr[10][..^1]),
                    arr => (arr[2][0] == 'l' ? -1 : 1) * int.Parse(arr[3]));

            int maxDist = Int32.MinValue;
            foreach (string[] perm in Util.GetPermutations(likeScores.Keys.Select(x => x.Item1).Distinct()
                .ToArray()))
            {
                int dist = 0;
                for (int i = 0; i < perm.Length; i++)
                {
                    dist += likeScores[(perm[i], perm[(i + 1) % perm.Length])];
                    dist += likeScores[(perm[(i + 1) % perm.Length], perm[i])];
                }

                maxDist = Math.Max(maxDist, dist);
            }

            return maxDist.ToString();
        }

        public override string Part2(string input)
        {
            Dictionary<(string, string), int> likeScores = input.Split('\n', StringSplitOptions.RemoveEmptyEntries)
                .Select(line => line.Split(' ')).ToDictionary(arr => (arr[0], arr[10][..^1]),
                    arr => (arr[2][0] == 'l' ? -1 : 1) * int.Parse(arr[3]));

            List<KeyValuePair<(string, string), int>> toAdd = new();
            foreach (string s in likeScores.Keys.Select(x => x.Item1).Distinct())
            {
                toAdd.Add(new KeyValuePair<(string, string), int>(("self", s), 0));
                toAdd.Add(new KeyValuePair<(string, string), int>((s, "self"), 0));
            }

            foreach (((string, string) key, int value) in toAdd)
                likeScores.Add(key, value);

            int maxDist = Int32.MinValue;
            foreach (string[] perm in Util.GetPermutations(likeScores.Keys.Select(x => x.Item1).Distinct()
                .ToArray()))
            {
                int dist = 0;
                for (int i = 0; i < perm.Length; i++)
                {
                    dist += likeScores[(perm[i], perm[(i + 1) % perm.Length])];
                    dist += likeScores[(perm[(i + 1) % perm.Length], perm[i])];
                }

                maxDist = Math.Max(maxDist, dist);
            }

            return maxDist.ToString();
        }
    }
}