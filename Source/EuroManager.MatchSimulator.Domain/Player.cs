using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using EuroManager.Common;
using EuroManager.Common.Domain;

namespace EuroManager.MatchSimulator.Domain
{
    public class Player
    {
        public static readonly double InitialRating = 0.55;

        #region InitiateAttackChanceByPosition
        private static readonly Dictionary<Position, double> initiateAttackChanceByPosition = new Dictionary<Position, double>
        {
            { Position.Goalkeeper, 1 },
            { Position.RightWingBack, 4 },
            { Position.RightBack, 3 },
            { Position.RightCenterBack, 2 },
            { Position.CenterBack, 2 },
            { Position.LeftCenterBack, 2 },
            { Position.LeftBack, 3 },
            { Position.LeftWingBack, 4 },
            { Position.RightDefendingMidfielder, 4 },
            { Position.CenterDefendingMidfielder, 4 },
            { Position.LeftDefendingMidfielder, 4 },
            { Position.RightMidfielder, 8 },
            { Position.RightCenterMidfielder, 8 },
            { Position.CenterMidfielder, 8 },
            { Position.LeftCenterMidfielder, 8 },
            { Position.LeftMidfielder, 8 },
            { Position.CenterAttackingMidfielder, 10 },
            { Position.RightWinger, 8 },
            { Position.LeftWinger, 8 },
            { Position.RightForward, 4 },
            { Position.LeftForward, 4 },
            { Position.Striker, 4 }
        };
        #endregion

        private IMatchRandomizer randomizer = MatchRandomizer.Current;

        public Player(int id, string name, Team team, Position position, int defensiveSkills, int offensiveSkills, int form)
        {
            Id = id;
            Name = name;
            Team = team;
            Position = position;

            DefensiveSkills = defensiveSkills;
            OffensiveSkills = offensiveSkills;
            Form = form;

            Rating = InitialRating;
        }

        public int Id { get; private set; }

        public Team Team { get; private set; }

        public string Name { get; private set; }

        public Position Position { get; private set; }

        public int DefensiveSkills { get; private set; }

        public int OffensiveSkills { get; private set; }

        public int Form { get; private set; }

        public double Rating { get; private set; }

        public int FinalRating
        {
            get { return Math.Max(1, (int)Math.Ceiling(10 * Rating)); }
        }

        public bool IsGoalkeeper
        {
            get { return Position == Position.Goalkeeper; }
        }

        private bool IsCenterMidfielder
        {
            get
            {
                return Position.In(
                    Position.RightCenterMidfielder,
                    Position.CenterMidfielder,
                    Position.LeftCenterMidfielder,
                    Position.CenterAttackingMidfielder);
            }
        }

        private bool IsWinger
        {
            get
            {
                return Position.In(
                    Position.RightWingBack,
                    Position.LeftWingBack,
                    Position.RightMidfielder,
                    Position.LeftMidfielder,
                    Position.RightWinger,
                    Position.LeftWinger);
            }
        }

        public bool IsStrategicPlayer(TeamStrategy strategy)
        {
            return
                (strategy == TeamStrategy.Center && IsCenterMidfielder) ||
                (strategy == TeamStrategy.Wings && IsWinger) ||
                (strategy == TeamStrategy.Playmaker && Position == Position.CenterAttackingMidfielder);
        }

        public override string ToString()
        {
            return string.Format(CultureInfo.CurrentCulture, "{0} {1}", Position, Name);
        }

        public void AdjustRating(double adjustment)
        {
            Rating = Math.Max(0, Math.Min(Rating + adjustment, 1.0));
        }

        public bool TryPass(Player receiver, Player opponent)
        {
            return randomizer.TryPass(
                passing: OffensiveSkills + Form,
                positioning: receiver.OffensiveSkills + receiver.Form,
                marking: opponent.DefensiveSkills + opponent.Form);
        }

        public bool TryDribble(Player opponent)
        {
            return randomizer.TryDribble(
                dribbling: OffensiveSkills + Form,
                tackling: opponent.DefensiveSkills + opponent.Form);
        }

        public ShotResult TryShoot(Player opponent, Player goalkeeper)
        {
            return randomizer.TryShoot(
                shooting: OffensiveSkills + Form,
                blocking: opponent.DefensiveSkills + opponent.Form,
                goalkeeping: goalkeeper.DefensiveSkills + goalkeeper.Form);
        }

        public double ChanceToInitiateAttack(TeamStrategy strategy)
        {
            double baseChance = initiateAttackChanceByPosition[Position];

            if (IsStrategicPlayer(strategy))
            {
                baseChance *= 3.0;
            }

            return baseChance;
        }

        public double ChanceToReceivePass(Player passingPlayer, TeamStrategy strategy)
        {
            if (passingPlayer == this)
            {
                return 0;
            }
            else
            {
                int distanceForward = passingPlayer.Position.DistanceForward(Position);
                int distanceSideways = passingPlayer.Position.DistanceSideways(Position);

                double chance = 1;
                
                if (distanceForward > 0)
                {
                    chance = 10.0 / distanceForward;
                }
                else if (distanceForward == 0)
                {
                    chance = 5.0;
                }
                else
                {
                    chance = 1.0 / (-distanceForward);
                }

                if (distanceSideways > 0)
                {
                    chance /= distanceSideways;
                }

                if (IsStrategicPlayer(strategy) && !passingPlayer.IsStrategicPlayer(strategy))
                {
                    chance *= 2.0;
                }

                return chance;
            }
        }

        public double ChanceToMarkPassReceiver(Player receiver)
        {
            return ChanceToConfrontPlayer(receiver);
        }

        public double ChanceToConfrontDribbler(Player dribbler)
        {
            return ChanceToConfrontPlayer(dribbler);
        }

        public double ChanceToConfrontShooter(Player shooter)
        {
            return ChanceToConfrontPlayer(shooter);
        }

        private double ChanceToConfrontPlayer(Player opponent)
        {
            Position oppositePosition = opponent.Position.Opposite();
            int distanceForward = oppositePosition.DistanceForward(Position);
            int distanceSideways = oppositePosition.DistanceSideways(Position);

            double chance = 10;

            if (distanceForward > 0)
            {
                chance = 2.0 / distanceForward;
            }
            else if (distanceForward == 0)
            {
                chance = 10.0;
            }
            else
            {
                chance = 5.0 / (-distanceForward);
            }

            if (distanceSideways > 0)
            {
                chance /= 2 * distanceSideways;
            }

            return chance;
        }
    }
}
