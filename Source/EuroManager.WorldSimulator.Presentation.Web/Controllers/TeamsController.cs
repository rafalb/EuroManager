using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EuroManager.WorldSimulator.Presentation.Web.Models;
using EuroManager.WorldSimulator.Services;

namespace EuroManager.WorldSimulator.Presentation.Web.Controllers
{
    public class TeamsController : Controller
    {
        public ActionResult Team(int id)
        {
            using (var worldSimulator = new WorldSimulatorService())
            {
                var teamModel = new TeamModel
                {
                    Team = worldSimulator.GetTeam(id),
                    Players = worldSimulator.GetTeamPlayers(id),
                    PlayerStats = worldSimulator.GetCombinedPlayerStatsByTeam(id)
                };

                return View(teamModel);
            }
        }
    }
}
