using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MovieTrackingWebsite.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            // Prevent from logged in user from seeing a page to join or login
            if (User.Identity.IsAuthenticated)
            {
               return RedirectToAction("Index", "PublicMovies");
            }
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}