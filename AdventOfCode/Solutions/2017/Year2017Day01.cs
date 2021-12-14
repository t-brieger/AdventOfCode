using System.Linq;

namespace AdventOfCode.Solutions;

public class Year2017Day01 : Solution
{
    public override string Part1(string input)
    {
        Node first = new() { value = input[0] };
        Node cur = first;
        for (int i = 1; i < input.Length; i++)
        {
            if (input[i] < '0' || input[i] > '9')
                continue;
            cur.next = new Node { value = input[i] };
            cur = cur.next;
        }

        cur.next = first;
        cur = cur.next.next;

        int sum = 0;
        do
        {
            if (cur.value == cur.next.value)
                sum += cur.value - '0';
            cur = cur.next;
        } while (cur != first.next);

        return sum.ToString();
    }

    public override string Part2(string input)
    {
        char[] c = input.Where(c2 => c2 is >= '0' and <= '9').ToArray();

        int sum = c.Where((t, i) => t == c[(i + c.Length / 2) % c.Length]).Sum(t => t - '0');

        return sum.ToString();
    }

    private class Node
    {
        public Node next;
        public char value;
    }
}