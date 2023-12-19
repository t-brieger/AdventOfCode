using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions;

public class Year2023Day19 : Solution
{
	public override string Part1(string input)
	{
		string[] inputSplit = input.Split("\n\n");

		string inputRules = inputSplit[0];
		string inputParts = inputSplit[1];

		Dictionary<string, List<(char, char, int, string)>> rules = new();
		foreach (string rule in inputRules.Split("\n"))
		{
			string[] split = rule.Replace("}", "").Split('{', ',');
			string ruleName = split[0];

			List<(char, char, int, string)> transfers = new();
			foreach (string transfer in split[1..])
			{
				if (!transfer.Contains(':'))
				{
					transfers.Add(('\0', '\0', 0, transfer));
					continue;
				}

				string[] splitAgain = transfer.Split('<', '>', ':');
				transfers.Add((transfer[0], transfer[1], int.Parse(splitAgain[^2]), splitAgain[^1]));
			}

			rules[ruleName] = transfers;
		}

		long sumAccepted = 0;

		foreach (string partString in inputParts.Split('\n'))
		{
			string[] parse = partString.Replace("{", "").Replace("}", "").Split(',');

			Dictionary<char, int> parameters = new();
			foreach (string parameter in parse)
				parameters[parameter[0]] = int.Parse(parameter[2..]);

			string workflow = "in";
			while (workflow is not "A" and not "R")
			{
				List<(char, char, int, string)> wf = rules[workflow];
				foreach ((char conditionVar, char conditionType, int val, string next) in wf)
				{
					if ((conditionType == '<' && parameters[conditionVar] < val) ||
					    (conditionType == '>' && parameters[conditionVar] > val) ||
					    conditionVar == '\0')
					{
						workflow = next;
						break;
					}
				}
			}

			if (workflow == "A")
				sumAccepted += parameters.Sum(kvp => kvp.Value);
		}

		return sumAccepted.ToString();
	}

	public override string Part2(string input)
	{
		string[] inputSplit = input.Split("\n\n");

		string inputRules = inputSplit[0];

		Dictionary<string, List<(int, char, int, string)>> rules = new();
		foreach (string rule in inputRules.Split("\n"))
		{
			string[] split = rule.Replace("}", "").Split('{', ',');
			string ruleName = split[0];

			List<(int, char, int, string)> transfers = [];
			foreach (string transfer in split[1..])
			{
				if (!transfer.Contains(':'))
				{
					transfers.Add((-1, '\0', 0, transfer));
					continue;
				}

				string[] splitAgain = transfer.Split('<', '>', ':');
				int propertyIx = transfer[0] switch
				{
					'x' => 0,
					'm' => 1,
					'a' => 2,
					's' => 3,
					_ => throw new ArgumentException($"\"{transfer[0]}\" is not a recognised property.")
				};
				transfers.Add((propertyIx, transfer[1], int.Parse(splitAgain[^2]), splitAgain[^1]));
			}

			rules[ruleName] = transfers;
		}

		List<(string, int, Interval[])> workflows =
		[
			("in", 0,
				new[] {new Interval(1, 4001), new Interval(1, 4001), new Interval(1, 4001), new Interval(1, 4001)})
		];

		long partsAccepted = 0;
		while (workflows.Count > 0)
		{
			(string name, int ruleIndex, Interval[] which) workflow = workflows[^1];
			workflows.RemoveAt(workflows.Count - 1);

			if (workflow.name == "A")
			{
				partsAccepted += workflow.which.Aggregate(1L, (agg, i) => agg * i.Length);
				continue;
			}

			if (workflow.name == "R")
				continue;

			List<(int, char, int, string)> rulesSet = rules[workflow.name];
			(int what, char op, int threshold, string next) = rulesSet[workflow.ruleIndex];

			if (what == -1)
			{
				workflows.Add((next, 0, workflow.which));
				continue;
			}

			Interval meetsCondition = op == '<' ? new Interval(1, threshold) : new Interval(threshold + 1, 4001);
			meetsCondition *= workflow.which[what];
			Interval[] newWhich = new Interval[4];
			for (int i = 0; i < 4; i++)
				newWhich[i] = i == what ? meetsCondition : workflow.which[i];
			if (meetsCondition.Length > 0)
				workflows.Add((next, 0, newWhich));

			(Interval l, Interval r) = workflow.which[what] - meetsCondition;

			newWhich = new Interval[4];
			for (int i = 0; i < 4; i++)
				newWhich[i] = i == what ? l : workflow.which[i];
			if (l.Length > 0)
				workflows.Add((workflow.name, workflow.ruleIndex + 1, newWhich));

			newWhich = new Interval[4];
			for (int i = 0; i < 4; i++)
				newWhich[i] = i == what ? r : workflow.which[i];
			if (r.Length > 0)
				workflows.Add((workflow.name, workflow.ruleIndex + 1, newWhich));
		}

		return partsAccepted.ToString();
	}
}
