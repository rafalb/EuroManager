using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EuroManager.Common.Tests;
using EuroManager.MatchSimulator.Domain.Events;
using NUnit.Framework;

namespace EuroManager.MatchSimulator.Domain.Tests
{
    [TestFixture]
    public class MatchTests : UnitTestFixture
    {
        [Test]
        public void ShouldBeActiveWhenStarted()
        {
            Match match = A.Match.Build();

            Assert.That(match.IsActive, Is.True);
        }

        [Test]
        public void ShouldBeActiveAfterAdvancingTime()
        {
            Match match = A.Match.Build();
            AdvanceTime(match, 5);

            Assert.That(match.IsActive, Is.True);
        }

        [Test]
        public void ShouldBecomeInactiveAfterAdvancingFullTime()
        {
            Match match = A.Match.Build();
            AdvanceTime(match, 90);

            Assert.That(match.IsActive, Is.False);
        }

        [Test]
        public void ShouldBecomeActiveAfterExtendingTime()
        {
            Match match = A.Match.Build();
            AdvanceTime(match, 90);
            match.ExtendTime(3);

            Assert.That(match.IsActive, Is.True);
        }

        [Test]
        public void ShouldBecomeInactiveAfterExtendedTimeRunsOut()
        {
            Match match = A.Match.Build();
            AdvanceTime(match, 90);
            match.ExtendTime(3);
            AdvanceTime(match, 3);

            Assert.That(match.IsActive, Is.False);
        }

        [Test]
        public void ShouldBecomeActiveAfterEnteringExtraTime()
        {
            Match match = A.Match.Build();
            AdvanceTime(match, 90);
            match.ExtendTime(3);
            AdvanceTime(match, 3);
            match.EnterExtraTime();

            Assert.That(match.IsActive, Is.True);
        }

        [Test]
        public void ShouldIncreaseLengthAfterEnteringExtraTime()
        {
            Match match = A.Match.Build();
            AdvanceTime(match, 90);
            match.ExtendTime(3);
            AdvanceTime(match, 3);
            match.EnterExtraTime();

            Assert.That(match.Length, Is.EqualTo(120));
        }

        [Test]
        public void ShouldBecomeInactiveAfterExtraTimeRunsOut()
        {
            Match match = A.Match.Build();
            AdvanceTime(match, 90);
            match.ExtendTime(3);
            AdvanceTime(match, 3);
            match.EnterExtraTime();
            AdvanceTime(match, 30);

            Assert.That(match.IsActive, Is.False);
        }

        [Test]
        public void ShouldBeConclusiveWhenNotDrawn()
        {
            Match match = A.Match.WithScore(1, 0).WithExtraTimeRequired().Build();

            Assert.That(match.IsConclusive, Is.True);
        }

        [Test]
        public void ShouldBeConclusiveWhenDrawnAndExtraTimeNotRequired()
        {
            Match match = A.Match.WithScore(1, 1).Build();

            Assert.That(match.IsConclusive, Is.True);
        }

        [Test]
        public void ShouldNotBeConclusiveWhenDrawnAndExtraTimeRequired()
        {
            Match match = A.Match.WithScore(1, 1).WithExtraTimeRequired().Build();

            Assert.That(match.IsConclusive, Is.False);
        }

        [Test]
        public void ShouldNotBeConclusiveWhenResultIsTheSameAsFirstLegs()
        {
            Match match = A.Match.WithFirstLegResult(1, 0).WithScore(1, 0).Build();

            Assert.That(match.IsConclusive, Is.False);
        }

        [Test]
        public void ShouldBeConclusiveWhenResultIsDifferentThanFirstLegs()
        {
            Match match = A.Match.WithFirstLegResult(1, 1).WithScore(2, 2).Build();

            Assert.That(match.IsConclusive, Is.True);
        }

        [Test]
        public void ShouldBeConclusiveWhenPenaltiesAreConclusive()
        {
            Match match = A.Match.WithScore(1, 1).WithExtraTimeRequired()
                .WithPenaltiesScored(3, 4).WithPenaltiesMissed(2, 0).Build();

            Assert.That(match.IsConclusive, Is.True);
        }

        [Test]
        public void ShouldNotBeConclusiveWhenPenaltiesAreNotFinished()
        {
            Match match = A.Match.WithScore(1, 1).WithExtraTimeRequired()
                .WithPenaltiesScored(3, 0).WithPenaltiesMissed(0, 2).Build();

            Assert.That(match.IsConclusive, Is.False);
        }

        [Test]
        public void ShouldNotBeConclusiveWhenPenaltiesAreNotConclusive()
        {
            Match match = A.Match.WithScore(0, 0).WithExtraTimeRequired()
                .WithPenaltiesScored(4, 4).WithPenaltiesMissed(0, 0).Build();

            Assert.That(match.IsConclusive, Is.False);
        }

        [Test]
        public void ShouldBeConclusiveWhenPenaltiesAreConclusiveAfterRegularRounds()
        {
            Match match = A.Match.WithScore(1, 1).WithExtraTimeRequired()
                .WithPenaltiesScored(5, 6).WithPenaltiesMissed(1, 0).Build();

            Assert.That(match.IsConclusive, Is.True);
        }

        [Test]
        public void ShouldNotBeConclusiveWhenPenaltiesAreNotConclusiveAfterRegularRounds()
        {
            Match match = A.Match.WithScore(2, 2).WithExtraTimeRequired()
                .WithPenaltiesScored(6, 5).WithPenaltiesMissed(0, 0).Build();

            Assert.That(match.IsConclusive, Is.False);
        }

        [Test]
        public void ShouldSelectPlayerInitiatingAttack()
        {
            Match match = A.Match.Build();
            Player player = match.Team2.Squad.Last();

            match.InitiateAttack(player);

            Assert.That(match.CurrentPlayer, Is.EqualTo(player));
        }

        [Test]
        public void ShouldUnselectPlayerWhenAttackCompleted()
        {
            Match match = A.Match.Build();
            Player player = match.Team1.Squad.First();

            match.InitiateAttack(player);
            match.CompleteAttack();

            Assert.That(match.CurrentPlayer, Is.Null);
        }

        [Test]
        public void ShouldSelectPassReceiver()
        {
            Match match = A.Match.Build();
            Player player = match.Team1.Squad.ElementAt(3);
            Player receiver = match.Team1.Squad.ElementAt(6);

            match.InitiateAttack(player);
            match.OnPass(receiver, null, isSuccessful: true);

            Assert.That(match.CurrentPlayer, Is.EqualTo(receiver));
        }

        [Test]
        public void ShouldRegisterPassEvent()
        {
            Match match = A.Match.Build();
            Player player = match.Team2.Squad.ElementAt(2);
            Player receiver = match.Team2.Squad.ElementAt(10);

            AdvanceTime(match, 12);
            match.InitiateAttack(player);
            match.OnPass(receiver, null, isSuccessful: true);
            match.CompleteAttack();

            Assert.That(match.Events, Has.Some.Matches<PassEvent>(e =>
                e.Minute == 13 && e.PassingPlayer == player && e.Receiver == receiver));
        }

        [Test]
        public void ShouldUnselectPlayerWhenDribblingFailed()
        {
            Match match = A.Match.Build();
            Player player = match.Team1.Squad.ElementAt(7);

            match.InitiateAttack(player);
            match.OnDribble(null, isSuccessful: false);

            Assert.That(match.CurrentPlayer, Is.Null);
        }

        [Test]
        public void ShouldRegisterGoalEvent()
        {
            Match match = A.Match.Build();
            Player player = match.Team2.Squad.ElementAt(4);
            Player assistant = match.Team2.Squad.ElementAt(7);
            Player scorer = match.Team2.Squad.ElementAt(9);

            AdvanceTime(match, 40);
            match.InitiateAttack(player);
            match.OnPass(assistant, null, isSuccessful: true);
            match.OnPass(scorer, null, isSuccessful: true);
            match.OnShoot(null, isSuccessful: true);
            match.CompleteAttack();

            Assert.That(match.Events.OfType<GoalEvent>(), Has.Some.Matches<GoalEvent>(e =>
                e.Minute == 41 && e.IsForSecondTeam && e.Scorer == scorer && e.Assistant == assistant));
        }

        [Test]
        public void ShouldIncreaseScore()
        {
            Match match = A.Match.WithScore(2, 0).Build();
            Player scorer = match.Team2.Squad.ElementAt(9);

            match.InitiateAttack(scorer);
            match.OnShoot(null, isSuccessful: true);
            match.CompleteAttack();

            Assert.That(match.Score2, Is.EqualTo(1));
        }

        [Test]
        public void ShouldIncreaseExtraPenaltyScore()
        {
            Match match = A.Match.WithScore(1, 1).WithExtraTimeRequired().WithPenaltiesScored(2, 2).WithPenaltiesMissed(0, 1).Build();

            match.OnExtraPenaltyKick(match.Team1, true);

            Assert.That(match.PenaltyScore1, Is.EqualTo(3));
        }

        [Test]
        public void ShouldRegisterEventInExtendedTime()
        {
            Match match = A.Match.Build();
            Player player = match.Team2.Squad.ElementAt(8);

            AdvanceTime(match, 90);
            match.ExtendTime(3);
            AdvanceTime(match, 2);
            
            match.InitiateAttack(player);
            match.OnDribble(null, isSuccessful: true);
            match.CompleteAttack();

            Assert.That(match.Events, Has.Some.Matches<DribbleEvent>(e =>
                e.Minute == 90 && e.Extended == 3 && e.Dribbler == player));
        }

        [Test]
        public void ShouldSelectWinnerByHigherScore()
        {
            Match match = A.Match.WithScore(1, 2).Build();

            Assert.That(match.Winner, Is.EqualTo(match.Team2));
        }

        [Test]
        public void ShouldSelectWinnerByHigherScoreAfterExtraTime()
        {
            Match match = A.Match.WithScore(1, 1).WithExtraTimeRequired().Build();

            AdvanceTime(match, 90);
            match.ExtendTime(1);
            AdvanceTime(match, 1);
            match.EnterExtraTime();

            match.InitiateAttack(match.Team1.Squad.ElementAt(7));
            match.OnShoot(null, isSuccessful: true);
            match.CompleteAttack();

            Assert.That(match.Winner, Is.EqualTo(match.Team1));
        }

        [Test]
        public void ShouldSelectNoWinnerWhenDrawn()
        {
            Match match = A.Match.WithScore(0, 0).Build();

            Assert.That(match.Winner, Is.Null);
        }

        [Test]
        public void ShouldSelectNoWinnerWhenDrawnAfterExtraTime()
        {
            Match match = A.Match.WithScore(0, 0).Build();

            AdvanceTime(match, 90);
            match.ExtendTime(1);
            AdvanceTime(match, 1);
            match.EnterExtraTime();

            Assert.That(match.Winner, Is.Null);
        }

        [Test]
        public void ShouldSelectWinnerAfterPenaltyShootout()
        {
            Match match = A.Match.WithScore(1, 1).WithPenaltiesMissed(2, 1).WithPenaltiesScored(2, 3).Build();

            Assert.That(match.Winner, Is.EqualTo(match.Team2));
        }

        [Test]
        public void ShouldSelectNoSecondLegWinnerWhenDrawnAfterExtraTime()
        {
            Match match = A.Match.WithFirstLegResult(1, 1).WithScore(1, 1).WithExtraTimeRequired().Build();

            AdvanceTime(match, 90);
            match.ExtendTime(1);
            AdvanceTime(match, 1);
            match.EnterExtraTime();

            Assert.That(match.Winner, Is.Null);
        }

        [Test]
        public void ShouldSelectSecondLegWinnerByHigherTotalScore()
        {
            Match match = A.Match.WithFirstLegResult(1, 2).WithScore(0, 2).WithExtraTimeRequired().Build();

            Assert.That(match.Winner, Is.EqualTo(match.Team2));
        }

        [Test]
        public void ShouldSelectSecondLegWinnerByHigherAwayScoreWhenDrawnAfterExtraTime()
        {
            Match match = A.Match.WithFirstLegResult(1, 1).WithScore(0, 0).WithExtraTimeRequired().Build();

            AdvanceTime(match, 90);
            match.ExtendTime(1);
            AdvanceTime(match, 1);
            match.EnterExtraTime();

            Assert.That(match.Winner, Is.EqualTo(match.Team1));
        }

        [Test]
        public void ShouldSelectSecondLegWinnerByHigherPenaltyScoreWhenDrawnAfterExtraTime()
        {
            Match match = A.Match.WithFirstLegResult(0, 0).WithScore(0, 0).WithExtraTimeRequired()
                .WithPenaltiesMissed(1, 2).WithPenaltiesScored(4, 3).Build();

            Assert.That(match.Winner, Is.EqualTo(match.Team1));
        }

        private void AdvanceTime(Match match, int count)
        {
            for (int i = 0; i < count; i++)
            {
                match.AdvanceTime();
            }
        }
    }
}
