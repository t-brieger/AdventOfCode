using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions
{
    public class Year2015Day22 : Solution
    {
        private class GameState
        {
            // true/false -> player/boss, null -> no winner yet
            public bool? playerWinner;

            // 0-2: shield, poison, recharge
            public byte[] effectDurations;
            public int bossHp;
            public byte bossAttack;

            public int playerHp;
            public int currentPlayerMana;

            public int manaSpent;

            public GameState Clone() => new()
            {
                playerWinner = this.playerWinner, effectDurations = this.effectDurations[..3], bossHp = this.bossHp,
                bossAttack = this.bossAttack, playerHp = this.playerHp, currentPlayerMana = this.currentPlayerMana,
                manaSpent = this.manaSpent
            };
        }

        private static IEnumerable<GameState> GetAllNexts(GameState gs, bool p2 = false)
        {
            GameState mm = gs.Clone();
            mm.bossHp -= 4;
            mm.currentPlayerMana -= 53;
            mm.manaSpent += 53;
            GameState drain = gs.Clone();
            drain.bossHp -= 2;
            drain.playerHp += 2;
            drain.currentPlayerMana -= 73;
            drain.manaSpent += 73;
            GameState shield = gs.Clone();
            shield.effectDurations[0] = 6;
            shield.currentPlayerMana -= 113;
            shield.manaSpent += 113;
            GameState poison = gs.Clone();
            poison.effectDurations[1] = 6;
            poison.currentPlayerMana -= 173;
            poison.manaSpent += 173;
            GameState recharge = gs.Clone();
            recharge.effectDurations[2] = 5;
            recharge.currentPlayerMana -= 229;
            recharge.manaSpent += 229;

            if (mm.currentPlayerMana >= 0)
            {
                AdvanceOneRound(mm, p2);
                yield return mm;
            }
            if (drain.currentPlayerMana >= 0)
            {
                AdvanceOneRound(drain, p2);
                yield return drain;
            }
            if (shield.currentPlayerMana >= 0 && gs.effectDurations[0] == 0)
            {
                AdvanceOneRound(shield, p2);
                yield return shield;
            }
            if (poison.currentPlayerMana >= 0 && gs.effectDurations[1] == 0)
            {
                AdvanceOneRound(poison, p2);
                yield return poison;
            }
            if (recharge.currentPlayerMana >= 0 && gs.effectDurations[2] == 0)
            {
                AdvanceOneRound(recharge, p2);
                yield return recharge;
            }
        }

        private static void AdvanceOneRound(GameState gs, bool p2 = false)
        {
            // this method gets called AFTER spell cast.
            if (gs.effectDurations[1] > 0)
            {
                gs.effectDurations[1]--;
                gs.bossHp -= 3;
            }

            if (gs.bossHp <= 0)
            {
                gs.playerWinner = true;
                return;
            }
            
            if (gs.effectDurations[0] > 0)
            {
                gs.effectDurations[0]--;
                if (gs.bossAttack <= 7)
                    gs.playerHp--;
                else
                    gs.playerHp -= gs.bossAttack - 7;
            }
            else
                gs.playerHp -= gs.bossAttack;

            if (gs.playerHp <= 0)
            {
                gs.playerWinner = false;
                return;
            }

            if (gs.effectDurations[2] > 0)
            {
                gs.effectDurations[2]--;
                gs.currentPlayerMana += 101;
            }
            
            // - end of boss' turn, start of player's (just apply effects again)

            if (p2)
            {
                gs.playerHp--;
                if (gs.playerHp <= 0)
                {
                    gs.playerWinner = false;
                    return;
                }
            }
            
            if (gs.effectDurations[0] > 0)
                gs.effectDurations[0]--;

            if (gs.effectDurations[1] > 0)
            {
                gs.effectDurations[1]--;
                gs.bossHp -= 3;
            }

            if (gs.effectDurations[2] > 0)
            {
                gs.effectDurations[2]--;
                gs.currentPlayerMana += 101;
            }


            if (gs.bossHp <= 0)
                gs.playerWinner = true;
        }

        public override string Part1(string input)
        {
            const int playerHp = 50;
            const int playerMana = 500;
            byte[] bossStats = input.Split('\n').Select(l => byte.Parse(l.Split(": ")[1])).ToArray();

            PriorityQueue<GameState, int> states = new();
            states.Enqueue(
                new GameState
                {
                    playerWinner = null, effectDurations = new byte[] { 0, 0, 0 }, bossHp = bossStats[0],
                    bossAttack = bossStats[1], playerHp = playerHp, currentPlayerMana = playerMana
                }, 0);

            // downsides of using nullable bools; doing !x is bad unless the value is guaranteed non-null
            while (states.Peek().playerWinner != true)
            {
                GameState current = states.Dequeue();
                if (current.playerWinner == false)
                    continue;
                IEnumerable<GameState> nexts = GetAllNexts(current);
                states.EnqueueRange(nexts.Select(e => (e, e.manaSpent)));
            }

            return states.Peek().manaSpent.ToString();
        }

        public override string Part2(string input)
        {
            const int playerHp = 49;
            const int playerMana = 500;
            byte[] bossStats = input.Split('\n').Select(l => byte.Parse(l.Split(": ")[1])).ToArray();

            PriorityQueue<GameState, int> states = new();
            states.Enqueue(
                new GameState
                {
                    playerWinner = null, effectDurations = new byte[] { 0, 0, 0 }, bossHp = bossStats[0],
                    bossAttack = bossStats[1], playerHp = playerHp, currentPlayerMana = playerMana
                }, 0);

            // downsides of using nullable bools; doing !x is bad unless the value is guaranteed non-null
            while (states.Peek().playerWinner != true)
            {
                GameState current = states.Dequeue();
                if (current.playerWinner == false)
                    continue;
                IEnumerable<GameState> nexts = GetAllNexts(current, true);
                states.EnqueueRange(nexts.Select(e => (e, e.manaSpent)));
            }

            return states.Peek().manaSpent.ToString();
        }
    }
}