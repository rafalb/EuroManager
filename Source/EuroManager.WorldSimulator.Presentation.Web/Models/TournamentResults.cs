using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EuroManager.WorldSimulator.Services.Data;

namespace EuroManager.WorldSimulator.Presentation.Web.Models
{
    public class TournamentResults
    {
        public Tournament Tournament { get; set; }

        public IEnumerable<MatchResult> MatchResults { get; set; }

        public IEnumerable<TeamStats> Standings { get; set; }
    }
}