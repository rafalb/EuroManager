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

            PlayerStats = new List<PlayerTournamentStats>();
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

        public int Played { get; private set; }

        public virtual List<PlayerTournamentStats> PlayerStats { get; private set; }

        public void ApplyResult(MatchResult result)
        {
            if (result.Team1 == Team || result.Team2 == Team)
            {
                Played += 1;

                if (!PlayerStats.Any())
                {
                    PlayerStats.AddRange(Team.Players.Select(p => new PlayerTournamentStats(TournamentSeason, p)).ToList());
                }

                foreach (var playerStats in PlayerStats)
                {
                    playerStats.ApplyResult(result);
                }
            }
        }
    }
}
