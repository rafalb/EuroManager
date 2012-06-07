using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;

namespace EuroManager.WorldSimulator.Domain
{
    public class TeamStats : IEntity, IComparable<TeamStats>
    {
        public TeamStats(Team team)
            : this(team, 0)
        {
        }

        public TeamStats(Team team, int groupNumber)
        {
            Team = team;
            TeamName = team.Name;
            GroupNumber = groupNumber;
        }

        protected TeamStats()
        {
        }

        public int Id { get; private set; }

        [Timestamp]
        public byte[] Version { get; private set; }

        public int? TeamId { get; private set; }

        public virtual Team Team { get; private set; }

        [StringLength(100)]
        public string TeamName { get; private set; }

        public int GroupNumber { get; private set; }

        public virtual List<PlayerTournamentStats> PlayersStats { get; private set; }

        public int Played { get; private set; }

        public int Wins { get; private set; }

        public int Draws { get; private set; }

        public int Losses { get; private set; }

        public int Points { get; private set; }

        public int GoalsFor { get; private set; }

        public int GoalsAgainst { get; private set; }

        public int GoalDifference
        {
            get { return GoalsFor - GoalsAgainst; }
        }

        public static bool operator ==(TeamStats stats1, TeamStats stats2)
        {
            return object.ReferenceEquals(stats1, stats2);
        }

        public static bool operator !=(TeamStats stats1, TeamStats stats2)
        {
            return !(stats1 == stats2);
        }

        public static bool operator <(TeamStats stats1, TeamStats stats2)
        {
            return stats1 != null && stats2 != null && stats1.CompareTo(stats2) < 0;
        }

        public static bool operator >(TeamStats stats1, TeamStats stats2)
        {
            return stats1 != stats2 && !(stats1 < stats2);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            else
            {
                var stats = obj as TeamStats;
                return stats != null && this == stats;
            }
        }

        public override int GetHashCode()
        {
            return TeamName.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "{0} - {1} pts", Team.Name, Points);
        }

        public int CompareTo(TeamStats other)
        {
            if (Points > other.Points)
            {
                return 1;
            }
            else if (Points < other.Points)
            {
                return -1;
            }
            else
            {
                if (GoalDifference > other.GoalDifference)
                {
                    return 1;
                }
                else if (GoalDifference < other.GoalDifference)
                {
                    return -1;
                }
                else
                {
                    if (GoalsFor > other.GoalsFor)
                    {
                        return 1;
                    }
                    else if (GoalsFor < other.GoalsFor)
                    {
                        return -1;
                    }
                    else
                    {
                        return string.Compare(Team.Name, other.Team.Name, StringComparison.OrdinalIgnoreCase);
                    }
                }
            }
        }

        public void ApplyResult(MatchResult result)
        {
            int score = result.Team1 == Team ? result.Score1 : result.Score2;
            int opponentScore = result.Team1 == Team ? result.Score2 : result.Score1;

            Played += 1;

            if (score > opponentScore)
            {
                Wins += 1;
                Points += 3;
            }
            else if (score == opponentScore)
            {
                Draws += 1;
                Points += 1;
            }
            else
            {
                Losses += 1;
            }

            GoalsFor += score;
            GoalsAgainst += opponentScore;
        }
    }
}
