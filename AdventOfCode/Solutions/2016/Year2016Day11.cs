using System;
using System.Collections.Generic;
using System.Linq;
using ILGPU.Runtime.Cuda;

namespace AdventOfCode.Solutions;

public class Year2016Day11 : Solution
{
    // strings are more convenient because of their immutability
    private static string StateToString(List<Device>[] devices, int ourLevel)
    {
        return ourLevel + "\n" + string.Join('\n',
            devices.Select(floor =>
                string.Join("",
                    floor.Select(dev => dev.Element[..3] + (dev.IsGenerator ? 'G' : 'C')).OrderBy(x => x))));
    }

    private static (List<Device>[], int) StringToState(string state)
    {
        string[] lines = state.Split('\n');
        List<Device>[] result = new List<Device>[lines.Length - 1];
        for (int i = 1; i < lines.Length; i++)
        {
            if (lines[i] == "")
            {
                result[i - 1] = new List<Device>();
                continue;
            }

            result[i - 1] = new List<Device>();

            for (int j = 0; j < lines[i].Length; j += 4)
            {
                result[i - 1].Add(new Device
                {
                    Element = lines[i][j..(j + 3)],
                    IsGenerator = lines[i][j + 3] == 'G'
                });
            }
        }

        return (result, int.Parse(lines[0]));
    }

    private class Device
    {
        public string Element;
        public bool IsGenerator;
    }

    // I'm almost certain this code is correct, but no guarantees about how its runtime compares to the age of the universe.
    public override string Part1(string input)
    {
        input = input.Replace("The ", "").Replace("floor ", "").Replace("contains ", "").Replace(" a ", " ")
            .Replace(" and ", " ").Replace(",", "").Replace(".", "");

        string[][] floorStrings = input.Split('\n').Select(x => x.Split(' ')).ToArray();

        List<Device>[] initialState = new List<Device>[floorStrings.Length];

        for (int i = 0; i < floorStrings.Length; i++)
        {
            if (floorStrings[i][1] == "nothing")
            {
                initialState[i] = new List<Device>();
                continue;
            }

            List<Device> devicesOnFloor = new List<Device>();
            for (int j = 1; j < floorStrings[i].Length; j += 2)
            {
                string element = floorStrings[i][j].Split('-')[0];
                bool isGen = floorStrings[i][j + 1] == "generator";

                devicesOnFloor.Add(new Device() {Element = element, IsGenerator = isGen});
            }

            initialState[i] = devicesOnFloor.ToList();
        }

        (_, int steps) = Util.Djikstra(StateToString(initialState, 0), (state, cost) =>
        {
            (List<Device>[] devices, int floor) = StringToState(state);

            List<(string, int)> reachable = new List<(string, int)>();

            for (int i = 0; i < devices[floor].Count; i++)
            {
                if (floor != 3)
                {
                    // copy
                    (List<Device>[], int) tmp = StringToState(state);
                    tmp.Item1[floor].Remove(devices[floor][i]);
                    tmp.Item1[floor + 1].Add(devices[floor][i]);
                    if (!tmp.Item1.Any(floorDevices => floorDevices.Where(dev => !dev.IsGenerator).Any(chip =>
                            !floorDevices.Any(gen => gen.IsGenerator && gen.Element == chip.Element) &&
                            floorDevices.Any(gen => gen.IsGenerator && gen.Element != chip.Element))))
                        reachable.Add((StateToString(tmp.Item1, tmp.Item2 + 1), cost + 1));
                }

                if (floor != 0)
                {
                    (List<Device>[], int) tmp = StringToState(state);
                    tmp.Item1[floor].Remove(devices[floor][i]);
                    tmp.Item1[floor - 1].Add(devices[floor][i]);
                    if (!tmp.Item1.Any(floorDevices => floorDevices.Where(dev => !dev.IsGenerator).Any(chip =>
                            !floorDevices.Any(gen => gen.IsGenerator && gen.Element == chip.Element) &&
                            floorDevices.Any(gen => gen.IsGenerator && gen.Element != chip.Element))))
                        reachable.Add((StateToString(tmp.Item1, tmp.Item2 - 1), cost + 1));
                }
            }

            for (int i = 0; i < devices[floor].Count; i++)
            {
                for (int j = i + 1; j < devices[floor].Count; j++)
                {
                    if (floor != 3)
                    {
                        (List<Device>[], int) tmp = StringToState(state);
                        tmp.Item1[floor].Remove(devices[floor][i]);
                        tmp.Item1[floor].Remove(devices[floor][j]);
                        tmp.Item1[floor + 1].Add(devices[floor][i]);
                        tmp.Item1[floor + 1].Add(devices[floor][j]);
                        if (!tmp.Item1.Any(floorDevices => floorDevices.Where(dev => !dev.IsGenerator).Any(chip =>
                                !floorDevices.Any(gen => gen.IsGenerator && gen.Element == chip.Element) &&
                                floorDevices.Any(gen => gen.IsGenerator && gen.Element != chip.Element))))
                            reachable.Add((StateToString(tmp.Item1, tmp.Item2 + 1), cost + 1));
                    }

                    if (floor != 0)
                    {
                        (List<Device>[], int) tmp = StringToState(state);
                        tmp.Item1[floor].Remove(devices[floor][i]);
                        tmp.Item1[floor].Remove(devices[floor][j]);
                        tmp.Item1[floor - 1].Add(devices[floor][i]);
                        tmp.Item1[floor - 1].Add(devices[floor][j]);
                        if (!tmp.Item1.Any(floorDevices => floorDevices.Where(dev => !dev.IsGenerator).Any(chip =>
                                !floorDevices.Any(gen => gen.IsGenerator && gen.Element == chip.Element) &&
                                floorDevices.Any(gen => gen.IsGenerator && gen.Element != chip.Element))))
                            reachable.Add((StateToString(tmp.Item1, tmp.Item2 - 1), cost + 1));
                    }
                }
            }

            return reachable;
        }, state => state.FirstIndexOf(c => c is >= 'A' and <= 'Z' or >= 'a' and <= 'z') == 5);

        return steps.ToString();
    }

    public override string Part2(string input)
    {
        //TODO
        return null;
    }
}
