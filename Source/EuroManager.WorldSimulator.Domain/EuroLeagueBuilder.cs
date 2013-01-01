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

        public NationalLeagueSeason CreateEuroLeague(IEnumerable<Team> championsLeagueClubs, IEnumerable<Team> europaLeagueClubs)
        {
            var league = new NationalLeague(world, "European Cups");

            var championsLeague = new League(league, "Champions League", 1, DayOfWeek.Wednesday, 2, hasReturnRound: false);
            championsLeague.AutomaticRelegationCount = 2;
            championsLeague.ConditionalRelegationCount = 2;
            league.AddDivision(championsLeague);

            var europaCup = new Cup(league, "Europa League", 2, DayOfWeek.Thursday, 2,
                new List<CupStage>
                {
                    new GroupStage(groupCount: 4, groupTeamCount: 4, groupPromotedCount: 2, isNeutralGround: false, hasReturnRound: true),
                    new TieKnockoutStage(4),
                    new TieKnockoutStage(2)
                });
            league.AddDivision(europaCup);

            league.PlayOffs = new LeaguePlayOffs(league, "European Play-Offs", DayOfWeek.Wednesday);

            DateTime startDate = new DateTime(world.StartYear, 08, 21);
            DateTime endDate = new DateTime(world.StartYear + 1, 05, 16);

            var leagueSeason = new NationalLeagueSeason(league, startDate, endDate);
            leagueSeason.AddDivisionSeason(new LeagueSeason((League)league.Divisions[0], startDate, endDate, championsLeagueClubs));
            leagueSeason.AddDivisionSeason(new CupSeason((Cup)league.Divisions[1], startDate, endDate, europaLeagueClubs));

            return leagueSeason;
        }
    }
}
