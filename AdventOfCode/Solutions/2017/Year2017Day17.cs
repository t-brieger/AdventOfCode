using System;

namespace AdventOfCode.Solutions
{
    public class Year2017Day17 : Solution
    {
        public override string Part1(string input)
        {
            Node<int> head = new(0);
            head.next = head;

            int skipAmount = Int32.Parse(input);

            for (int i = 1; i <= 2017; i++)
            {
                for (int j = 0; j < skipAmount; j++) head = head.next;
                Node<int> newNode = new(i) { next = head.next };
                head.next = newNode;
                head = newNode;
            }

            while (true)
            {
                if (head.value == 2017)
                    return head.next.value.ToString();
                head = head.next;
            }
        }

        public override string Part2(string input)
        {
            int after0 = 0;
            int i = 0;

            int step = Int32.Parse(input);

            for (int listLength = 1; listLength < 50_000_000; listLength++)
            {
                i = (i + step) % listLength + 1;
                if (i == 1)
                    after0 = listLength;
            }

            return after0.ToString();
        }

        private class Node<T>
        {
            public readonly T value;
            public Node<T> next;

            public Node(T val)
            {
                this.value = val;
            }
        }
    }
}