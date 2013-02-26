using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EuroManager.WorldSimulator.Services;

namespace EuroManager.WorldSimulator.Presentation.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            using (var worldSimulator = new WorldSimulatorService())
            {
                var tournaments = worldSimulator.GetTournamentsWithFixturesForToday();

                if (tournaments.Any())
                {
                    var teamStats = worldSimulator.GetLeagueStandings(tournaments.First().Id);
                    return View(teamStats);
                }
                else
                {
                    return View();
                }
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
