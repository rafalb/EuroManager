﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace EuroManager.WorldSimulator.Domain
{
    public class PlayerTournamentStats : IEntity
    {
        public PlayerTournamentStats(Player player)
        {
            Player = player;
        }

        protected PlayerTournamentStats()
        {
        }

        public int Id { get; private set; }

        [Timestamp]
        public byte[] Version { get; private set; }

        public int? PlayerId { get; private set; }

        public virtual Player Player { get; private set; }

        public int Played { get; private set; }

        public double AverageRating { get; private set; }

        public void ApplyResult(MatchResult result)
        {
            PlayerMatchStats matchStats = result.PlayersStats.FirstOrDefault(s => s.Player == Player);

            if (matchStats != null)
            {
                Played += 1;
                AverageRating = (AverageRating * (Played - 1) + matchStats.Rating) / Played;
            }
        }
    }
}
