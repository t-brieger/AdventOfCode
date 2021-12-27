using System.Formats.Asn1;

namespace AdventOfCode.Solutions;

public class Year2021Day23 : Solution
{
    private static uint Encode(int[] state)
    {
        for (int i = 0; i < 8; i += 2)
        {
            if (state[i] < state[i + 1])
                continue;
            int _ = state[i];
            state[i] = Math.Min(state[i], state[i + 1]);
            state[i + 1] = Math.Max(_, state[i + 1]);
        }
        List<int> l = new(19);
        for (int i = 0; i < 19; i++)
            l.Add(-1);
        for (int i = 0; i < 8; i++)
            l[state[i]] = i;

        uint result = 0;
        for (int i = 0; i < 8; i++)
        {
            result *= 18 - (uint)i + 1;
            result += (uint)l.IndexOf(i);
            l.Remove(i);
        }

        return result;
    }

    private static int[] Decode(uint state)
    {
        List<int> result = new(19);
        for (int i = 0; i < 11; i++)
            result.Add(-1);
        for (int i = 7; i >= 0; i--)
        {
            result.Insert((int)(state % (18 - i + 1)), i);
            state = (uint)(state / (18 - i + 1));
        }

        int[] output = new int[8];
        for (int i = 0; i < 8; i++)
            output[i] = result.IndexOf(i);

        return output;
    }

    public override string Part1(string input)
    {
        PriorityQueue<(uint state, int weight), int> queue = new();
        input = input.Replace(" ", "").Replace("\n", "").Replace("#", "");
        queue.Enqueue((Encode(new[]
            {
                input.IndexOf('A'), input.LastIndexOf('A'),
                input.IndexOf('B'), input.LastIndexOf('B'),
                input.IndexOf('C'), input.LastIndexOf('C'),
                input.IndexOf('D'), input.LastIndexOf('D')
            }), 0), 0);

        HashSet<uint> seenStates = new HashSet<uint>();

        while (true)
        {
            (uint state, int weight) = queue.Dequeue();
            if (seenStates.Contains(state))
                continue;
            seenStates.Add(state);
            if (seenStates.Count % 1000000 == 0)
                Console.WriteLine(seenStates.Count / 1000000);
            int[] decodedState = Decode(state);

            for (int i = 0; i < 8; i++)
            {
                if (decodedState[i] != 11 + i / 2 && decodedState[i] != 15 + i / 2)
                    break;
                if (i == 7)
                    return weight.ToString();
            }
            
            for (int i = 0; i < 8; i++)
            {
                int[] decodedStateCopy = new int[8];
                decodedState.CopyTo(decodedStateCopy, 0);
                int moveWeight = i / 2 == 0 ? 1 : i / 2 == 1 ? 10 : i / 2 == 2 ? 100 : 1000;
                // move into matching hole
                if (decodedState[i] == i / 2 * 2 + 2 && !decodedState.Contains(11 + i / 2))
                {
                    decodedStateCopy[i] = 11 + i / 2;
                    queue.Enqueue((Encode(decodedStateCopy), weight + moveWeight), weight + moveWeight);
                    decodedState.CopyTo(decodedStateCopy, 0);
                }
                // move right in top row
                if (decodedState[i] is >= 0 and < 10 && !decodedState.Contains(decodedState[i] + 1))
                {
                    decodedStateCopy[i]++;
                    queue.Enqueue((Encode(decodedStateCopy), weight + moveWeight), weight + moveWeight);
                    decodedState.CopyTo(decodedStateCopy, 0);
                }
                // move left in top row
                if (decodedState[i] is > 0 and <= 10 && !decodedState.Contains(decodedState[i] - 1))
                {
                    decodedStateCopy[i]--;
                    queue.Enqueue((Encode(decodedStateCopy), weight + moveWeight), weight + moveWeight);
                    decodedState.CopyTo(decodedStateCopy, 0);
                }
                // move down in hole
                if (decodedState[i] is > 10 and <= 14 && !decodedState.Contains(decodedState[i] + 4))
                {
                    decodedStateCopy[i] += 4;
                    queue.Enqueue((Encode(decodedStateCopy), weight + moveWeight), weight + moveWeight);
                    decodedState.CopyTo(decodedStateCopy, 0);
                }
                // move up in hole (from bottom to middle)
                if (decodedState[i] > 14 && !decodedState.Contains(decodedState[i] - 4))
                {
                    decodedStateCopy[i] -= 4;
                    queue.Enqueue((Encode(decodedStateCopy), weight + moveWeight), weight + moveWeight);
                    decodedState.CopyTo(decodedStateCopy, 0);
                }
                // move up in hole (from middle to top)
                if (decodedState[i] is > 10 and <= 14 && !decodedState.Contains((decodedState[i] - 11) * 2 + 2))
                {
                    decodedStateCopy[i] = (decodedState[i] - 11) * 2 + 2;
                    queue.Enqueue((Encode(decodedStateCopy), weight + moveWeight), weight + moveWeight);
                }
            }
        }
    }

    public override string Part2(string input)
    {
        throw new NotImplementedException();
    }
}