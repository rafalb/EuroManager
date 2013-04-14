using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EuroManager.WorldSimulator.Services;

namespace EuroManager.WorldSimulator.Presentation.Web.Controllers
{
    public class TeamsController : Controller
    {
        public ActionResult Team(int teamId)
        {
            using (var worldSimulator = new WorldSimulatorService())
            {
                var team = worldSimulator.GetTeam(teamId);
                return View(team);
            }
        }
    }
}
