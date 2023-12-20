using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions;

public class Year2016Day11 : Solution
{
	private static uint changeOneLocationInState(uint prevState, int id, bool isChip, int newLocation)
	{
		// ... Chip1Floor Gen1Floor Chip0Floor Gen0Floor OurFloor
		int dataIndex = 1 + (2 * id) + (isChip ? 1 : 0);
		uint newState = prevState & (0xFF_FF_FF_FF << (2 + 2 * dataIndex));
		newState |= prevState & (0xFF_FF_FF_FF >> (32 - 2 * dataIndex));

		newState |= (uint) (newLocation << (2 * dataIndex));

		return newState;
	}

	private static int CountSteps(string input)
	{
		string[] lines = input.Split('\n');
		Dictionary<string, int> elementToId = new();

		HashSet<(int floor, int type, bool isChip)> devices = [];
		for (int i = 0; i < lines.Length; i++)
		{
			string[] words = lines[i].Split(" ");
			if (words[4] == "nothing")
				continue;
			for (int j = 0; j < words.Length; j++)
			{
				string word = words[j];
				string element = "";
				bool type = false;
				if (word.Contains('-'))
				{
					// x-compatible microchip (we want "x")
					element = word.Split('-')[0];
					type = true;
				}
				else if (word.StartsWith("generator"))
				{
					element = words[j - 1];
				}

				if (element == "") continue;
				
				int id;
				if (elementToId.TryGetValue(element, out int elementId))
					id = elementId;
				else
					id = elementToId[element] = elementToId.Count;

				devices.Add((i, id, type));
			}
		}

		uint state = 0;
		for (int i = 0; i < elementToId.Count; i++)
		{
			state <<= 2;
			state |= (uint) devices.First(d => d.type == i && d.isChip).floor;
			state <<= 2;
			state |= (uint) devices.First(d => d.type == i && !d.isChip).floor;
		}

		state <<= 2;
		// our current floor.
		state |= 0;

		int chipCount = elementToId.Count;

		(uint _, int cost) res = Util.Djikstra(state, (st, cost) =>
		{
			uint originalState = st;

			int[] chipFloors = new int[chipCount];
			int[] genFloors = new int[chipCount];
			uint ourFloor = st & 3;

			List<(bool isChip, int id)> onOurFloor = [];

			for (int i = 0; i < chipCount; i++)
			{
				st >>= 2;
				genFloors[i] = (int) (st & 3);
				st >>= 2;
				chipFloors[i] = (int) (st & 3);

				if (genFloors[i] == ourFloor)
					onOurFloor.Add((false, i));
				if (chipFloors[i] == ourFloor)
					onOurFloor.Add((true, i));
			}

			// check for chips that are in the same room as a (non-compatible) generator without their own generator
			// nearby.
			for (int i = 0; i < chipCount; i++)
			{
				if (chipFloors[i] == genFloors[i])
					continue;
				bool invalid = false;
				for (int j = 0; j < chipCount; j++)
				{
					if (chipFloors[i] == genFloors[j])
					{
						invalid = true;
						break;
					}
				}

				if (invalid)
					return new (uint, int)[] { };
			}

			int amountOnOurFloor = onOurFloor.Count;
			List<(uint, int)> ret = new(amountOnOurFloor * (amountOnOurFloor - 1) + amountOnOurFloor * 2);

			foreach ((bool c, int id)[] elevatorContent in Util.GetPermutations(onOurFloor))
			{
				(bool c, int id, int currentFloor) d1 = (elevatorContent[0].c, elevatorContent[0].id,
					(elevatorContent[0].c ? chipFloors : genFloors)[elevatorContent[0].id]);
				(bool c, int id, int currentFloor) d2 = (elevatorContent[1].c, elevatorContent[1].id,
					(elevatorContent[1].c ? chipFloors : genFloors)[elevatorContent[1].id]);

				uint newState1 = changeOneLocationInState(originalState, d1.id, d1.c, d1.currentFloor + 1);
				newState1 = changeOneLocationInState(newState1, d2.id, d2.c, d2.currentFloor + 1);
				newState1 ^= ourFloor;
				newState1 |= ourFloor + 1;
				uint newState2 = changeOneLocationInState(originalState, d1.id, d1.c, d1.currentFloor - 1);
				newState2 = changeOneLocationInState(newState2, d2.id, d2.c, d2.currentFloor - 1);
				newState2 ^= ourFloor;
				newState2 |= ourFloor - 1;

				if (ourFloor != 3)
					ret.Add((newState1, cost + 1));
				if (ourFloor != 0)
					ret.Add((newState2, cost + 1));
			}

			foreach ((bool c, int id) elevatorContent in onOurFloor)
			{
				(bool c, int id, int currentFloor) cont = (elevatorContent.c, elevatorContent.id,
					(elevatorContent.c ? chipFloors : genFloors)[elevatorContent.id]);

				uint newState1 = changeOneLocationInState(originalState, cont.id, cont.c, cont.currentFloor + 1);
				newState1 ^= ourFloor;
				newState1 |= ourFloor + 1;
				uint newState2 = changeOneLocationInState(originalState, cont.id, cont.c, cont.currentFloor - 1);
				newState2 ^= ourFloor;
				newState2 |= ourFloor - 1;

				if (ourFloor != 3)
					ret.Add((newState1, cost + 1));
				if (ourFloor != 0)
					ret.Add((newState2, cost + 1));
			}

			return ret;
		}, st =>
		{
			bool allOnTopFloor = true;
			for (int i = 0; i < chipCount; i++)
			{
				if (((st >>= 2) & 3) != 3)
				{
					allOnTopFloor = false;
					break;
				}
				if (((st >>= 2) & 3) != 3)
				{
					allOnTopFloor = false;
					break;
				}
			}

			return allOnTopFloor;
		});

		return res.cost;
	}

	public override string Part1(string input)
	{
		return CountSteps(input).ToString();
	}

	public override string Part2(string input)
	{
		string[] lines = input.Split('\n');
		lines[0] += " elerium generator elerium-compatible dilithium generator dilithium-compatible";

		return CountSteps(string.Join("\n", lines)).ToString();
	}
}
