using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace EuroManager.WorldSimulator.Domain
{
    public class TeamTournamentStats : IEntity
    {
        public TeamTournamentStats(TournamentSeason tournamentSeason, Team team)
        {
            TournamentSeason = tournamentSeason;
            Team = team;
        }

        protected TeamTournamentStats()
        {
        }

        public int Id { get; private set; }

        [Timestamp]
        public byte[] Version { get; private set; }

        public int? TournamentSeasonId { get; private set; }

        public virtual TournamentSeason TournamentSeason { get; private set; }

        public int? TeamId { get; private set; }

        public virtual Team Team { get; private set; }

        public void ApplyResult(MatchResult result)
        {
        }
    }
}
