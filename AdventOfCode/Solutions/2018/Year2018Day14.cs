using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode.Solutions
{
    public class Year2018Day14 : Solution
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
            /*
            input = "2018";
            //*/

            int warmup = int.Parse(input);

            LinkedList<byte> recipes = new LinkedList<byte>();

            recipes.AddLast(3);
            recipes.AddLast(7);

            LinkedListNode<byte> firstElf = recipes.First;
            LinkedListNode<byte> secondElf = recipes.Last;

            while (recipes.Count < warmup + 10)
            {
                byte combinedValue = (byte)(firstElf.Value + secondElf.Value);
                if (combinedValue > 9)
                    recipes.AddLast(1);
                recipes.AddLast((byte) (combinedValue % 10));

                byte oldValue = firstElf.Value;
                for (byte b = 0; b < oldValue + 1; b++)
                    firstElf = Next(firstElf, recipes);
                oldValue = secondElf.Value;
                for (byte b = 0; b < oldValue + 1; b++)
                    secondElf = Next(secondElf, recipes);
            }

            if (recipes.Count > warmup + 10)
                recipes.RemoveLast();

            StringBuilder output = new StringBuilder();
            foreach (byte b in recipes.Skip(warmup))
            {
                output.Append(b.ToString());
            }
            return output.ToString();
        }

        public override string Part2(string input)
        {
            /*
            input = "293801";
            //*/

            int target = int.Parse(input);

            LinkedList<byte> recipes = new LinkedList<byte>();

            recipes.AddLast(3);
            recipes.AddLast(7);

            LinkedListNode<byte> firstElf = recipes.First;
            LinkedListNode<byte> secondElf = recipes.Last;

            int recipesMatching = 0;

            while (recipesMatching != input.Length)
            {
                byte combinedValue = (byte)(firstElf.Value + secondElf.Value);
                if (combinedValue > 9)
                {
                    recipes.AddLast(1);

                    if (input[recipesMatching] == '1')
                        recipesMatching++;
                    else
                        recipesMatching = input[0] == '1' ? 1 : 0;

                    if (recipesMatching == input.Length)
                        break;
                }

                if (input[recipesMatching] == (combinedValue % 10).ToString()[0])
                    recipesMatching++;
                else
                    recipesMatching = input[0] == (combinedValue % 10).ToString()[0] ? 1 : 0;

                recipes.AddLast((byte)(combinedValue % 10));

                byte oldValue = firstElf.Value;
                for (byte b = 0; b < oldValue + 1; b++)
                    firstElf = Next(firstElf, recipes);
                oldValue = secondElf.Value;
                for (byte b = 0; b < oldValue + 1; b++)
                    secondElf = Next(secondElf, recipes);
            }

            return (recipes.Count - input.Length).ToString();
        }
    }
}