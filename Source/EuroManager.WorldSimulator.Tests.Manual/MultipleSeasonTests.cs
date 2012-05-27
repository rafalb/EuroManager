using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using EuroManager.WorldSimulator.Services;
using EuroManager.WorldSimulator.Services.Data;

namespace EuroManager.WorldSimulator.Tests.Manual
{
    public class MultipleSeasonTests
    {
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
        }

        private void PrintMatchResult(MatchResult result)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("{0,57}{1,2}:{2,-2}{3,-57}", result.Team1Name, result.Score1, result.Score2, result.Team2Name);
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

                Console.WriteLine("                          {0,2}. {1,-23}{2,3}{3,4}  {4,3}{5,3}{6,3}  {7,3}-{8,-3}",
                    position, standing.TeamName, standing.Played, standing.Points,
                    standing.Wins, standing.Draws, standing.Losses, standing.GoalsFor, standing.GoalsAgainst);

                previousGroupNumber = standing.GroupNumber;
                position++;
            }
        }
    }
}
