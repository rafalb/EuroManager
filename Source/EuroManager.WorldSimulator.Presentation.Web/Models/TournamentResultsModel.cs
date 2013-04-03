using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EuroManager.WorldSimulator.Presentation.Web.Models
{
    public class TournamentResultsModel
    {
        public DateTime CurrentDate { get; set; }

        public bool AllowAdvanceDate { get; set; }

        public bool AdvanceByMonth { get; set; }

        public IEnumerable<TournamentResults> TournamentResults { get; set; }
    }
}