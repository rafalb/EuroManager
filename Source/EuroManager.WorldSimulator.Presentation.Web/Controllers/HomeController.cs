﻿using System;
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
        public ActionResult Index()
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

                return View(model);
            }
        }

        [HttpGet]
        [Authorize(Roles = UserRole.Administrator)]
        public ActionResult AdvanceDate()
        {
            using (var worldSimulator = new WorldSimulatorService())
            {
                var model = new AdvanceDateModel
                {
                    TargetDate = worldSimulator.GetNextFixtureDate()
                };

                return View(model);
            }
        }

        [HttpPost]
        [Authorize(Roles = UserRole.Administrator)]
        public ActionResult AdvanceDate(AdvanceDateModel model)
        {
            DateTime currentDate;

            using (var worldSimulator = new WorldSimulatorService())
            {
                currentDate = worldSimulator.GetCurrentDate();
            }

            if (model.TargetDate <= currentDate || model.TargetDate > currentDate.AddMonths(3))
            {
                ModelState.AddModelError("TargetDate", "The target date must be between now and 3 months in future.");
            }

            if (ModelState.IsValid)
            {
                while (currentDate < model.TargetDate)
                {
                    using (var worldSimulator = new WorldSimulatorService())
                    {
                        PlayAllTodayFixtures(worldSimulator);
                        currentDate = worldSimulator.GetCurrentDate();

                        if (currentDate < model.TargetDate)
                        {
                            if (worldSimulator.IsSeasonEnd())
                            {
                                worldSimulator.AdvanceSeason();
                            }
                            else
                            {
                                worldSimulator.AdvanceDate();
                            }
                        }
                    }
                }

                return RedirectToAction("Index");
            }
            else
            {
                return View(model);
            }
        }

        public ActionResult MatchResult(int resultId)
        {
            using (var worldSimulator = new WorldSimulatorService())
            {
                var result = worldSimulator.GetMatchResultDetails(resultId);
                var model = new MatchResultModel { Result = result };

                return View(model);
            }
        }

        public ActionResult About()
        {
            return View();
        }

        [ChildActionOnly]
        public ActionResult WorldDate()
        {
            using (var worldSimulator = new WorldSimulatorService())
            {
                ViewBag.WorldDate = worldSimulator.GetCurrentDate();
                return PartialView();
            }
        }

        private static void PlayAllTodayFixtures(WorldSimulatorService worldSimulator)
        {
            var tournaments = worldSimulator.GetTournamentsWithFixturesForToday();

            foreach (var tournament in tournaments)
            {
                while (worldSimulator.PlayNextTodayFixture(tournament.Id))
                {
                }
            }
        }
    }
}
