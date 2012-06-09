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
        private static readonly Dictionary<Position, double> InitiateAttackChanceByPosition = new Dictionary<Position, double>
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

        public int Goals { get; private set; }

        public int Assists { get; private set; }

        public int PassesCompleted { get; private set; }

        public int PassesFailed { get; private set; }

        public int DribblesCompleted { get; private set; }

        public int DribblesFailed { get; private set; }

        public int ShotsOnTarget { get; private set; }

        public int ShotsMissed { get; private set; }

        public int ShotsBlocked { get; private set; }

        public int PassesIntercepted { get; private set; }

        public int PassesAllowed { get; private set; }

        public int TacklesCompleted { get; private set; }

        public int TacklesFailed { get; private set; }

        public int ShotsPrevented { get; private set; }

        public int ShotsAllowed { get; private set; }

        public int ShotsSaved { get; private set; }

        public int ShotsNotSaved { get; private set; }

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
            bool isSuccessful = randomizer.TryPass(
                passing: OffensiveSkills + Form,
                positioning: receiver.OffensiveSkills + receiver.Form,
                marking: opponent.DefensiveSkills + opponent.Form);

            if (isSuccessful)
            {
                PassesCompleted += 1;
                opponent.PassesAllowed += 1;
            }
            else
            {
                PassesFailed += 1;
                opponent.PassesIntercepted += 1;
            }

            return isSuccessful;
        }

        public bool TryDribble(Player opponent)
        {
            bool isSuccessful = randomizer.TryDribble(
                dribbling: OffensiveSkills + Form,
                tackling: opponent.DefensiveSkills + opponent.Form);

            if (isSuccessful)
            {
                DribblesCompleted += 1;
                opponent.TacklesFailed += 1;
            }
            else
            {
                DribblesFailed += 1;
                opponent.TacklesCompleted += 1;
            }

            return isSuccessful;
        }

        public ShotResult TryShoot(Player assistant, Player opponent, Player goalkeeper)
        {
            ShotResult result = randomizer.TryShoot(
                shooting: OffensiveSkills + Form,
                blocking: opponent.DefensiveSkills + opponent.Form,
                goalkeeping: goalkeeper.DefensiveSkills + goalkeeper.Form);

            if (result == ShotResult.Scored)
            {
                Goals += 1;
                ShotsOnTarget += 1;
                opponent.ShotsAllowed += 1;
                goalkeeper.ShotsNotSaved += 1;

                if (assistant != null)
                {
                    assistant.Assists += 1;
                }
            }
            else if (result == ShotResult.Saved)
            {
                ShotsOnTarget += 1;
                opponent.ShotsAllowed += 1;
                goalkeeper.ShotsSaved += 1;
            }
            else if (result == ShotResult.Missed)
            {
                ShotsMissed += 1;
                opponent.ShotsAllowed += 1;
            }
            else if (result == ShotResult.Blocked)
            {
                ShotsBlocked += 1;
                opponent.ShotsPrevented += 1;
            }
            else
            {
                throw new NotSupportedException(string.Format(CultureInfo.InvariantCulture, "Shot result {0} not supported", result));
            }

            return result;
        }

        public double ChanceToInitiateAttack(TeamStrategy strategy)
        {
            double baseChance = InitiateAttackChanceByPosition[Position];

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

                if (IsGoalkeeper)
                {
                    chance /= 5;
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

            if (IsGoalkeeper)
            {
                chance /= 5;
            }

            return chance;
        }
    }
}
