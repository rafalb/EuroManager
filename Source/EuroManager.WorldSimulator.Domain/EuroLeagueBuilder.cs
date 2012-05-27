using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EuroManager.Common;

namespace EuroManager.WorldSimulator.Domain
{
    public class EuroLeagueBuilder
    {
        private World world;

        public EuroLeagueBuilder(World world)
        {
            this.world = world;
        }

        public NationalLeagueSeason CreateEuroLeague(IEnumerable<Team> division1Clubs, IEnumerable<Team> division2Clubs, IEnumerable<Team> division3Clubs)
        {
            var league = new NationalLeague(world, "EuroLeague");
            var euroLeagueBottomDivisionStages = new List<CupStage>
            {
                new GroupStage(groupCount: 4, groupTeamCount: 4, groupPromotedCount: 2, isNeutralGround: false, hasReturnRound: true),
                new TieKnockoutStage(4),
                new TieKnockoutStage(2),
                new KnockoutStage(1)
            };

            league.AddDivision(new League(league, "EuroLeague Division 1", 1, DayOfWeek.Wednesday, 2));
            league.AddDivision(new League(league, "EuroLeague Division 2", 2, DayOfWeek.Wednesday, 2));
            league.AddDivision(new Cup(league, "EuroLeague Division 3", 3, DayOfWeek.Wednesday, 2, euroLeagueBottomDivisionStages));

            league.PlayOffs = new LeaguePlayOffs(league, "EuroLeague Play-Offs", DayOfWeek.Wednesday);

            DateTime startDate = new DateTime(world.StartYear, 08, 21);
            DateTime endDate = new DateTime(world.StartYear + 1, 05, 16);

            var leagueSeason = new NationalLeagueSeason(league, startDate, endDate);
            leagueSeason.AddDivisionSeason(new LeagueSeason((League)league.Divisions[0], startDate, endDate, division1Clubs));
            leagueSeason.AddDivisionSeason(new LeagueSeason((League)league.Divisions[1], startDate, endDate, division2Clubs));
            leagueSeason.AddDivisionSeason(new CupSeason((Cup)league.Divisions[2], startDate, endDate, division3Clubs));

            return leagueSeason;
        }
    }
}
