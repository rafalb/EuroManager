using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using EuroManager.Common;

namespace EuroManager.WorldSimulator.Domain
{
    public class Scheduler
    {
        private IRandomGenerator random = RandomGenerator.Current;

        public int CalculateRoundCount(int teamCount)
        {
            Contract.Requires(teamCount > 1);

            return 2 * (checked(teamCount - 1) / 2) + 1;
        }

        public IEnumerable<DateTime> ScheduleRoundDates(DateTime startDate, DateTime endDate, DayOfWeek dayOfWeek, int frequency, params int[] stageRoundCounts)
        {
            int roundCount = stageRoundCounts.Sum();
            int stageCount = stageRoundCounts.Length;

            var dates = new List<DateTime>();
            DateTime date = startDate.Next(dayOfWeek);
            int weeks = (endDate - date).Days / 7 + 1;
            int breakLength = weeks - frequency * roundCount;

            int halfStageIndex = CalculateHalfStageIndex(stageRoundCounts, roundCount, stageCount);

            for (int i = 0; i < stageCount; i++)
            {
                var roundDates = new DateTime[stageRoundCounts[i]];

                for (int j = 0; j < roundDates.Length; j++)
                {
                    roundDates[j] = date;
                    date = date.AddDays(checked(7 * frequency)).Date;
                }

                dates.AddRange(roundDates);

                if (i == halfStageIndex)
                {
                    date = date.AddDays(7 * breakLength).Date;
                }
            }

            return dates;
        }
        
        public IEnumerable<Fixture> ScheduleLeagueFixtures(TournamentSeason season, IEnumerable<Team> teams,
            DateTime startDate, DateTime endDate, DayOfWeek dayOfWeek, int frequency)
        {
            int roundCount = CalculateRoundCount(teams.Count());
            var dates = ScheduleRoundDates(startDate, endDate, dayOfWeek, frequency, roundCount, roundCount);

            return ScheduleLeagueFixtures(season, dates, true, teams);
        }

        public IEnumerable<Fixture> ScheduleLeagueFixtures(TournamentSeason season, IEnumerable<DateTime> dates, bool hasReturnRound, IEnumerable<Team> teams)
        {
            int round = 0;
            int roundCount = CalculateRoundCount(teams.Count());

            teams = random.Sort(teams);
            var fixtures = new List<Fixture>();

            foreach (var date in dates)
            {
                var roundFixtures = ScheduleRoundFixtures(season, date, round % roundCount, hasReturnRound && round >= roundCount, teams);
                fixtures.AddRange(roundFixtures);

                round++;
            }

            return fixtures;
        }

        private IEnumerable<Fixture> ScheduleRoundFixtures(TournamentSeason season, DateTime date, int round, bool isReturnRound, IEnumerable<Team> teams)
        {
            int count;
            var fixtures = new List<Fixture>();
            Team restingTeam = teams.ElementAt(round);
            Team lastTeam = null;

            if (teams.Count() % 2 == 0)
            {
                count = teams.Count() - 1;
                lastTeam = teams.Last();
            }
            else
            {
                count = teams.Count();
            }

            for (int i = 1; i <= (count - 1) / 2; i++)
            {
                Team team1 = teams.ElementAt((round + i) % count);
                Team team2 = teams.ElementAt((round + count - i) % count);

                if (round % 2 == (isReturnRound ? 1 : 0))
                {
                    fixtures.Add(new Fixture(season, date, team1, team2, false, false));
                }
                else
                {
                    fixtures.Add(new Fixture(season, date, team2, team1, false, false));
                }
            }

            if (lastTeam != null)
            {
                if (round % 2 == (isReturnRound ? 1 : 0))
                {
                    fixtures.Add(new Fixture(season, date, lastTeam, restingTeam, false, false));
                }
                else
                {
                    fixtures.Add(new Fixture(season, date, restingTeam, lastTeam, false, false));
                }
            }

            return random.Sort(fixtures);
        }

        private static int CalculateHalfStageIndex(int[] stageRoundCounts, int roundCount, int stageCount)
        {
            if (stageCount == 1)
            {
                return -1;
            }
            else
            {
                int stageIndex = 0;
                int halfRoundCount = 0;

                while (stageIndex < stageCount && halfRoundCount < roundCount / 2)
                {
                    halfRoundCount += stageRoundCounts[stageIndex];
                    stageIndex++;
                }

                return stageIndex - 1;
            }
        }
    }
}
