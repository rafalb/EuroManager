using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EuroManager.WorldSimulator.Domain.Tests.Builders
{
    public class MatchResultBuilder
    {
        private Fixture fixture;
        private Team winner;
        private int score1 = 1;
        private int score2 = 0;

        public MatchResultBuilder ForFixture(Fixture fixture)
        {
            this.fixture = fixture;
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

        public MatchResult Build()
        {
            return new MatchResult(fixture, winner == null ? fixture.Team1 : winner, score1, score2, 0, 0, Enumerable.Empty<Goal>());
        }
    }
}
