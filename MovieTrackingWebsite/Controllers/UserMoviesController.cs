using MovieTrackingWebsite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace MovieTrackingWebsite.Controllers
{
    public class UserMoviesController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
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


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            // Get current user
            UserMovie userMovie = db.UserMovies.Find(id);
            db.UserMovies.Remove(userMovie);
            db.SaveChanges();

            return RedirectToAction("Index", "UserLists");
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}