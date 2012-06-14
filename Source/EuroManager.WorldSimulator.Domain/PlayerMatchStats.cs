using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace EuroManager.WorldSimulator.Domain
{
    public class PlayerMatchStats : IEntity
    {
        public PlayerMatchStats(Player player, int rating, int goals, int assists)
        {
            Player = player;
            Rating = rating;
            Goals = goals;
            Assists = assists;
        }

        protected PlayerMatchStats()
        {
        }

        public int Id { get; private set; }

        [Timestamp]
        public byte[] Version { get; private set; }

        public int? PlayerId { get; private set; }

        public virtual Player Player { get; private set; }

        public int Rating { get; private set; }

        public int Goals { get; private set; }

        public int Assists { get; private set; }
    }
}
