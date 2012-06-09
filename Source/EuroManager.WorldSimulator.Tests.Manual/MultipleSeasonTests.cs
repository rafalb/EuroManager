using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using EuroManager.Common;
using EuroManager.WorldSimulator.Services;
using EuroManager.WorldSimulator.Services.Data;

namespace EuroManager.WorldSimulator.Tests.Manual
{
    public class MultipleSeasonTests
    {
        private static readonly string[] TrackedTeams = { "Barcelona", "Real M", "Newcastle", "Wisla", "Polonia" };

        public void Perform()
        {
            DateTime nextDate;

            using (var worldSimulator = new WorldSimulatorService())
            {
                nextDate = worldSimulator.GetCurrentDate();
            }

            while (true)
            {
                using (var worldSimulator = new WorldSimulatorService())
                {
                    DateTime date = worldSimulator.GetCurrentDate();

                    var tournaments = worldSimulator.GetTournamentsWithFixturesForToday().OrderBy(t => t.Level).ToArray();

                    foreach (var tournament in tournaments)
                    {
                        PlayTournament(worldSimulator, date, tournament);

                        if (date >= nextDate)
                        {
                            Console.Write("> ");
                            string answer = Console.ReadLine();
                            int weeks = 0;

                            if (int.TryParse(answer, out weeks))
                            {
                                nextDate = date.AddDays(7 * weeks);
                            }
                            else
                            {
                                nextDate = date;
                            }
                        }
                    }
                }

                using (var worldSimulator = new WorldSimulatorService())
                {
                    if (worldSimulator.IsSeasonEnd())
                    {
                        worldSimulator.AdvanceSeason();
                    }
                    else
                    {
                        worldSimulator.AdvanceDate();
                    }
                }
            }
        }

        private void PlayTournament(WorldSimulatorService worldSimulator, DateTime date, Tournament tournament)
        {
            Console.Clear();
            Console.WriteLine("{0,-59} {1,59}", tournament.Name, date.ToLongDateString());
            Console.WriteLine();

            while (worldSimulator.PlayNextTodayFixture(tournament.Id))
            {
                var result = worldSimulator.GetLastMatchResult(tournament.Id);
                PrintMatchResult(result);
            }

            var standings = worldSimulator.GetLeagueStandings(tournament.Id);

            if (standings.Any())
            {
                Console.WriteLine();
                PrintStandings(standings);
            }
            else
            {
                standings = worldSimulator.GetCupGroupStageStandings(tournament.Id);

                if (standings.Any())
                {
                    Console.WriteLine();
                    PrintStandings(standings);
                }
            }

            Console.WriteLine();

            var playerStats = worldSimulator.GetTopPlayerStats(tournament.Id, 20);

            foreach (var stats in playerStats)
            {
                Console.WriteLine("          {0,-4} {1,-20}{2,-25}{3:0.00}", stats.Position, stats.Name, stats.TeamName, stats.Rating);
            }

            Console.WriteLine();
        }

        private void PrintMatchResult(MatchResult result)
        {
            Console.ForegroundColor = ConsoleColor.White;
            PrintTeamName("{0,57}", result.Team1Name);
            Console.Write("{0,2}:{1,-2}", result.Score1, result.Score2);
            PrintTeamName("{0,-57}", result.Team2Name);
            Console.WriteLine();
            Console.ResetColor();

            if (result.Goals1.Any() || result.Goals2.Any())
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("{0,57}     {1,-57}", ConcatTeamGoals(result.Goals1), ConcatTeamGoals(result.Goals2));
                Console.ResetColor();
            }

            Console.WriteLine();
        }

        private string ConcatTeamGoals(IEnumerable<Goal> goals)
        {
            var goalsStrings = goals.Select(g =>
                string.Format(CultureInfo.CurrentCulture, g.Extended == 0 ? "{0} {1}" : "{0} {1}+{2}", g.ScorerName, g.Minute, g.Extended)).ToArray();
            return string.Join(", ", goalsStrings);
        }

        private void PrintStandings(IEnumerable<TeamStats> standings)
        {
            Console.WriteLine();
            Console.WriteLine("                          {0,2}  {1,-23}{2,3}{3,4}  {4,3}{5,3}{6,3}  {7,3}-{8,-3}",
                "#", "Team", "M", "Pts", "W", "D", "L", "F", "A");
            Console.WriteLine("                          -----------------------------------------------------");

            int position = 1;
            int previousGroupNumber = standings.First().GroupNumber;

            foreach (var standing in standings)
            {
                if (standing.GroupNumber != previousGroupNumber)
                {
                    position = 1;
                    Console.WriteLine("                          -----------------------------------------------------");
                }

                Console.Write("                          {0,2}. ", position);
                PrintTeamName("{0,-23}", standing.TeamName);
                Console.WriteLine("{0,3}{1,4}  {2,3}{3,3}{4,3}  {5,3}-{6,-3}",
                    standing.Played, standing.Points, standing.Wins, standing.Draws, standing.Losses,
                    standing.GoalsFor, standing.GoalsAgainst);

                previousGroupNumber = standing.GroupNumber;
                position++;
            }
        }

        private void PrintTeamName(string format, string name)
        {
            bool isTrackedTeam = TrackedTeams.Any(t => name.Contains(t));

            if (isTrackedTeam)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
            }

            Console.Write(format, name);

            if (isTrackedTeam)
            {
                Console.ResetColor();
            }
        }
    }
}
