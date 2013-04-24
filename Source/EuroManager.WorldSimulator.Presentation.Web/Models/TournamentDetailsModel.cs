using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EuroManager.WorldSimulator.Services.Data;

namespace EuroManager.WorldSimulator.Presentation.Web.Models
{
    public class TournamentResultsModel
    {
        public IEnumerable<Tournament> Tournaments { get; set; }

        public int SelectedTournamentId { get; set; }

        public TournamentResults Results { get; set; }

        public TournamentRatings Ratings { get; set; }
    }
}