using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace AdventOfCode.Solutions
{
    public class Year2018Day09 : Solution
    {
        private static LinkedListNode<T> Next<T>(LinkedListNode<T> node, LinkedList<T> list)
        {
            return node.Next ?? list.First;
        }
        private static LinkedListNode<T> Prev<T>(LinkedListNode<T> node, LinkedList<T> list)
        {
            return node.Previous ?? list.Last;
        }

        public override string Part1(string input)
        {
            string[] words = input.Split(' ');
            int players = int.Parse(words[0]);
            int lastMarble = int.Parse(words[6]);

            int[] scores = new int[players];
            LinkedList<int> marbles = new LinkedList<int>();
            marbles.AddLast(0);
            marbles.AddLast(1);
            LinkedListNode<int> current = marbles.Last;

            int currentPlayer = 2;
            for (int i = 2; i <= lastMarble; i++)
            {
                currentPlayer++;
                currentPlayer %= players;
                if (i % 23 == 0)
                {
                    scores[currentPlayer] += i;
                    current = Prev(
                        Prev(Prev(Prev(Prev(Prev(Prev(current, marbles), marbles), marbles), marbles), marbles),
                            marbles), marbles);

                    scores[currentPlayer] += current.Value;


                    current = Next(current, marbles);
                    marbles.Remove(Prev(current, marbles));
                    continue;
                }

                current = Next(current, marbles);
                current = marbles.AddAfter(current, i);
            }

            return scores.Max().ToString();
        }

        public override string Part2(string input)
        {
            string[] words = input.Split(' ');
            int players = int.Parse(words[0]);
            int lastMarble = int.Parse(words[6]) * 100;

            long[] scores = new long[players];
            LinkedList<int> marbles = new LinkedList<int>();
            marbles.AddLast(0);
            marbles.AddLast(1);
            LinkedListNode<int> current = marbles.Last;

            int currentPlayer = 2;
            for (int i = 2; i <= lastMarble; i++)
            {
                currentPlayer++;
                currentPlayer %= players;
                if (i % 23 == 0)
                {
                    scores[currentPlayer] += i;
                    current = Prev(
                        Prev(Prev(Prev(Prev(Prev(Prev(current, marbles), marbles), marbles), marbles), marbles),
                            marbles), marbles);

                    scores[currentPlayer] += current.Value;


                    current = Next(current, marbles);
                    marbles.Remove(Prev(current, marbles));
                    continue;
                }

                current = Next(current, marbles);
                current = marbles.AddAfter(current, i);
            }

            return scores.Max().ToString();
        }
    }
}