using System.Linq;

namespace AdventOfCode.Solutions
{
    public class Year2017Day01 : Solution
    {
        private class Node
        {
            public char value;
            public Node next;
        }

        public override string Part1(string input)
        {
            Node first = new Node {value = input[0]};
            Node cur = first;
            for (int i = 1; i < input.Length; i++)
            {
                if (input[i] < '0' || input[i] > '9')
                    continue;
                cur.next = new Node {value = input[i]};
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
            char[] c = input.Where(c2 => c2 >= '0' && c2 <= '9').ToArray();

            int sum = 0;
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] == c[(i + c.Length / 2) % c.Length])
                    sum += c[i] - '0';
            }
            
            return sum.ToString();
        }
    }
}