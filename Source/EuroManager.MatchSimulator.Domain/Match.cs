using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using EuroManager.MatchSimulator.Domain.Events;

namespace EuroManager.MatchSimulator.Domain
{
    public class Match
    {
        private bool isExtraTimeRequired = false;
        private bool isSecondLeg = false;
        private int firstLegScore1 = 0;
        private int firstLegScore2 = 0;

        private List<IMatchEvent> events = new List<IMatchEvent>();

        public Match(Team team1, Team team2, bool isNeutralGround, bool isExtraTimeRequired)
        {
            Team1 = team1;
            Team2 = team2;
            IsNeutralGround = isNeutralGround;
            this.isExtraTimeRequired = isExtraTimeRequired;

            team1.Opponent = team2;
            team2.Opponent = team1;

            Length = 90;
            Minute = 1;
            ExtendedLength = 0;
            ExtendedMinute = 0;
        }

        public Match(Team team1, Team team2, int firstLegScore1, int firstLegScore2)
            : this(team1, team2, false, true)
        {
            this.firstLegScore1 = firstLegScore1;
            this.firstLegScore2 = firstLegScore2;
            isSecondLeg = true;
        }

        public Team Team1 { get; private set; }

        public Team Team2 { get; private set; }

        public bool IsNeutralGround { get; private set; }

        public int Score1 { get; private set; }

        public int Score2 { get; private set; }

        public int PenaltyScore1 { get; private set; }

        public int PenaltyScore2 { get; private set; }

        public int Length { get; private set; }

        public int ExtendedLength { get; private set; }

        public int Minute { get; private set; }

        public int ExtendedMinute { get; private set; }

        public Player CurrentPlayer { get; private set; }

        public IEnumerable<IMatchEvent> Events
        {
            get { return events; }
        }

        public bool IsActive
        {
            get { return ExtendedLength == 0 ? Minute <= Length : ExtendedMinute <= ExtendedLength; }
        }

        public bool IsConclusive
        {
            get
            {
                if (isSecondLeg)
                {
                    return !(Score1 == firstLegScore1 && Score2 == firstLegScore2) || ArePenaltiesConclusive();
                }
                else
                {
                    return !isExtraTimeRequired || Score1 != Score2 || ArePenaltiesConclusive();
                }
            }
        }

        public Team Winner
        {
            get { return isSecondLeg ? SelectTieWinner() : SelectSingleMatchWinner(); }
        }

        private Player PreviousPlayer { get; set; }

        private int PenaltyRoundsPlayed1 { get; set; }

        private int PenaltyRoundsPlayed2 { get; set; }

        private int TotalScore1
        {
            get { return (isSecondLeg ? firstLegScore2 : 0) + Score1; }
        }

        private int TotalScore2
        {
            get { return (isSecondLeg ? firstLegScore1 : 0) + Score2; }
        }

        private int AwayGoals1
        {
            get { return isSecondLeg && !IsNeutralGround ? firstLegScore2 : 0; }
        }

        private int AwayGoals2
        {
            get { return isSecondLeg && !IsNeutralGround ? Score2 : 0; }
        }

        public void AdvanceTime()
        {
            Contract.Requires(IsActive);

            if (ExtendedLength > 0)
            {
                ExtendedMinute++;
            }
            else
            {
                Minute++;
            }
        }

        public void ExtendTime(int minutes)
        {
            Contract.Requires(!IsActive);
            Contract.Requires(minutes > 0);
            Contract.Ensures(IsActive);

            Minute = 90;
            ExtendedMinute = 1;
            ExtendedLength = minutes;
        }

        public void EnterExtraTime()
        {
            Contract.Requires(!IsActive);
            Contract.Ensures(IsActive);

            Minute = 91;
            Length = 120;
            ExtendedMinute = 0;
            ExtendedLength = 0;
        }

        public void InitiateAttack(Player player)
        {
            PreviousPlayer = null;
            CurrentPlayer = player;
        }

        public void CompleteAttack()
        {
            PreviousPlayer = null;
            CurrentPlayer = null;
        }

        public void OnPass(Player receiver, Player opponent, bool isSuccessful)
        {
            events.Add(new PassEvent(Minute, ExtendedMinute, CurrentPlayer, receiver, opponent));

            PreviousPlayer = CurrentPlayer;
            CurrentPlayer = isSuccessful ? receiver : null;
        }

        public void OnDribble(Player opponent, bool isSuccessful)
        {
            events.Add(new DribbleEvent(Minute, ExtendedMinute, CurrentPlayer, opponent));

            if (!isSuccessful)
            {
                PreviousPlayer = CurrentPlayer;
                CurrentPlayer = null;
            }
        }

        public void OnShoot(Player opponent, bool isSuccessful)
        {
            events.Add(new ShootEvent(Minute, ExtendedMinute, CurrentPlayer, opponent));

            if (isSuccessful)
            {
                IncreaseScore(CurrentPlayer.Team);
                events.Add(new GoalEvent(CurrentPlayer.Team == Team1, Minute, ExtendedMinute, CurrentPlayer, PreviousPlayer));
            }

            PreviousPlayer = CurrentPlayer;
            CurrentPlayer = null;
        }

        public void OnExtraPenaltyKick(Team team, bool isScored)
        {
            if (team == Team1)
            {
                PenaltyScore1 += isScored ? 1 : 0;
                PenaltyRoundsPlayed1++;
            }
            else
            {
                PenaltyScore2 += isScored ? 1 : 0;
                PenaltyRoundsPlayed2++;
            }

            events.Add(new PenaltyKickEvent(team == Team1, 120, 0, isScored));
        }

        private void IncreaseScore(Team team)
        {
            if (team == Team1)
            {
                Score1 += 1;
            }
            else
            {
                Score2 += 1;
            }
        }

        private bool ArePenaltiesConclusive()
        {
            if (PenaltyRoundsPlayed1 >= 100)
            {
                return true;
            }
            else if (PenaltyRoundsPlayed1 < 5 || PenaltyRoundsPlayed2 < 5)
            {
                return
                    PenaltyScore1 + (5 - PenaltyRoundsPlayed1) < PenaltyScore2 ||
                    PenaltyScore2 + (5 - PenaltyRoundsPlayed2) < PenaltyScore1;
            }
            else
            {
                if (PenaltyRoundsPlayed1 == PenaltyRoundsPlayed2)
                {
                    return PenaltyScore1 != PenaltyScore2;
                }
                else if (PenaltyRoundsPlayed1 > PenaltyRoundsPlayed2)
                {
                    return PenaltyScore1 < PenaltyScore2;
                }
                else
                {
                    return PenaltyScore1 > PenaltyScore2;
                }
            }
        }

        private Team SelectSingleMatchWinner()
        {
            if (Score1 > Score2)
            {
                return Team1;
            }
            else if (Score1 < Score2)
            {
                return Team2;
            }
            else
            {
                if (PenaltyScore1 > PenaltyScore2)
                {
                    return Team1;
                }
                else if (PenaltyScore1 < PenaltyScore2)
                {
                    return Team2;
                }
                else
                {
                    return null;
                }
            }
        }

        private Team SelectTieWinner()
        {
            if (TotalScore1 > TotalScore2)
            {
                return Team1;
            }
            else if (TotalScore1 < TotalScore2)
            {
                return Team2;
            }
            else
            {
                if (AwayGoals1 > AwayGoals2)
                {
                    return Team1;
                }
                else if (AwayGoals1 < AwayGoals2)
                {
                    return Team2;
                }
                else
                {
                    if (PenaltyScore1 > PenaltyScore2)
                    {
                        return Team1;
                    }
                    else if (PenaltyScore1 < PenaltyScore2)
                    {
                        return Team2;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }
    }
}
