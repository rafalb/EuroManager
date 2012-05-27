using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EuroManager.MatchSimulator.Domain.Actions;
using EuroManager.MatchSimulator.Domain.Events;

namespace EuroManager.MatchSimulator.Domain
{
    public class Simulator
    {
        private IMatchRandomizer randomizer;

        public Simulator(IMatchRandomizer randomizer)
        {
            this.randomizer = randomizer;
        }

        public MatchResult Play(Match match)
        {
            AdvanceMatch(match);

            int extendedLength = randomizer.ExtendedTime();
            match.ExtendTime(extendedLength);

            AdvanceMatch(match);

            if (!match.IsConclusive)
            {
                match.EnterExtraTime();
                AdvanceMatch(match);

                if (!match.IsConclusive)
                {
                    PlayPenaltyShootout(match);
                }
            }

            return new MatchResult(match.Team1, match.Team2, match.Winner, match.Score1, match.Score2, match.PenaltyScore1, match.PenaltyScore2, match.Events);
        }

        private void AdvanceMatch(Match match)
        {
            while (match.IsActive)
            {
                PerformAttack(match);
                match.AdvanceTime();
            }
        }

        private void PerformAttack(Match match)
        {
            Team team = randomizer.AttackingTeam(match.Team1, match.Team2, match.IsNeutralGround);
            Player player = team.PickPlayerInitiatingAttack();
            match.InitiateAttack(player);

            int level = 0;
            bool canContinue = true;

            while (canContinue)
            {
                level++;

                IMatchAction action = randomizer.NextAction(level, player);
                action.Perform(match);

                canContinue = action.CanContinue;
            }

            match.CompleteAttack();
        }

        private void PlayPenaltyShootout(Match match)
        {
            bool isFirstTeamShooting = randomizer.IsFirstTeamStartingPenaltyShootout();

            while (!match.IsConclusive)
            {
                Team team = isFirstTeamShooting ? match.Team1 : match.Team2;

                bool isScored = randomizer.TryPenaltyKick();
                match.OnExtraPenaltyKick(team, isScored);

                isFirstTeamShooting = !isFirstTeamShooting;
            }
        }
    }
}
