using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EuroManager.WorldSimulator.Services.Data;

namespace EuroManager.WorldSimulator.Presentation.Web.Models
{
    public class TournamentDetailsModel
    {
        public IEnumerable<Tournament> Tournaments { get; set; }

        public int SelectedTournamentId { get; set; }
    }
}