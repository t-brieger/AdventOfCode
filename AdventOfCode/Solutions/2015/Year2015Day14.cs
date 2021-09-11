using System;
using System.Linq;

namespace AdventOfCode.Solutions
{
    public class Year2015Day14 : Solution
    {
        public override string Part1(string input)
        {
            int maxDist = int.MinValue;
            foreach (string[] reindeerInfo in input.Split("\n", StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Split(' ')))
            {
                int speed = int.Parse(reindeerInfo[3]);
                int secFly = int.Parse(reindeerInfo[6]);
                int secRest = int.Parse(reindeerInfo[13]);
                bool flying = true;
                int secsInCurrentState = 0;
                int dist = 0;

                for (int i = 0; i < 2503; i++)
                {
                    secsInCurrentState++;
                    if (flying)
                        dist += speed;

                    if ((!flying || secsInCurrentState < secFly) && (flying || secsInCurrentState < secRest)) continue;
                    flying = !flying;
                    secsInCurrentState = 0;
                }

                maxDist = Math.Max(maxDist, dist);
            }

            return maxDist.ToString();
        }

        public override string Part2(string input)
        {
            string[] lines = input.Split("\n", StringSplitOptions.RemoveEmptyEntries);

            (int, int, int, int, int, bool)[] reindeerInfo = new (int, int, int, int, int, bool)[lines.Length];
            int[] scores = new int[reindeerInfo.Length];
            for (int i = 0; i < lines.Length; i++)
            {
                string[] split = lines[i].Split(' ');
                reindeerInfo[i] = (int.Parse(split[3]), int.Parse(split[6]), int.Parse(split[13]), 0, 0, true);
                scores[i] = 0;
            }

            for (int i = 0; i < 2503; i++)
            {
                for (int j = 0; j < reindeerInfo.Length; j++)
                {
                    (int speed, int secFly, int secRest, int secsInCurrentState, int dist, bool flying) =
                        reindeerInfo[j];

                    secsInCurrentState++;
                    if (flying)
                        dist += speed;

                    if (flying && secsInCurrentState >= secFly || !flying && secsInCurrentState >= secRest)
                    {
                        flying = !flying;
                        secsInCurrentState = 0;
                    }

                    reindeerInfo[j] = (speed, secFly, secRest, secsInCurrentState, dist, flying);
                }

                for (int j = 0; j < scores.Length; j++)
                    if (reindeerInfo[j].Item5 == reindeerInfo.Max(t => t.Item5))
                        scores[j]++;
            }

            return scores.Max().ToString();
        }
    }
}