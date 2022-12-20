using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions;

public class Year2022Day20 : Solution
{
    private class Node
    {
        public Node next;
        public Node prev;
        public long value;
    }
    
    public override string Part1(string input)
    {
        int[] order = input.Split('\n').Select(int.Parse).ToArray();
        Dictionary<int, Node> lookup = new();
        Node last = new Node
        {
            value = order[0]
        };
        lookup[0] = last;
        for (int i = 1; i < order.Length; i++)
        {
            Node next = new Node
            {
                value = order[i],
                prev = last
            };
            lookup[i] = next;
            last.next = next;
            last = next;
        }

        lookup[0].prev = lookup[order.Length - 1];
        lookup[order.Length - 1].next = lookup[0];

        for (int i = 0; i < order.Length; i++)
        {
            int o = order[i];
            Node n = lookup[i];
            if (o > 0)
            {
                for (int j = 0; j < o; j++)
                {
                    Node originalPrevious = n.prev;
                    n.prev.next = n.next;
                    n.next = n.next.next;
                    n.next.prev = n;
                    n.prev.next.next = n;
                    n.prev = n.prev.next;
                    n.prev.prev = originalPrevious;
                }
            }
            else
            {
                for (int j = 0; j < -o; j++)
                {
                    Node originalNext = n.next;
                    n.next.prev = n.prev;
                    n.prev = n.prev.prev;
                    n.prev.next = n;
                    n.next.prev.prev = n;
                    n.next = n.next.prev;
                    n.next.next = originalNext;
                }
            }
        }

        long sum = 0;
        Node node = lookup[order.FirstIndexOf(0)];
        for (int i = 0; i < 3000; i++)
        {
            node = node.next;
            if (i % 1000 == 999)
                sum += node.value;
        }
        
        return sum.ToString();
    }

    public override string Part2(string input)
    {
        long[] order = input.Split('\n').Select(s => 811589153L * long.Parse(s)).ToArray();
        Dictionary<int, Node> lookup = new();
        Node last = new Node
        {
            value = order[0]
        };
        lookup[0] = last;
        for (int i = 1; i < order.Length; i++)
        {
            Node next = new Node
            {
                value = order[i],
                prev = last
            };
            lookup[i] = next;
            last.next = next;
            last = next;
        }

        lookup[0].prev = lookup[order.Length - 1];
        lookup[order.Length - 1].next = lookup[0];

        for (int _ = 0; _ < 10; _++)
        {
            for (int i = 0; i < order.Length; i++)
            {
                long o = order[i];
                Node n = lookup[i];
                if (o > 0)
                {
                    for (int j = 0; j < o % (order.Length - 1); j++)
                    {
                        Node originalPrevious = n.prev;
                        n.prev.next = n.next;
                        n.next = n.next.next;
                        n.next.prev = n;
                        n.prev.next.next = n;
                        n.prev = n.prev.next;
                        n.prev.prev = originalPrevious;
                    }
                }
                else
                {
                    for (int j = 0; j < -o % (order.Length - 1); j++)
                    {
                        Node originalNext = n.next;
                        n.next.prev = n.prev;
                        n.prev = n.prev.prev;
                        n.prev.next = n;
                        n.next.prev.prev = n;
                        n.next = n.next.prev;
                        n.next.next = originalNext;
                    }
                }
            }
        }

        long sum = 0;
        Node node = lookup[order.FirstIndexOf(0)];
        for (int i = 0; i < 3000; i++)
        {
            node = node.next;
            if (i % 1000 == 999)
                sum += node.value;
        }
        
        return sum.ToString();
    }
}