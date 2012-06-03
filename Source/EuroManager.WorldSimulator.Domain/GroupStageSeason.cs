using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EuroManager.Common;

namespace EuroManager.WorldSimulator.Domain
{
    public class GroupStageSeason : CupStageSeason
    {
        private IRandomGenerator randomGenerator = RandomGenerator.Current;

        public GroupStageSeason(GroupStage stage)
            : base(stage)
        {
            GroupCount = stage.GroupCount;
            GroupTeamCount = stage.GroupTeamCount;
            GroupPromotedCount = stage.GroupPromotedCount;
            IsNeutralGround = stage.IsNeutralGround;
            HasReturnRound = stage.HasReturnRound;

            RoundDates = new List<RoundDate>();
            Groups = new List<GroupStats>();
        }

        protected GroupStageSeason()
        {
        }

        public int GroupCount { get; private set; }

        public int GroupTeamCount { get; private set; }

        public int GroupPromotedCount { get; private set; }

        public bool IsNeutralGround { get; private set; }

        public bool HasReturnRound { get; private set; }

        public virtual List<RoundDate> RoundDates { get; private set; }

        public virtual List<GroupStats> Groups { get; private set; }

        public override void ScheduleRoundDates(IEnumerable<DateTime> roundDates)
        {
            RoundDates.AddRange(roundDates.Select((d, i) => new RoundDate(d, i + 1)).ToArray());
        }

        public override void Activate(IEnumerable<Team> teams)
        {
            Teams.AddRange(teams);
            var sortedTeams = randomGenerator.Sort(Teams);

            for (int i = 0; i < GroupCount; i++)
            {
                var groupTeams = sortedTeams.Skip(i * GroupTeamCount).Take(GroupTeamCount).ToArray();
                Groups.Add(new GroupStats(i + 1, groupTeams, IsNeutralGround, HasReturnRound));
            }
        }

        public override void ScheduleFixtures(Action<Fixture> addFixture)
        {
            if (Phase == TournamentPhase.NotStarted)
            {
                var dates = RoundDates.Select(d => d.Date).ToArray();

                foreach (var group in Groups)
                {
                    var fixtures = group.CreateFixtures(CupSeason, dates);

                    foreach (var fixture in fixtures)
                    {
                        addFixture(fixture);
                    }
                }

                Phase = TournamentPhase.InProgress;
            }
        }

        public override void ApplyResult(MatchResult result)
        {
            foreach (var group in Groups)
            {
                if (group.TryApplyResult(result))
                {
                    if (group.IsCompleted)
                    {
                        foreach (var teamStats in group.Standings.Take(GroupPromotedCount))
                        {
                            PromoteTeam(teamStats.Team);
                        }

                        if (Groups.All(g => g.IsCompleted))
                        {
                            Phase = TournamentPhase.Finished;
                        }
                    }

                    break;
                }
            }
        }
    }
}
