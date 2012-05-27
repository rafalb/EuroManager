using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace EuroManager.WorldSimulator.Domain
{
    public class GroupStats : IEntity
    {
        public GroupStats(int groupNumber, IEnumerable<Team> teams, bool isNeutralGround, bool hasReturnRound)
        {
            Number = groupNumber;
            IsNeutralGround = isNeutralGround;
            HasReturnRound = hasReturnRound;
            TeamStats = teams.Select(t => new TeamStats(t, groupNumber)).ToList();
        }

        protected GroupStats()
        {
        }

        public int Id { get; private set; }

        [Timestamp]
        public byte[] Version { get; private set; }

        public int Number { get; private set; }

        public bool IsNeutralGround { get; private set; }

        public bool HasReturnRound { get; private set; }

        public virtual List<TeamStats> TeamStats { get; private set; }

        public IEnumerable<TeamStats> Standings
        {
            get { return TeamStats.OrderByDescending(s => s).ToArray(); }
        }

        public IEnumerable<Team> Teams
        {
            get { return TeamStats.Select(s => s.Team).ToArray(); }
        }

        public int RoundsPerTeam
        {
            get { return (HasReturnRound ? 2 : 1) * (TeamStats.Count - 1); }
        }

        public bool IsCompleted
        {
            get { return TeamStats.All(s => s.Played == RoundsPerTeam); }
        }

        public IEnumerable<Fixture> CreateFixtures(TournamentSeason season, IEnumerable<DateTime> dates)
        {
            var scheduler = new Scheduler();
            return scheduler.ScheduleLeagueFixtures(season, dates, HasReturnRound, Teams);
        }

        public bool TryApplyResult(Team team1, Team team2, int score1, int score2)
        {
            var stats1 = TeamStats.FirstOrDefault(s => s.Team == team1);
            var stats2 = TeamStats.FirstOrDefault(s => s.Team == team2);

            if (stats1 != null && stats2 != null)
            {
                stats1.ApplyResult(score1, score2);
                stats2.ApplyResult(score2, score1);

                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
