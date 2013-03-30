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
        [HttpGet]
        public ActionResult Index(bool advanceByMonth = false)
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
                        AdvanceByMonth = advanceByMonth,
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

        [HttpPost]
        public ActionResult Index(TournamentResultsModel model)
        {
            DateTime targetDate;
            DateTime currentDate;

            using (var worldSimulator = new WorldSimulatorService())
            {
                currentDate = worldSimulator.GetCurrentDate();
                targetDate = model.AdvanceByMonth ? currentDate.AddMonths(1) : currentDate.AddDays(1);
            }

            while (currentDate < targetDate)
            {
                using (var worldSimulator = new WorldSimulatorService())
                {
                    PlayFixturesAndAdvanceDate(worldSimulator);
                    currentDate = worldSimulator.GetCurrentDate();
                }
            }

            return RedirectToAction("Index", new { model.AdvanceByMonth });
        }

        public ActionResult MatchResult(int resultId)
        {
            using (var worldSimulator = new WorldSimulatorService())
            {
                var result = worldSimulator.GetMatchResult(resultId);
                var model = new MatchResultModel { Result = result };

                return View(model);
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

        private static void PlayFixturesAndAdvanceDate(WorldSimulatorService worldSimulator)
        {
            var tournaments = worldSimulator.GetTournamentsWithFixturesForToday();

            while (!tournaments.Any())
            {
                if (worldSimulator.IsSeasonEnd())
                {
                    worldSimulator.AdvanceSeason();
                }
                else
                {
                    worldSimulator.AdvanceDate();
                }

                tournaments = worldSimulator.GetTournamentsWithFixturesForToday();
            }

            foreach (var tournament in tournaments)
            {
                while (worldSimulator.PlayNextTodayFixture(tournament.Id)) ;
            }
        }
    }
}
