using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using EuroManager.Common;
using EuroManager.MatchSimulator.Domain.Actions;

namespace EuroManager.MatchSimulator.Domain
{
    public class MatchRandomizer : IMatchRandomizer
    {
        private IRandomGenerator random;

        static MatchRandomizer()
        {
            ResetCurrent();
        }

        public MatchRandomizer(IRandomGenerator random)
        {
            Contract.Requires(random != null);

            this.random = random;
        }

        public static IMatchRandomizer Current { get; set; }

        public static void ResetCurrent()
        {
            Current = new MatchRandomizer(RandomGenerator.Current);
        }

        public int ExtendedTime()
        {
            int[] lengths = { 1, 2, 3, 4, 5, 6 };
            return random.Choose(lengths, 0.05, 0.34, 0.34, 0.20, 0.05, 0.02);
        }

        public Team AttackingTeam(Team team1, Team team2, bool isNeutralGround)
        {
            double firstTeamChance = isNeutralGround ? 0.5 : 0.6;
            return random.Decide(firstTeamChance) ? team1 : team2;
        }

        public virtual bool IsFirstTeamStartingPenaltyShootout()
        {
            return random.Decide(0.5);
        }
            
        public virtual bool TryPass(double passing, double positioning, double marking)
        {
            double attack = 0.5 * (passing + positioning);
            attack = EnhanceSkill(attack, marking);

            return Match(3 * attack, marking);
        }

        public virtual bool TryDribble(double dribbling, double tackling)
        {
            dribbling = EnhanceSkill(dribbling, tackling);
            return Match(dribbling, tackling);
        }

        public virtual ShotResult TryShoot(double shooting, double blocking, double goalkeeping)
        {
            shooting = EnhanceSkill(shooting, blocking);
            double adjustedGoalkeeping = 0.3 * goalkeeping + 0.7 * blocking;

            if (Match(0.1 * blocking, shooting))
            {
                return ShotResult.Blocked;
            }
            else if (Match(blocking, shooting))
            {
                return ShotResult.Missed;
            }
            else if (Match(1.6 * adjustedGoalkeeping, shooting))
            {
                return ShotResult.Saved;
            }
            else
            {
                return ShotResult.Scored;
            }
        }

        public virtual bool TryPenaltyKick()
        {
            return random.Decide(0.75);
        }

        public Player PlayerInitiatingAttack(IEnumerable<Player> players, double[] chances)
        {
            return random.Choose(players, chances);
        }

        public Player PlayerReceivingPass(IEnumerable<Player> players, double[] chances)
        {
            return random.Choose(players, chances);
        }

        public Player PlayerMarkingPassReceiver(IEnumerable<Player> players, double[] chances)
        {
            return random.Choose(players, chances);
        }

        public Player PlayerConfrontingDribbler(IEnumerable<Player> players, double[] chances)
        {
            return random.Choose(players, chances);
        }

        public Player PlayerConfrontingShooter(IEnumerable<Player> players, double[] chances)
        {
            return random.Choose(players, chances);
        }

        public IMatchAction NextAction(int level, Player player)
        {
            Func<IMatchAction>[] actions =
            {
                () => new PassAction(),
                () => new DribbleAction(),
                () => new ShootAction()
            };

            if (player.Position.Location == Location.Goal)
            {
                return new PassAction();
            }
            else if (player.Position.Location == Location.Back)
            {
                switch (level)
                {
                    case 1: return random.Choose(actions, 0.90, 0.10, 0)();
                    case 2: return random.Choose(actions, 0.85, 0.10, 0.05)();
                    case 3: return random.Choose(actions, 0.75, 0.10, 0.15)();
                    case 4: return random.Decide(0.1) ? (IMatchAction)new ShootAction() : new NullAction();
                    default: return new NullAction();
                }
            }
            else if (player.Position.Location == Location.Defensive)
            {
                switch (level)
                {
                    case 1: return random.Choose(actions, 0.80, 0.20, 0)();
                    case 2: return random.Choose(actions, 0.80, 0.15, 0.05)();
                    case 3: return random.Choose(actions, 0.70, 0.15, 0.15)();
                    case 4: return random.Decide(0.1) ? (IMatchAction)new ShootAction() : new NullAction();
                    default: return new NullAction();
                }
            }
            else if (player.Position.Location == Location.Forward)
            {
                switch (level)
                {
                    case 1: return random.Choose(actions, 0.50, 0.50, 0)();
                    case 2: return random.Choose(actions, 0.35, 0.55, 0.10)();
                    case 3: return random.Choose(actions, 0.35, 0.45, 0.20)();
                    case 4: return random.Decide(0.1) ? (IMatchAction)new ShootAction() : new NullAction();
                    default: return new NullAction();
                }
            }
            else
            {
                switch (level)
                {
                    case 1: return random.Choose(actions, 0.67, 0.33, 0)();
                    case 2: return random.Choose(actions, 0.65, 0.30, 0.05)();
                    case 3: return random.Choose(actions, 0.60, 0.25, 0.15)();
                    case 4: return random.Decide(0.1) ? (IMatchAction)new ShootAction() : new NullAction();
                    default: return new NullAction();
                }
            }
        }

        private double EnhanceSkill(double skill, double opponent)
        {
            return skill + 1.25 * (skill - opponent);
        }
            
        private bool Match(double skill1, double skill2)
        {
            return random.Decide(skill1 / (skill1 + skill2));
        }
    }
}
