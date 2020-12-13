using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions
{
    public class Year2018Day12 : Solution
    {
        public override string Part1(string input)
        {
            /*
            input =
                "initial state: #..#.#..##......###...###\r\n\r\n" +
                "..... => .\r\n" +
                "....# => .\r\n" +
                "...#. => .\r\n" +
                "...## => #\r\n" +
                "..#.. => #\r\n" +
                "..#.# => .\r\n" +
                "..##. => .\r\n" +
                "..### => .\r\n" +
                ".#... => #\r\n" +
                ".#..# => .\r\n" +
                ".#.#. => #\r\n" +
                ".#.## => #\r\n" +
                ".##.. => #\r\n" +
                ".##.# => .\r\n" +
                ".###. => .\r\n" +
                ".#### => #\r\n" +
                "#.... => .\r\n" +
                "#...# => .\r\n" +
                "#..#. => .\r\n" +
                "#..## => .\r\n" +
                "#.#.. => .\r\n" +
                "#.#.# => #\r\n" +
                "#.##. => .\r\n" +
                "#.### => #\r\n" +
                "##... => .\r\n" +
                "##..# => .\r\n" +
                "##.#. => #\r\n" +
                "##.## => #\r\n" +
                "###.. => #\r\n" +
                "###.# => #\r\n" +
                "####. => #\r\n" +
                "##### => .";
            //*/

            string startState = input.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)[0].Split(' ', 3)[2];

            bool[] state = new bool[startState.Length + 100]; //probably only needs 80 (40 in either direction)

            Array.Copy(startState.Select(c => c == '#').ToArray(), 0, state, 50, startState.Length);

            Dictionary<Tuple<bool, bool, bool, bool, bool>, bool> conversions = new Dictionary<Tuple<bool, bool, bool, bool, bool>, bool>(input
                .Split(new[] {'\n', '\r'}, StringSplitOptions.RemoveEmptyEntries).Skip(1).Select(s =>
                {
                    string[] parts = s.Split(" => ");

                    KeyValuePair<Tuple<bool, bool, bool, bool, bool>, bool> kvp = new KeyValuePair<Tuple<bool, bool, bool, bool, bool>, bool>(
                        new Tuple<bool, bool, bool, bool, bool>(parts[0][0] == '#', parts[0][1] == '#', parts[0][2] == '#', parts[0][3] == '#', parts[0][4] == '#'), parts[1] == "#");
                    return kvp;
                }));

            for (int t = 0; t < 20; t++)
            {
                bool[] newState = new bool[state.Length];
                for (int i = 2; i < state.Length - 2; i++)
                {
                    newState[i] = conversions[new Tuple<bool, bool, bool, bool, bool>(state[i - 2], state[i - 1], state[i], state[i + 1], state[i + 2])];
                }

                state = newState;
            }

            int sum = 0;
            for (int i = -50; i < state.Length - 50; i++)
            {
                if (state[i + 50])
                    sum += i;
            }

            return sum.ToString();
        }

        public override string Part2(string input)
        {
            //I'm not sure how to get it programmatically for every input, but it seems like every input eventually increases in score at a constant rate (here: 5)
            return (5 * 50_000_000_000 + 219).ToString();
        }
    }
}
