﻿using System;
using System.Collections.Generic;

namespace AdventOfCode.Solutions
{
    public class Year2017Day18 : Solution
    {
        private static long GetValueOfString(string s, IReadOnlyList<long> registers)
        {
            return s[0] >= 'a' ? registers[s[0] - 'a'] : Int64.Parse(s);
        }

        public override string Part1(string input)
        {
            long[] registers = new long[26];

            for (int i = 0; i < 26; i++)
                registers[i] = 0;

            string[] instructions = input.Split('\n', StringSplitOptions.RemoveEmptyEntries);

            long lastFrequency = -1;

            for (long i = 0; i < instructions.Length; i++)
            {
                string instruction = instructions[i];
                string[] arguments = instruction.Split(' ');

                switch (arguments[0])
                {
                    case "snd":
                        lastFrequency = GetValueOfString(arguments[1], registers);
                        break;
                    case "set":
                        registers[arguments[1][0] - 'a'] = GetValueOfString(arguments[2], registers);
                        break;
                    case "add":
                        registers[arguments[1][0] - 'a'] += GetValueOfString(arguments[2], registers);
                        break;
                    case "mul":
                        registers[arguments[1][0] - 'a'] *= GetValueOfString(arguments[2], registers);
                        break;
                    case "mod":
                        registers[arguments[1][0] - 'a'] %= GetValueOfString(arguments[2], registers);
                        break;
                    case "rcv":
                        if (GetValueOfString(arguments[1], registers) != 0)
                            return lastFrequency.ToString();
                        break;
                    case "jgz":
                        if (GetValueOfString(arguments[1], registers) > 0)
                            i += GetValueOfString(arguments[2], registers) - 1;
                        break;
                    default:
                        return "unrecognized instruction: " + instruction;
                }
            }

            return "";
        }

        // Spaghetti
        public override string Part2(string input)
        {
            long[] registers0 = new long[26];
            long[] registers1 = new long[26];

            registers0['p' - 'a'] = 0;
            registers1['p' - 'a'] = 1;

            string[] instructions = input.Split('\n', StringSplitOptions.RemoveEmptyEntries);

            bool oneWaiting = false, twoWaiting = false;

            long i0 = 0;
            long i1 = 0;

            Queue<long> queue0 = new();
            Queue<long> queue1 = new();

            long oneSendCount = 0;

            while (!(oneWaiting && twoWaiting))
            {
                if (!oneWaiting)
                {
                    if (i0 >= instructions.Length)
                    {
                        oneWaiting = true;
                        continue;
                    }

                    string instruction = instructions[i0];
                    string[] arguments = instruction.Split(' ');

                    switch (arguments[0])
                    {
                        case "snd":
                            queue1.Enqueue(GetValueOfString(arguments[1], registers0));
                            twoWaiting = false;
                            break;
                        case "set":
                            registers0[arguments[1][0] - 'a'] = GetValueOfString(arguments[2], registers0);
                            break;
                        case "add":
                            registers0[arguments[1][0] - 'a'] += GetValueOfString(arguments[2], registers0);
                            break;
                        case "mul":
                            registers0[arguments[1][0] - 'a'] *= GetValueOfString(arguments[2], registers0);
                            break;
                        case "mod":
                            registers0[arguments[1][0] - 'a'] %= GetValueOfString(arguments[2], registers0);
                            break;
                        case "rcv":
                            if (queue0.Count == 0)
                            {
                                oneWaiting = true;
                                continue;
                            }
                            else
                            {
                                registers0[arguments[1][0] - 'a'] = queue0.Dequeue();
                            }

                            break;
                        case "jgz":
                            if (GetValueOfString(arguments[1], registers0) > 0)
                                i0 += GetValueOfString(arguments[2], registers0) - 1;
                            break;
                        default:
                            return "unrecognized instruction: " + instruction;
                    }

                    i0++;
                }

                if (twoWaiting) continue;
                {
                    if (i1 >= instructions.Length)
                    {
                        twoWaiting = true;
                        continue;
                    }


                    string instruction = instructions[i1];
                    string[] arguments = instruction.Split(' ');

                    switch (arguments[0])
                    {
                        case "snd":
                            oneSendCount++;
                            queue0.Enqueue(GetValueOfString(arguments[1], registers1));
                            oneWaiting = false;
                            break;
                        case "set":
                            registers1[arguments[1][0] - 'a'] = GetValueOfString(arguments[2], registers1);
                            break;
                        case "add":
                            registers1[arguments[1][0] - 'a'] += GetValueOfString(arguments[2], registers1);
                            break;
                        case "mul":
                            registers1[arguments[1][0] - 'a'] *= GetValueOfString(arguments[2], registers1);
                            break;
                        case "mod":
                            registers1[arguments[1][0] - 'a'] %= GetValueOfString(arguments[2], registers1);
                            break;
                        case "rcv":
                            if (queue1.Count == 0)
                            {
                                twoWaiting = true;
                                continue;
                            }
                            else
                            {
                                registers1[arguments[1][0] - 'a'] = queue1.Dequeue();
                            }

                            break;
                        case "jgz":
                            if (GetValueOfString(arguments[1], registers1) > 0)
                                i1 += GetValueOfString(arguments[2], registers1) - 1;
                            break;
                        default:
                            return "unrecognized instruction: " + instruction;
                    }

                    i1++;
                }
            }

            return oneSendCount.ToString();
        }
    }
}