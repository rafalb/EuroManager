using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EuroManager.Common.Domain;
using EuroManager.MatchSimulator.Domain;
using NUnit.Framework;

namespace EuroManager.MatchSimulator.Tests.Statistical
{
    [TestFixture]
    public class StatisticalTests
    {
        private static readonly int matchRepeatCount = 1000;
        private IMatchRandomizer randomizer = MatchRandomizer.Current;
        private Simulator simulator;
        private Team veryStrongTeam = CreateVeryStrongTeam();
        private Team weakTeam = CreateWeakTeam();
        private IEnumerable<MatchResult> results;

        [TestFixtureSetUp]
        public void FixtureSetUp()
        {
            simulator = new Simulator(randomizer);
            results = SimulateMatchRepeatedly(veryStrongTeam, weakTeam, false, false);
        }

        [Test]
        public void VeryStrongTeamShouldWinMostMatchesWithWeakTeam()
        {
            double winsPercent = CalculateResultsPercent(results, r => r.Winner == veryStrongTeam);
            Assert.That(winsPercent, Is.GreaterThan(0.84));
        }

        [Test]
        public void VeryStrongTeamShouldNotLoseTooManyMatchesWithWeakTeam()
        {
            double lossesPercent = CalculateResultsPercent(results, r => r.Winner == weakTeam);
            Assert.That(lossesPercent, Is.LessThan(0.04));
        }

        [Test]
        public void VeryStrongTeamShouldScoreManyGoalsWithWeakTeam()
        {
            double averageGoalsScored = (double)results.Sum(r => r.Score1) / results.Count();
            Assert.That(averageGoalsScored, Is.GreaterThan(2.68));
        }

        [Test]
        public void VeryStrongTeamShouldNotLoseManyGoalsWithWeakTeam()
        {
            double averageGoalsLost = (double)results.Sum(r => r.Score2) / results.Count();
            Assert.That(averageGoalsLost, Is.LessThan(0.37));
        }

        private double CalculateResultsPercent(IEnumerable<MatchResult> results, Func<MatchResult, bool> predicate)
        {
            return (double)results.Count(predicate) / results.Count();
        }

        private IEnumerable<MatchResult> SimulateMatchRepeatedly(Team team1, Team team2, bool isNeutralGround, bool isExtraTimeRequired)
        {
            var results = new MatchResult[matchRepeatCount];

            for (int i = 0; i < matchRepeatCount; i++)
            {
                var match = new Match(team1, team2, isNeutralGround, isExtraTimeRequired);
                results[i] = simulator.Play(match);
            }

            return results;
        }

        private static Team CreateVeryStrongTeam()
        {
            Team strongTeam = new Team("FC Barcelona", TeamStrategy.Wings);
            strongTeam.AddSquadPlayer(1, "V. Valdes", Position.Goalkeeper, 84, 50, 79);
            strongTeam.AddSquadPlayer(2, "Dani Alves", Position.RightBack, 82, 89, 86);
            strongTeam.AddSquadPlayer(3, "Puyol", Position.RightCenterBack, 92, 61, 83);
            strongTeam.AddSquadPlayer(4, "Pique", Position.LeftCenterBack, 87, 73, 77);
            strongTeam.AddSquadPlayer(5, "Jordi Alba", Position.LeftBack, 74, 85, 94);
            strongTeam.AddSquadPlayer(6, "S. Busquets", Position.CenterDefendingMidfielder, 86, 67, 85);
            strongTeam.AddSquadPlayer(7, "Xavi", Position.RightCenterMidfielder, 75, 89, 86);
            strongTeam.AddSquadPlayer(8, "Fabregas", Position.LeftCenterMidfielder, 71, 88, 85);
            strongTeam.AddSquadPlayer(9, "A. Sanchez", Position.RightWinger, 58, 83, 77);
            strongTeam.AddSquadPlayer(10, "Iniesta", Position.LeftWinger, 62, 92, 91);
            strongTeam.AddSquadPlayer(11, "Messi", Position.Striker, 54, 95, 96);
            return strongTeam;
        }

        private static Team CreateWeakTeam()
        {
            Team weakTeam = new Team("Weak", TeamStrategy.Wings);
            weakTeam.AddSquadPlayer(1, "Player", Position.Goalkeeper, 62, 50, 77);
            weakTeam.AddSquadPlayer(2, "Player", Position.RightBack, 68, 69, 76);
            weakTeam.AddSquadPlayer(3, "Player", Position.RightCenterBack, 66, 51, 73);
            weakTeam.AddSquadPlayer(4, "Player", Position.LeftCenterBack, 61, 59, 69);
            weakTeam.AddSquadPlayer(5, "Player", Position.LeftBack, 54, 65, 78);
            weakTeam.AddSquadPlayer(6, "Player", Position.CenterDefendingMidfielder, 62, 57, 70);
            weakTeam.AddSquadPlayer(7, "Player", Position.RightCenterMidfielder, 55, 67, 73);
            weakTeam.AddSquadPlayer(8, "Player", Position.LeftCenterMidfielder, 61, 64, 72);
            weakTeam.AddSquadPlayer(9, "Player", Position.RightWinger, 58, 61, 77);
            weakTeam.AddSquadPlayer(10, "Player", Position.LeftWinger, 62, 66, 80);
            weakTeam.AddSquadPlayer(11, "Player", Position.Striker, 54, 64, 72);
            return weakTeam;
        }
    }
}
