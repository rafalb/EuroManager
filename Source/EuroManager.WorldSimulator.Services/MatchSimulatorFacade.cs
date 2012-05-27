using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EuroManager.WorldSimulator.Domain;

namespace EuroManager.WorldSimulator.Services
{
    public class MatchSimulatorFacade
    {
        public MatchResult Play(Fixture fixture)
        {
            MatchSimulator.Domain.Team team1 = ConvertTeamForMatchSimulator(fixture.Team1);
            MatchSimulator.Domain.Team team2 = ConvertTeamForMatchSimulator(fixture.Team2);

            MatchSimulator.Domain.Match match;

            if (fixture.IsSecondLeg)
            {
                var firstLegResult = fixture.FirstLeg.Result;
                match = new MatchSimulator.Domain.Match(team1, team2, firstLegResult.Score1, firstLegResult.Score2);
            }
            else
            {
                match = new MatchSimulator.Domain.Match(team1, team2, fixture.IsNeutralGround, fixture.RequiresExtraTime);
            }

            var simulator = new MatchSimulator.Domain.Simulator(MatchSimulator.Domain.MatchRandomizer.Current);
            var simulatorResult = simulator.Play(match);

            var allPlayers = Enumerable.Union(fixture.Team1.Players, fixture.Team2.Players).ToArray();
            var goals = simulatorResult.Goals.Select(g =>
                new Goal(g.IsForFirstTeam, g.Minute, g.Extended, allPlayers.Single(p => p.Id == g.Scorer.Id))).ToArray();

            Team winner = null;

            if (simulatorResult.Winner != null)
            {
                winner = simulatorResult.Winner == team1 ? fixture.Team1 : fixture.Team2;
            }

            var result = new MatchResult(fixture, winner, simulatorResult.Score1, simulatorResult.Score2,
                simulatorResult.PenaltyScore1, simulatorResult.PenaltyScore2, goals);

            return result;
        }

        private MatchSimulator.Domain.Team ConvertTeamForMatchSimulator(Team team)
        {
            var matchTeam = new MatchSimulator.Domain.Team(team.Name, team.Strategy);

            foreach (var squadMember in team.Squad)
            {
                Player player = squadMember.Player;
                matchTeam.AddSquadPlayer(player.Id, player.Name,
                    MatchSimulator.Domain.Position.FromCode(squadMember.Position),
                    player.DefensiveSkills, player.OffensiveSkills, player.Form);
            }

            return matchTeam;
        }
    }
}
