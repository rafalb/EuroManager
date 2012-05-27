using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using EuroManager.Common.Domain;
using EuroManager.MatchSimulator.Domain;
using EuroManager.MatchSimulator.Domain.Events;
using EuroManager.WorldEditor.Loader;

namespace EuroManager.MatchSimulator.Tests.Manual
{
    public class MultipleMatchesTests
    {
        private TeamConverter teamConverter;

        public MultipleMatchesTests(World world)
        {
            teamConverter = new TeamConverter(world);
        }

        public void Perform(int matchCount)
        {
            var results = new List<MatchResult>();
            Team team1 = teamConverter.CreateTeam("Wisla Krakow");
            Team team2 = teamConverter.CreateTeam("Polonia Warszawa");

            for (int i = 0; i < matchCount; i++)
            {
                var match = new Match(team1, team2, isNeutralGround: false, isExtraTimeRequired: false);
                var simulator = new Simulator(MatchRandomizer.Current);
                var result = simulator.Play(match);
                results.Add(result);

                Console.WriteLine("{0}:{1}\t{2}", result.Score1, result.Score2, string.Join(", ", match.Events.OfType<GoalEvent>()));
            }

            PrintStatistics(results, team1);
        }

        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Justification = "Temporary print method")]
        private void PrintStatistics(IEnumerable<MatchResult> results, Team homeTeam)
        {
            Console.WriteLine();

            Console.WriteLine("Total games: {0}", results.Count());

            int wins = results.Count(m => m.Score1 > m.Score2);
            int draws = results.Count(m => m.Score1 == m.Score2);
            int losses = results.Count(m => m.Score1 < m.Score2);

            Console.WriteLine("Wins: {0}%, Draws: {1}%, Losses: {2}%",
                100 * wins / results.Count(),
                100 * draws / results.Count(),
                100 * losses / results.Count());

            int maxScore = results.Max(m => m.Score1 + m.Score2);
            var matchWithMaxScore = results.First(m => m.Score1 + m.Score2 == maxScore);
            Console.WriteLine("Max score: {0}:{1}", matchWithMaxScore.Score1, matchWithMaxScore.Score2);

            int homeGoals = results.Sum(m => m.Score1);
            int awayGoals = results.Sum(m => m.Score2);
            int totalGoals = results.Sum(m => m.Score1 + m.Score2);
            Console.WriteLine("Home goals:  {0,6} {1,8}", homeGoals, (float)homeGoals / results.Count());
            Console.WriteLine("Away goals:  {0,6} {1,8}", awayGoals, (float)awayGoals / results.Count());
            Console.WriteLine("Total goals: {0,6} {1,8}", totalGoals, (float)totalGoals / results.Count());

            Console.WriteLine();
            Console.WriteLine("No goals:    {0,4}", results.Count(m => m.Score1 + m.Score2 == 0));
            Console.WriteLine("Max 1 goal:  {0,4}", results.Count(m => m.Score1 + m.Score2 <= 1));
            Console.WriteLine("Max 2 goals: {0,4}", results.Count(m => m.Score1 + m.Score2 <= 2));
            Console.WriteLine("Max 3 goals: {0,4}", results.Count(m => m.Score1 + m.Score2 <= 3));
            Console.ReadLine();

            Console.WriteLine();
            Console.WriteLine("Results counts:");

            var matchesByScore = results.OrderBy(m => m.Score2).OrderBy(m => m.Score1 + m.Score2).ToArray();
            int count = 0;
            MatchResult previous = null;

            for (int i = 0; i < matchesByScore.Length; i++)
            {
                var current = matchesByScore[i];

                if (previous == null || (current.Score1 == previous.Score1 && current.Score2 == previous.Score2))
                {
                    count++;
                }
                else
                {
                    Console.WriteLine("{0}:{1} {2,5}% {3,5}", previous.Score1, previous.Score2, 100 * count / results.Count(), count);
                    count = 1;
                }

                previous = current;
            }

            Console.ReadLine();
            Console.WriteLine();
            Console.WriteLine("Goals by position (home only):");

            var positions = Enum.GetValues(typeof(PositionCode)).Cast<PositionCode>();

            foreach (var position in positions)
            {
                int goals = results.Sum(m => m.Events.OfType<GoalEvent>().Count(e =>
                    e.Scorer.Team == homeTeam && e.Scorer.Position.Code == position));
                Console.WriteLine("{0,3} {1,5}% {2,5}", position, 100 * goals / homeGoals, goals);
            }
        }
    }
}
