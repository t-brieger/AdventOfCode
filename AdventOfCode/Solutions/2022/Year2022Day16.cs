using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions;

public class Year2022Day16 : Solution
{
    private int MaximalRelease((int, int) timeLeft, string[] valves,
        Dictionary<string, (int rate, string[] tunnels)> valveInfo,
        Dictionary<(string, string), int> pathLengths, bool part2 = false, (int, int)? position = null, ulong open = 0,
        Dictionary<((int, int), (int, int)?, bool, ulong), int> seen = null)
    {
        seen ??= new Dictionary<((int, int), (int, int)?, bool, ulong), int>();

        if (position == null)
        {
            int ix = valves.FirstIndexOf(v => v == "AA");
            position = (ix, ix);
        }

        if (open == 0)
            RescaleBar((part2 ? 2 : 1) * valves.Length);

        if (!seen.ContainsKey((timeLeft, position, false, open)))
        {
            int val = 0;
            if (timeLeft.Item1 > 1)
            {
                for (int valveIx = 0; valveIx < valves.Length; valveIx++)
                {
                    string destination = valves[valveIx];

                    if (open == 0)
                        IncreaseBar();
                    if (valveIx == position.Value.Item1)
                        continue;
                    if (valveInfo[destination].rate == 0)
                        continue;
                    if (timeLeft.Item1 <= pathLengths[(valves[position.Value.Item1], destination)])
                        continue;
                    if ((open & (1UL << valveIx)) != 0)
                        continue;

                    open ^= 1UL << valveIx;
                    int flowFromOpened =
                        (timeLeft.Item1 - pathLengths[(valves[position.Value.Item1], destination)] - 1) *
                        valveInfo[destination].rate;
                    int maxRemainingFlow =
                        MaximalRelease(
                            (timeLeft.Item1 - pathLengths[(valves[position.Value.Item1], destination)] - 1,
                                timeLeft.Item2), valves,
                            valveInfo, pathLengths, part2, (valveIx, position.Value.Item2), open, seen);
                    int pathValue = flowFromOpened + maxRemainingFlow;
                    val = Math.Max(val, pathValue);

                    open ^= 1UL << valveIx;
                }
            }

            seen.Add((timeLeft, position, false, open), val);
            seen.Add(((timeLeft.Item2, timeLeft.Item1), (position.Value.Item2, position.Value.Item1), true, open), val);
        }

        if (open == 0 && part2)
            SetBar(GetBarScale() / 2);
        
        if (!seen.ContainsKey((timeLeft, position, true, open)))
        {
            int val = 0;
            if (timeLeft.Item2 > 1)
            {
                for (int valveIx = 0; valveIx < valves.Length; valveIx++)
                {
                    string destination = valves[valveIx];

                    if (open == 0 && part2)
                        IncreaseBar();
                    if (valveIx == position.Value.Item2)
                        continue;
                    if (valveInfo[destination].rate == 0)
                        continue;
                    if (timeLeft.Item2 <= pathLengths[(valves[position.Value.Item2], destination)])
                        continue;
                    if ((open & (1UL << valveIx)) != 0)
                        continue;

                    open ^= 1UL << valveIx;
                    int flowFromOpened =
                        (timeLeft.Item2 - pathLengths[(valves[position.Value.Item2], destination)] - 1) *
                        valveInfo[destination].rate;
                    int maxRemainingFlow =
                        MaximalRelease(
                            (timeLeft.Item1,
                                timeLeft.Item2 - pathLengths[(valves[position.Value.Item2], destination)] - 1), valves,
                            valveInfo, pathLengths, part2, (position.Value.Item1, valveIx), open, seen);
                    int pathValue = flowFromOpened + maxRemainingFlow;
                    val = Math.Max(val, pathValue);

                    open ^= 1UL << valveIx;
                }
            }

            seen.Add((timeLeft, position, true, open), val);
            seen.Add(((timeLeft.Item2, timeLeft.Item1), (position.Value.Item2, position.Value.Item1), false, open),
                val);
        }

        return Math.Max(seen[(timeLeft, position, false, open)], seen[(timeLeft, position, true, open)]);
    }

    public override string Part1(string input)
    {
        string[] valveStrings = input.Split('\n');

        Dictionary<string, (int rate, string[] tunnels)>
            valves = new Dictionary<string, (int rate, string[] tunnels)>();
        foreach (string valve in valveStrings)
        {
            string[] split = valve.Split(new[] {' ', ';', '=', ','}, StringSplitOptions.RemoveEmptyEntries);

            string name = split[1];
            int rate = int.Parse(split[5]);
            string[] connections = split.Skip(10).ToArray();

            valves.Add(name, (rate, connections));
        }

        Dictionary<(string, string), int> pathLengths = new Dictionary<(string, string), int>();
        foreach (string start in valves.Keys)
        {
            foreach (string end in valves.Keys)
            {
                pathLengths.Add((start, end),
                    Util.Djikstra(start, (curr, cost) => { return valves[curr].tunnels.Select(t => (t, cost + 1)); },
                        curr => curr == end).Item2);
            }
        }

        return MaximalRelease((30, 0), valves.Keys.ToArray(), valves, pathLengths).ToString();
    }

    public override string Part2(string input)
    {
        string[] valveStrings = input.Split('\n');

        Dictionary<string, (int rate, string[] tunnels)>
            valves = new Dictionary<string, (int rate, string[] tunnels)>();
        foreach (string valve in valveStrings)
        {
            string[] split = valve.Split(new[] {' ', ';', '=', ','}, StringSplitOptions.RemoveEmptyEntries);

            string name = split[1];
            int rate = int.Parse(split[5]);
            string[] connections = split.Skip(10).ToArray();

            valves.Add(name, (rate, connections));
        }

        Dictionary<(string, string), int> pathLengths = new Dictionary<(string, string), int>();
        foreach (string start in valves.Keys)
        {
            foreach (string end in valves.Keys)
            {
                pathLengths.Add((start, end),
                    Util.Djikstra(start, (curr, cost) => { return valves[curr].tunnels.Select(t => (t, cost + 1)); },
                        curr => curr == end).Item2);
            }
        }

        return MaximalRelease((26, 26), valves.Keys.ToArray(), valves, pathLengths, true).ToString();
    }
}
