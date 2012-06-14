using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace EuroManager.WorldSimulator.Domain
{
    public class PlayerTournamentStats : IEntity
    {
        public PlayerTournamentStats(TournamentSeason tournamentSeason, Player player)
        {
            TournamentSeason = tournamentSeason;
            Player = player;
            PlayerName = player.Name;
        }

        protected PlayerTournamentStats()
        {
        }

        public int Id { get; private set; }

        [Timestamp]
        public byte[] Version { get; private set; }

        public int? TournamentSeasonId { get; private set; }

        public virtual TournamentSeason TournamentSeason { get; private set; }

        public int? PlayerId { get; private set; }

        public virtual Player Player { get; private set; }

        public string PlayerName { get; private set; }

        public int Played { get; private set; }

        public double AverageRating { get; private set; }

        public int Goals { get; private set; }

        public int Assists { get; private set; }

        public void ApplyResult(MatchResult result)
        {
            PlayerMatchStats matchStats = result.PlayersStats.FirstOrDefault(s => s.Player == Player);

            if (matchStats != null)
            {
                Played += 1;
                Goals += matchStats.Goals;
                Assists += matchStats.Assists;

                AverageRating = (AverageRating * (Played - 1) + matchStats.Rating) / Played;
            }
        }
    }
}
