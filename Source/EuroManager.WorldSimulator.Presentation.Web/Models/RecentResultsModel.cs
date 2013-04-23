using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EuroManager.WorldSimulator.Presentation.Web.Models
{
    public class RecentResultsModel
    {
        public IEnumerable<TournamentResults> TournamentResults { get; set; }
    }
}