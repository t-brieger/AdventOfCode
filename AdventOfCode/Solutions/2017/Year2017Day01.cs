using System.Collections.Generic;

namespace AdventOfCode.Solutions._2017
{
    public class Year2017Day01 : Solution
    {
        public override string Part1(string input)
        {
            LinkedList<char> numbers = new LinkedList<char>(input);

            LinkedListNode<char> currentNumber = numbers.First;

            int sum = 0;
            for (int _ = 0; _ < numbers.Count; _++)
            {
                if (currentNumber.Value == (currentNumber.Next ?? numbers.First).Value)
                    sum += currentNumber.Value - 0x30;

                currentNumber = currentNumber.Next;
            }

            return sum.ToString();
        }

        public override string Part2(string input)
        {
            char[] numbers = input.ToCharArray();

            int sum = 0;

            for (int i = 0; i < numbers.Length; i++)
            {
                if (numbers[i] == numbers[(i + numbers.Length / 2) % numbers.Length])
                    sum += numbers[i] - 0x30;
            }

            return sum.ToString();
        }
    }
}