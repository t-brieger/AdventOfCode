using System;
using System.Linq;

namespace AdventOfCode.Solutions;

public class Year2015Day15 : Solution
{
	private static (long, int) CalculateScoreAndCalories((int cap, int dur, int fla, int tex, int cal)[] ingredients, int[] amounts)
	{
		if (amounts.Length != ingredients.Length)
		{
			int[] tmp = new int[ingredients.Length];
			Array.Copy(amounts, 0, tmp, 0, amounts.Length);
			amounts = tmp;
		}

		int resCap = ingredients.Select((ing, ix) => ing.cap * amounts[ix]).Sum();
		int resDur = ingredients.Select((ing, ix) => ing.dur * amounts[ix]).Sum();
		int resFla = ingredients.Select((ing, ix) => ing.fla * amounts[ix]).Sum();
		int resTex = ingredients.Select((ing, ix) => ing.tex * amounts[ix]).Sum();
		int resCal = ingredients.Select((ing, ix) => ing.cal * amounts[ix]).Sum();

		if (resCap < 0 || resDur < 0 || resFla < 0 || resTex < 0)
			return (0, resCal);

		return (resCap * resDur * resFla * resTex, resCal);
	}
	
	private static void GetBestAmounts((int, int, int, int, int)[] ingredients, ref (long sc, int[] amounts) bestScore, bool p2 = false, int[] prevSteps = null)
	{
		prevSteps ??= new int[0];
		
		if (prevSteps.Length == ingredients.Length)
		{
			(long score, int calories) = CalculateScoreAndCalories(ingredients, prevSteps);
			if (p2 && calories != 500)
				return;
			
			if (score > bestScore.sc)
			{
				int[] copy = new int[prevSteps.Length];
				Array.Copy(prevSteps, 0, copy, 0, prevSteps.Length);
				bestScore = (score, copy);
			}

			return;
		}

		int[] attempt = new int[prevSteps.Length + 1];
		Array.Copy(prevSteps, 0, attempt, 0, prevSteps.Length);

		int tspLeft = 100 - prevSteps.Sum();

		for (int amountCurrent = ingredients.Length - attempt.Length > 0 ? 0 : tspLeft; amountCurrent <= tspLeft; amountCurrent++)
		{
			attempt[^1] = amountCurrent;
			GetBestAmounts(ingredients, ref bestScore, p2, attempt);
		}
	}

	public override string Part1(string input)
	{
		(int, int, int, int, int)[] ingredients = input.Split("\n", StringSplitOptions.RemoveEmptyEntries)
			.Select(line => line.Replace(",", "").Split(' ')).Select(lineArr => (int.Parse(lineArr[2]),
				int.Parse(lineArr[4]), int.Parse(lineArr[6]), int.Parse(lineArr[8]), 0)).ToArray();

		(long sc, int[] amounts) result = (0, new int[] {});
		GetBestAmounts(ingredients, ref result);

		return result.sc.ToString();
	}

	public override string Part2(string input)
	{
		(int, int, int, int, int)[] ingredients = input.Split("\n", StringSplitOptions.RemoveEmptyEntries)
			.Select(line => line.Replace(",", "").Split(' ')).Select(lineArr => (int.Parse(lineArr[2]),
				int.Parse(lineArr[4]), int.Parse(lineArr[6]), int.Parse(lineArr[8]), int.Parse(lineArr[10])))
			.ToArray();

		(long sc, int[] amounts) result = (0, new int[] {});
		GetBestAmounts(ingredients, ref result, true);

		return result.sc.ToString();
	}
}
