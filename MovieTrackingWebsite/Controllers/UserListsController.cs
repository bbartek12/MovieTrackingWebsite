using MovieTrackingWebsite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace MovieTrackingWebsite.Controllers
{
    public class UserListsController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        [Authorize]
        // GET: UserLists
        public ActionResult Index()
        {

            return View(db.UserLists.FirstOrDefault(user => user.User.UserName == User.Identity.Name).WatchList);
        }

        public ActionResult Delete(int ? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserMovie userMovie = db.UserMovies.Find(id);
            if (userMovie == null)
            {
                return HttpNotFound();
            }

            return View(userMovie);
        }
    }
}