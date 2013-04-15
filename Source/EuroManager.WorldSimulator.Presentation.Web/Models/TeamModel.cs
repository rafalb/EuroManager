using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EuroManager.WorldSimulator.Services.Data;

namespace EuroManager.WorldSimulator.Presentation.Web.Models
{
    public class TeamModel
    {
        public Team Team { get; set; }

        public IEnumerable<Player> Players { get; set; }
    }
}