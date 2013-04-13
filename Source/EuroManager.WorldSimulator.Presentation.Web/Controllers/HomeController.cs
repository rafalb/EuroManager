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
            return View();
        }

        public ActionResult About()
        {
            return View();
        }
    }
}
