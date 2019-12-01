﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions
{
    public class Year2018Day04 : Solution
    {
        public override string Part1(string input)
        {
            String[] events = input.Split('\n');
            Array.Sort(events, StringComparer.InvariantCulture);

            int currentGuard = -1;
            byte minuteSleepStart = 0;

            Dictionary<int, Dictionary<byte, int>> asleepMins = new Dictionary<int, Dictionary<byte, int>>();
            Dictionary<int, int> totalAsleepMins = new Dictionary<int, int>();

            foreach (string s in events)
            {
                if (s[19] == 'G') //guard change
                {
                    currentGuard = Int16.Parse(s.Split(' ', 5)[3].Substring(1));
                }
                else if (s[19] == 'f') //sleep
                {
                    minuteSleepStart = byte.Parse(s.Split(':', 2)[1].Split(']', 2)[0]);
                }
                else //wake up
                {
                    byte minsSpentAsleep = (byte)(byte.Parse(s.Split(':', 2)[1].Split(']', 2)[0]) - minuteSleepStart);
                    if (asleepMins.ContainsKey(currentGuard))
                    {
                        totalAsleepMins[currentGuard] += minsSpentAsleep;
                        foreach (byte minute in Enumerable.Range(minuteSleepStart, minsSpentAsleep))
                            if (asleepMins[currentGuard].ContainsKey(minute))
                                asleepMins[currentGuard][minute]++;
                            else
                                asleepMins[currentGuard].Add(minute, 1);
                    }
                    else
                    {
                        totalAsleepMins.Add(currentGuard, minsSpentAsleep);
                        asleepMins.Add(currentGuard, new Dictionary<byte, int>());
                        foreach (byte minute in Enumerable.Range(minuteSleepStart, minsSpentAsleep))
                            asleepMins[currentGuard].Add(minute, 1);
                    }
                }
            }

            int maxGuardId = totalAsleepMins.Aggregate((l, r) => l.Value > r.Value ? l : r).Key;

            return (maxGuardId * asleepMins[maxGuardId].Aggregate((l, r) => l.Value > r.Value ? l : r).Key).ToString();
        }
        
        public override string Part2(string input)
        {
            String[] events = input.Split('\n');
            Array.Sort(events, StringComparer.InvariantCulture);

            int currentGuard = -1;
            byte minuteSleepStart = 0;

            Dictionary<int, Dictionary<byte, int>> asleepMins = new Dictionary<int, Dictionary<byte, int>>();

            foreach (string s in events)
            {
                if (s[19] == 'G') //guard change
                {
                    currentGuard = Int16.Parse(s.Split(' ', 5)[3].Substring(1));
                }
                else if (s[19] == 'f') //sleep
                {
                    minuteSleepStart = byte.Parse(s.Split(':', 2)[1].Split(']', 2)[0]);
                }
                else //wake up
                {
                    byte minsSpentAsleep = (byte)(byte.Parse(s.Split(':', 2)[1].Split(']', 2)[0]) - minuteSleepStart);
                    if (asleepMins.ContainsKey(currentGuard))
                    {
                        foreach (byte minute in Enumerable.Range(minuteSleepStart, minsSpentAsleep))
                            if (asleepMins[currentGuard].ContainsKey(minute))
                                asleepMins[currentGuard][minute]++;
                            else
                                asleepMins[currentGuard].Add(minute, 1);
                    }
                    else
                    {
                        asleepMins.Add(currentGuard, new Dictionary<byte, int>());
                        foreach (byte minute in Enumerable.Range(minuteSleepStart, minsSpentAsleep))
                            asleepMins[currentGuard].Add(minute, 1);
                    }
                }
            }

            int maxGuardId = asleepMins.Aggregate((l, r) => l.Value.Aggregate((l2, r2) => l2.Value > r2.Value ? l2 : r2).Value > r.Value.Aggregate((l2, r2) => l2.Value > r2.Value ? l2 : r2).Value ? l : r).Key;

            return (maxGuardId * asleepMins[maxGuardId].Aggregate((l, r) => l.Value > r.Value ? l : r).Key).ToString();
        }
    }
}