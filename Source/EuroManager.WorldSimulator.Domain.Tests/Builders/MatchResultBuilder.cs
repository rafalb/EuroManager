using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EuroManager.WorldSimulator.Domain.Tests.Builders
{
    public class MatchResultBuilder
    {
        private Fixture fixture;
        private Team team1;
        private Team team2;
        private Team winner;
        private int score1 = 1;
        private int score2 = 0;
        private IEnumerable<PlayerMatchStats> playersStats1 = Enumerable.Empty<PlayerMatchStats>();
        private IEnumerable<PlayerMatchStats> playersStats2 = Enumerable.Empty<PlayerMatchStats>();

        public MatchResultBuilder ForFixture(Fixture fixture)
        {
            this.fixture = fixture;
            this.team1 = fixture.Team1;
            this.team2 = fixture.Team2;
            return this;
        }

        public MatchResultBuilder ForTeams(Team team1, Team team2)
        {
            this.team1 = team1;
            this.team2 = team2;
            return this;
        }

        public MatchResultBuilder WonBy(Team winner)
        {
            this.winner = winner;
            return this;
        }

        public MatchResultBuilder WithScore(int score1, int score2)
        {
            this.score1 = score1;
            this.score2 = score2;
            return this;
        }

        public MatchResultBuilder WithTeam1PlayersStats(params PlayerMatchStats[] playersStats)
        {
            this.playersStats1 = playersStats;
            return this;
        }

        public MatchResultBuilder WithTeam2PlayersStats(params PlayerMatchStats[] playersStats)
        {
            this.playersStats2 = playersStats;
            return this;
        }
        
        public MatchResult Build()
        {
            if (fixture == null)
            {
                if (team1 == null)
                {
                    team1 = A.Team.Build();
                }

                if (team2 == null)
                {
                    team2 = A.Team.Build();
                }

                fixture = new Fixture(A.LeagueSeason.Build(), A.Date, team1, team2, true, false);
            }

            return new MatchResult(fixture, winner == null ? team1 : winner, score1, score2, 0, 0,
                Enumerable.Empty<Goal>(), playersStats1, playersStats2);
        }
    }
}
