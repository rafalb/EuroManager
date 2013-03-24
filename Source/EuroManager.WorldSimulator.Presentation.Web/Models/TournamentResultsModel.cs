using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EuroManager.WorldSimulator.Presentation.Web.Models
{
    public class TournamentResultsModel
    {
        public DateTime CurrentDate { get; set; }

        public IEnumerable<TournamentResults> TournamentResults { get; set; }
    }
}