using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Text;

namespace EuroManager.WorldSimulator.Domain
{
    public class Fixture : IEntity
    {
        public Fixture(TournamentSeason tournamentSeason, DateTime date, Team team1, Team team2, bool isNeutralGround, bool requiresExtraTime)
            : this(tournamentSeason.World, tournamentSeason.Id, date, team1, team2, isNeutralGround, requiresExtraTime)
        {
        }

        public Fixture(DateTime date, Fixture firstLeg)
            : this(firstLeg.World, firstLeg.TournamentSeasonId, date, firstLeg.Team2, firstLeg.Team1, false, true)
        {
            FirstLeg = firstLeg;
        }

        protected Fixture()
        {
        }

        [SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors", Justification = "Required by Entity Framework")]
        private Fixture(World world, int tournamentSeasonId, DateTime date, Team team1, Team team2, bool isNeutralGround, bool requiresExtraTime)
        {
            TournamentSeasonId = tournamentSeasonId;
            World = world;
            Date = date;
            Team1 = team1;
            Team2 = team2;
            Team1Name = team1.Name;
            Team2Name = team2.Name;
            IsNeutralGround = isNeutralGround;
            RequiresExtraTime = requiresExtraTime;
        }

        public int Id { get; private set; }

        [Timestamp]
        public byte[] Version { get; private set; }

        public int? WorldId { get; private set; }

        public World World { get; private set; }

        public int TournamentSeasonId { get; private set; }
        
        public DateTime Date { get; private set; }

        public int? Team1Id { get; private set; }

        public virtual Team Team1 { get; set; }

        public string Team1Name { get; private set; }

        public int? Team2Id { get; private set; }

        public virtual Team Team2 { get; set; }

        public string Team2Name { get; private set; }

        public bool IsNeutralGround { get; private set; }

        public bool RequiresExtraTime { get; private set; }

        public int? ResultId { get; set; }

        public virtual MatchResult Result { get; set; }

        public int? FirstLegId { get; private set; }

        public virtual Fixture FirstLeg { get; private set; }

        public bool IsSecondLeg
        {
            get { return FirstLeg != null; }
        }

        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "{0} - {1}", Team1Name, Team2Name);
        }
    }
}
