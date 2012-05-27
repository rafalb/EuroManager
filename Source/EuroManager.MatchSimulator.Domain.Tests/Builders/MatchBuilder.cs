using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EuroManager.MatchSimulator.Domain.Tests.Builders
{
    public class MatchBuilder
    {
        private int score1;
        private int score2;
        private bool isExtraTimeRequired = false;
        private bool isSecondLeg = false;
        private int firstLegScore1;
        private int firstLegScore2;
        private int penaltiesScored1;
        private int penaltiesScored2;
        private int penaltiesMissed1;
        private int penaltiesMissed2;

        public MatchBuilder WithScore(int score1, int score2)
        {
            this.score1 = score1;
            this.score2 = score2;
            return this;
        }

        public MatchBuilder WithFirstLegResult(int firstLegScore1, int firstLegScore2)
        {
            isSecondLeg = true;
            this.firstLegScore1 = firstLegScore1;
            this.firstLegScore2 = firstLegScore2;
            return this;
        }

        public MatchBuilder WithExtraTimeRequired()
        {
            isExtraTimeRequired = true;
            return this;
        }

        public MatchBuilder WithPenaltiesScored(int penaltiesScored1, int penaltiesScored2)
        {
            this.penaltiesScored1 = penaltiesScored1;
            this.penaltiesScored2 = penaltiesScored2;
            return this;
        }

        public MatchBuilder WithPenaltiesMissed(int penaltiesMissed1, int penaltiesMissed2)
        {
            this.penaltiesMissed1 = penaltiesMissed1;
            this.penaltiesMissed2 = penaltiesMissed2;
            return this;
        }

        public Match Build()
        {
            Match match;

            if (isSecondLeg)
            {
                match = new Match(A.Team.Build(), A.Team.Build(), firstLegScore1, firstLegScore2);
            }
            else
            {
                match = new Match(A.Team.Build(), A.Team.Build(), false, isExtraTimeRequired);
            }

            AddGoals(match, match.Team1, score1);
            AddGoals(match, match.Team2, score2);

            AddPenalties(match, match.Team1, penaltiesScored1, penaltiesMissed1);
            AddPenalties(match, match.Team2, penaltiesScored2, penaltiesMissed2);

            return match;
        }

        private void AddGoals(Match match, Team team, int score)
        {
            for (int i = 0; i < score; i++)
            {
                match.InitiateAttack(team.Squad.Last());
                match.OnShoot(null, true);
            }
        }

        private void AddPenalties(Match match, Team team, int scored, int missed)
        {
            for (int i = 0; i < missed + scored; i++)
            {
                match.OnExtraPenaltyKick(team, i >= missed);
            }
        }
    }
}
