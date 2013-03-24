using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EuroManager.WorldSimulator.Presentation.Web.Models;
using EuroManager.WorldSimulator.Services;

namespace EuroManager.WorldSimulator.Presentation.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            using (var worldSimulator = new WorldSimulatorService())
            {
                var tournaments = worldSimulator.GetTournamentsWithResultsForToday();

                if (tournaments.Any())
                {
                    var results = tournaments.Select(t =>
                        new TournamentResults
                        {
                            Tournament = t,
                            MatchResults = worldSimulator.GetTodayMatchResults(t.Id),
                            TeamStats = worldSimulator.GetLeagueStandings(t.Id)
                        });

                    var model = new TournamentResultsModel
                    {
                        CurrentDate = worldSimulator.GetCurrentDate(),
                        TournamentResults = results
                    };

                    return View(model);
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
