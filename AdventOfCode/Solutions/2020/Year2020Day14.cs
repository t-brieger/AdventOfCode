using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions
{
    public class Year2020Day14 : Solution
    {
        public override string Part1(string input)
        {
            string[] lines = input.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            //could be a bit shorter but eh who cares, this is the max length
            Dictionary<long, long> mem = new Dictionary<long, long>(lines.Length - 1);
            bool?[] currentMask = new bool?[0];
            foreach (string line in lines)
            {
                if (line[1] == 'a') //mask
                {
                    currentMask = line.Split(" = ")[1].ToCharArray()
                        .Select(c => c switch
                        {
                            '1' => true,
                            '0' => (bool?) false,
                            _ => null
                        }).ToArray();
                }
                else //memset
                {
                    long memAddress = long.Parse(line.Substring(4).Split("] = ")[0]);
                    long val = long.Parse(line.Split("= ")[1]);
                    for (int i = 0; i < currentMask.Length; i++)
                    {
                        if (currentMask[i] == null)
                            continue;
                        if (currentMask[i] is true)
                            val |= 0b1000_00000000_00000000_00000000_00000000 >> i;
                        else if (currentMask[i] is false)
                            val &= ~(0b1000_00000000_00000000_00000000_00000000 >> i);
                    }

                    mem[memAddress] = val;
                }
            }

            return mem.Select(kvp => kvp.Value).Sum().ToString();
        }

        private void addCombinationsToDict(bool?[] mask, Dictionary<long, long> dict, long origAddress, long origVal)
        {
            for (int i = 0; i < mask.Length; i++)
            {
                if (mask[i] is true)
                {
                    origAddress |= 0b1000_00000000_00000000_00000000_00000000 >> i;
                }
            }

            for (int i = 0; i < mask.Length; i++)
            {
                if (mask[i] == null)
                {
                    mask[i] = false;
                    origAddress |= 0b1000_00000000_00000000_00000000_00000000 >> i;
                    addCombinationsToDict(mask, dict, origAddress, origVal);
                    origAddress &= ~(0b1000_00000000_00000000_00000000_00000000 >> i);
                    addCombinationsToDict(mask, dict, origAddress, origVal);
                    mask[i] = null;
                    return;
                }
            }
            dict[origAddress] = origVal;
        }
        
        public override string Part2(string input)
        {
            string[] lines = input.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            //could be a bit shorter but eh who cares, this is the max length
            Dictionary<long, long> mem = new Dictionary<long, long>(lines.Length - 1);
            bool?[] currentMask = new bool?[0];
            foreach (string line in lines)
            {
                if (line[1] == 'a') //mask
                {
                    currentMask = line.Split(" = ")[1].ToCharArray()
                        .Select(c => c switch
                        {
                            '1' => true,
                            '0' => (bool?) false,
                            _ => null
                        }).ToArray();
                }
                else //memset
                {
                    long memAddress = long.Parse(line.Substring(4).Split("] = ")[0]);
                    long val = long.Parse(line.Split("= ")[1]);
                    addCombinationsToDict(currentMask, mem, memAddress, val);
                }
            }

            return mem.Select(kvp => kvp.Value).Sum().ToString();
        }
    }
}
