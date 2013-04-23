using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EuroManager.WorldSimulator.Presentation.Web.Models;
using EuroManager.WorldSimulator.Services;

namespace EuroManager.WorldSimulator.Presentation.Web.Controllers
{
    public class ResultsController : Controller
    {
        public ActionResult Recent()
        {
            var model = LoadRecentTournamentResults();

            if (ControllerContext.IsChildAction)
            {
                return PartialView(model);
            }
            else
            {
                return View(model);
            }
        }

        public ActionResult Tournament(int id = 0)
        {
            using (var worldSimulator = new WorldSimulatorService())
            {
                var tournaments = worldSimulator.GetTournaments();
                var model = new TournamentDetailsModel { Tournaments = tournaments, SelectedTournamentId = id };

                return View(model);
            }
        }

        public ActionResult Match(int id)
        {
            using (var worldSimulator = new WorldSimulatorService())
            {
                var result = worldSimulator.GetMatchResultDetails(id);
                var model = new MatchResultModel { Result = result };

                return View(model);
            }
        }

        private TournamentResultsModel LoadRecentTournamentResults()
        {
            using (var worldSimulator = new WorldSimulatorService())
            {
                var tournaments = worldSimulator.GetTournamentsWithResultsForToday();

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

                return model;
            }
        }
    }
}
