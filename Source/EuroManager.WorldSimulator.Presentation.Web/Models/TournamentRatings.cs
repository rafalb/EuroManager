using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EuroManager.Common.Domain;
using EuroManager.WorldSimulator.Services.Data;

namespace EuroManager.WorldSimulator.Presentation.Web.Models
{
    public class TournamentRatings
    {
        public IEnumerable<PlayerStats> TopScorers { get; set; }

        public IEnumerable<PlayerStats> TopAssistants { get; set; }

        public IEnumerable<PlayerStats> TopRating { get; set; }

        public IDictionary<PositionCode, PlayerStats> RatingByPosition { get; set; }
    }
}
