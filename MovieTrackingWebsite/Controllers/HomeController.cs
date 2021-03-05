using MovieTrackingWebsite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MovieTrackingWebsite.Controllers
{
    public class HomeController : Controller
    {

        ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            // Prevent from logged in user from seeing a page to join or login
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "PublicMovies");
            }
            return View();
        }


        // Create a userlist for the registered user
        // Register from Accountcontroller redirects to this method on account creation (this is a workaround since unsure how to bring appdbcontext into account controller safely)
        public void CreateUserList()
        {

            ApplicationUser currUser = db.Users.FirstOrDefault(user => user.UserName == User.Identity.Name);

            db.UserLists.Add(new UserList()
            {
                User = currUser
            });
            db.SaveChanges();

        }



        // Used to create user list upon registration
        public ActionResult Register()
        {
            CreateUserList();

            return RedirectToAction("Index", "PublicMovies");
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