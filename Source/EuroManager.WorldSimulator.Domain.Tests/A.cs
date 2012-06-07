using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using EuroManager.WorldSimulator.Domain.Tests.Builders;

namespace EuroManager.WorldSimulator.Domain.Tests
{
    [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "A", Justification = "Object Mother design pattern")]
    public static class A
    {
        public static DateTime Date
        {
            get { return new DateTime(2013, 02, 08); }
        }

        public static WorldBuilder World
        {
            get { return new WorldBuilder(); }
        }

        public static TeamBuilder Team
        {
            get { return new TeamBuilder(); }
        }

        public static PlayerBuilder Player
        {
            get { return new PlayerBuilder(); }
        }

        public static MatchResultBuilder MatchResult
        {
            get { return new MatchResultBuilder(); }
        }

        public static NationalLeagueBuilder NationalLeague
        {
            get { return new NationalLeagueBuilder(); }
        }

        public static NationalLeagueSeasonBuilder NationalLeagueSeason
        {
            get { return new NationalLeagueSeasonBuilder(); }
        }

        public static LeagueBuilder League
        {
            get { return new LeagueBuilder(); }
        }

        public static LeagueSeasonBuilder LeagueSeason
        {
            get { return new LeagueSeasonBuilder(); }
        }

        public static CupBuilder Cup
        {
            get { return new CupBuilder(); }
        }

        public static CupSeasonBuilder CupSeason
        {
            get { return new CupSeasonBuilder(); }
        }

        public static GroupStageSeasonBuilder GroupStageSeason
        {
            get { return new GroupStageSeasonBuilder(); }
        }

        public static KnockoutStageSeasonBuilder KnockoutStageSeason
        {
            get { return new KnockoutStageSeasonBuilder(); }
        }

        public static LeaguePlayOffsSeasonBuilder LeaguePlayOffsSeason
        {
            get { return new LeaguePlayOffsSeasonBuilder(); }
        }
    }
}
