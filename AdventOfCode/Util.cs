using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace AdventOfCode;

// ReSharper disable once InconsistentNaming
public static class Util
{
	// https://stackoverflow.com/a/29717490
	public static long Lcm(params long[] numbers)
	{
		return numbers.Aggregate(lcm);
	}

	private static long lcm(long a, long b)
	{
		return Math.Abs(a * b) / Gcd(a, b);
	}

	// https://stackoverflow.com/a/41766138
	public static long Gcd(long a, long b)
	{
		while (true)
		{
			if ((a == 0) ^ (b == 0)) return Math.Abs(a + b);
			if (a == 0 && b == 0) return 1;

			if (Math.Sign(a) != Math.Sign(b))
			{
				a *= -1;
				continue;
			}

			if (a < 0 && b < 0)
			{
				a *= -1;
				b *= -1;
			}

			while (a != 0 && b != 0)
				if (a > b)
					a %= b;
				else
					b %= a;

			return a | b;
		}
	}

	// https://stackoverflow.com/a/10629938
	[SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
	public static IEnumerable<T[]> GetPermutations<T>(IEnumerable<T> list, int length = 2) where T : IComparable
	{
		if (length == 1) return list.Select(t => new[] {t});
		return GetPermutations(list, length - 1)
			.SelectMany(t => list.Where(o => !t.Contains(o)),
				(t1, t2) => t1.Concat(new[] {t2}).ToArray());
	}

	public static string Reverse(this string s)
	{
		char[] charArray = s.ToCharArray();
		Array.Reverse(charArray);
		return new string(charArray);
	}

	public static int FirstIndexOf<T>(this IEnumerable<T> e, Func<T, bool> f)
	{
		int i = 0;
		foreach (T t in e)
		{
			if (f(t))
				return i;
			i++;
		}

		return -1;
	}

	public static int FirstIndexOf<T>(this IEnumerable<T> e, T what)
	{
		return e.FirstIndexOf(t => t.Equals(what));
	}

	public static (TState, int) Djikstra<TState>(TState initial,
		Func<TState, int, IEnumerable<(TState, int)>> generateReachableStates, Func<TState, bool> isGoal,
		bool skipSeenStates = true)
	{
		HashSet<TState> seen = [];
		PriorityQueue<(TState, int), int> states = new PriorityQueue<(TState, int), int>();
		states.Enqueue((initial, 0), 0);

		while (states.Count > 0)
		{
			(TState currentState, int weight) = states.Dequeue();
			if (skipSeenStates)
				if (!seen.Add(currentState))
					continue;

			if (isGoal(currentState))
				return (currentState, weight);

			foreach ((TState, int) s in generateReachableStates(currentState, weight))
			{
				states.Enqueue(s, s.Item2);
			}
		}

		// would be a little clearer to return "null" here instead of the initial state, but alas, it seems not to be
		// possible to cast null to State (or even to "State?")
		return (initial, -1);
	}

	public static (TState, int) Djikstra<TState>(TState initial,
		Func<TState, int, IEnumerable<(TState, int)>> generateReachable, TState singleGoal, bool skipSeen = true)
	{
		return Djikstra(initial, generateReachable, candidate => singleGoal.Equals(candidate), skipSeen);
	}

	public static HashSet<TState> FloodFill<TState>(IEnumerable<TState> locations,
		Func<TState, IEnumerable<TState>> transitions)
	{
		HashSet<TState> seen = [];
		HashSet<TState> locationsHs = [..locations];
		while (locationsHs.Any())
		{
			TState curr = locationsHs.First();
			locationsHs.Remove(curr);
			if (!seen.Add(curr))
				continue;

			foreach (TState next in transitions(curr))
				locationsHs.Add(next);
		}

		return seen;
	}
}

// Start is included, End isn't.
public struct Interval(long start, long end)
{
	public readonly long Start = start, End = end;

	public long Length => End - Start;

	public static (Interval l, Interval r) operator -(Interval a, Interval b)
	{
		long startIntersect = Math.Max(a.Start, b.Start);
		long endIntersect = Math.Min(a.End, b.End);

		return (new Interval(a.Start, Math.Max(startIntersect, a.Start)),
			new Interval(Math.Min(endIntersect, a.End), a.End));
	}
	
	// this is just an intersect operation.
	public static Interval operator *(Interval a, Interval b)
	{
		long startIntersect = Math.Max(a.Start, b.Start);
		long endIntersect = Math.Min(a.End, b.End);

		return new Interval(startIntersect, endIntersect);
	}

	public override string ToString() => $"[{Start}-{End}]";
}
