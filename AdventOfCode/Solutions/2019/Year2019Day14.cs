using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions;

public class Year2019Day14 : Solution
{
    private static Dictionary<string, long> FindRequired(Dictionary<string, (int, (int, string)[])> recipes, Dictionary<string, long> wanted, Dictionary<string, long> leftOvers)
    {
        Dictionary<string, long> newWanted = new Dictionary<string, long>();
        foreach (KeyValuePair<string, long> kvp in wanted)
        {
            if (!recipes.ContainsKey(kvp.Key))
            {
                if (newWanted.ContainsKey(kvp.Key))
                    newWanted[kvp.Key] += kvp.Value;
                else
                    newWanted.Add(kvp.Key, kvp.Value);
                continue;
            }

            long actualWanted = kvp.Value;

            if (leftOvers.ContainsKey(kvp.Key))
            {
                long leftOverAmount = leftOvers[kvp.Key];
                if (leftOverAmount <= actualWanted)
                { 
                    actualWanted -= leftOverAmount;
                    leftOvers[kvp.Key] = 0;
                }
                else
                {
                    leftOvers[kvp.Key] -= actualWanted;
                    actualWanted = 0;
                }
            }

            (int, (int, string)[]) recipe = recipes[kvp.Key];

            long amount = (long)Math.Ceiling((double) actualWanted / recipe.Item1);

            if (leftOvers.ContainsKey(kvp.Key))
                leftOvers[kvp.Key] += amount * recipe.Item1 - actualWanted;
            else
                leftOvers[kvp.Key] = amount * recipe.Item1 - actualWanted;
            
            foreach ((int ingredientAmount, string ingredient) in recipe.Item2)
            {
                if (newWanted.ContainsKey(ingredient))
                    newWanted[ingredient] += amount * ingredientAmount;
                else
                    newWanted.Add(ingredient, amount * ingredientAmount);
            }
        }

        return newWanted;
    }

    private static long CalculateOreFromFuel(Dictionary<string, (int, (int, string)[])> recipes, long fuel)
    {
        Dictionary<string, long> left_over = new Dictionary<string, long>();
        Dictionary<string, long> wanted = new Dictionary<string, long>();
        wanted.Add("FUEL", fuel);

        while (wanted.Any(kvp => kvp.Key != "ORE"))
            wanted = FindRequired(recipes, wanted, left_over);

        return wanted["ORE"];
    }
    
    public override string Part1(string input)
    {
        //         name ->  #,   (#,   ingred)[]
        Dictionary<string, (int, (int, string)[])> recipes = new Dictionary<string, (int, (int, string)[])>();
        foreach (string recipe in input.Split('\n'))
        {
            string[] ingredientsAndResult = recipe.Split(" => ");
            string[] result = ingredientsAndResult[1].Split(' ');
            string[][] ingredients = ingredientsAndResult[0].Split(", ").Select(ing => ing.Split(' ')).ToArray();
            
            recipes.Add(result[1], (int.Parse(result[0]), ingredients.Select(ing => (int.Parse(ing[0]), ing[1])).ToArray()));
        }

        return CalculateOreFromFuel(recipes, 1).ToString();
    }

    public override string Part2(string input)
    {
        Dictionary<string, (int, (int, string)[])> recipes = new Dictionary<string, (int, (int, string)[])>();
        foreach (string recipe in input.Split('\n'))
        {
            string[] ingredientsAndResult = recipe.Split(" => ");
            string[] result = ingredientsAndResult[1].Split(' ');
            string[][] ingredients = ingredientsAndResult[0].Split(", ").Select(ing => ing.Split(' ')).ToArray();
            
            recipes.Add(result[1], (int.Parse(result[0]), ingredients.Select(ing => (int.Parse(ing[0]), ing[1])).ToArray()));
        }

        long min = 0;
        // assume 1 fuel >= 1 ore
        long max = 1_000_000_000_000;

        while (true)
        {
            long interval = max - min;
            long guess = min + interval / 2;

            long oreForFuel = CalculateOreFromFuel(recipes, guess);
            long oreForFuel2 = CalculateOreFromFuel(recipes, guess + 1);

            if (oreForFuel <= 1_000_000_000_000 && oreForFuel2 > 1_000_000_000_000)
                return guess.ToString();

            if (oreForFuel < 1_000_000_000_000)
                min = guess;
            else
                max = guess;
        }
    }
}