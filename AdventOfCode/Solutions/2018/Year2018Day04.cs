using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions
{
    public class Year2018Day04 : Solution
    {
        public override string Part1(string input)
        {
            string[] events = input.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            Array.Sort(events, StringComparer.InvariantCulture);

            int currentGuard = -1;
            byte minuteSleepStart = 0;

            Dictionary<int, Dictionary<byte, int>> asleepMins = new();
            Dictionary<int, int> totalAsleepMins = new();

            foreach (string s in events)
                switch (s[19])
                {
                    //guard change
                    case 'G':
                        currentGuard = Int16.Parse(s.Split(' ', 5)[3][1..]);
                        break;
                    //sleep
                    case 'f':
                        minuteSleepStart = Byte.Parse(s.Split(':', 2)[1].Split(']', 2)[0]);
                        break;
                    //wake up
                    default:
                    {
                        byte minsSpentAsleep =
                            (byte)(Byte.Parse(s.Split(':', 2)[1].Split(']', 2)[0]) - minuteSleepStart);
                        if (asleepMins.ContainsKey(currentGuard))
                        {
                            totalAsleepMins[currentGuard] += minsSpentAsleep;
                            foreach (byte minute in Enumerable.Range(minuteSleepStart, minsSpentAsleep)
                                .Select(i => (byte)i))
                                if (asleepMins[currentGuard].ContainsKey(minute))
                                    asleepMins[currentGuard][minute]++;
                                else
                                    asleepMins[currentGuard].Add(minute, 1);
                        }
                        else
                        {
                            totalAsleepMins.Add(currentGuard, minsSpentAsleep);
                            asleepMins.Add(currentGuard, new Dictionary<byte, int>());
                            foreach (byte minute in Enumerable.Range(minuteSleepStart, minsSpentAsleep)
                                .Select(i => (byte)i)) asleepMins[currentGuard].Add(minute, 1);
                        }

                        break;
                    }
                }

            int maxGuardId = totalAsleepMins.Aggregate((l, r) => l.Value > r.Value ? l : r).Key;

            return (maxGuardId * asleepMins[maxGuardId].Aggregate((l, r) => l.Value > r.Value ? l : r).Key).ToString();
        }

        public override string Part2(string input)
        {
            string[] events = input.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            Array.Sort(events, StringComparer.InvariantCulture);

            int currentGuard = -1;
            byte minuteSleepStart = 0;

            Dictionary<int, Dictionary<byte, int>> asleepMins = new();

            foreach (string s in events)
                switch (s[19])
                {
                    //guard change
                    case 'G':
                        currentGuard = Int16.Parse(s.Split(' ', 5)[3][1..]);
                        break;
                    //sleep
                    case 'f':
                        minuteSleepStart = Byte.Parse(s.Split(':', 2)[1].Split(']', 2)[0]);
                        break;
                    //wake up
                    default:
                    {
                        byte minsSpentAsleep =
                            (byte)(Byte.Parse(s.Split(':', 2)[1].Split(']', 2)[0]) - minuteSleepStart);
                        if (asleepMins.ContainsKey(currentGuard))
                        {
                            foreach (byte minute in Enumerable.Range(minuteSleepStart, minsSpentAsleep)
                                .Select(i => (byte)i))
                                if (asleepMins[currentGuard].ContainsKey(minute))
                                    asleepMins[currentGuard][minute]++;
                                else
                                    asleepMins[currentGuard].Add(minute, 1);
                        }
                        else
                        {
                            asleepMins.Add(currentGuard, new Dictionary<byte, int>());
                            foreach (byte minute in Enumerable.Range(minuteSleepStart, minsSpentAsleep)
                                .Select(i => (byte)i)) asleepMins[currentGuard].Add(minute, 1);
                        }

                        break;
                    }
                }

            int maxGuardId = asleepMins.Aggregate((l, r) =>
                l.Value.Aggregate((l2, r2) => l2.Value > r2.Value ? l2 : r2).Value >
                r.Value.Aggregate((l2, r2) => l2.Value > r2.Value ? l2 : r2).Value
                    ? l
                    : r).Key;

            return (maxGuardId * asleepMins[maxGuardId].Aggregate((l, r) => l.Value > r.Value ? l : r).Key).ToString();
        }
    }
}