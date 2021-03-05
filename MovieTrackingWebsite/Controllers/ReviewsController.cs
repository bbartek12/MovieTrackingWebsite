using Microsoft.AspNet.Identity;
using MovieTrackingWebsite.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace MovieTrackingWebsite.Controllers
{
    public class ReviewsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: All Reviews for specific movie
        public ActionResult Index(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            return View(db.Reviews.Where(review => review.PublicMovieId == id));
        }

        // Pass id from MovieInfo Page and create an Empty reivew with this id
        [Authorize]
        public ActionResult CreateReview(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return View(new Review() { PublicMovieId = (int)id });
        }

        // Save movie and attach it to current movie id
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateReview([Bind(Include = "ReviewId, PublicMovieId, Comment, ReviewScore, User")] Review review)
        {
            if (ModelState.IsValid)
            {
                // Store current user
                string userId = User.Identity.GetUserId();
                ApplicationUser user = db.Users.FirstOrDefault(usr => usr.UserName == User.Identity.Name);

                review.User = user;
                Debug.WriteLine(review.User.UserName);
                db.Reviews.Add(review);
                db.SaveChanges();
                return RedirectToAction("MovieInfo", "PublicMovies", new { id = review.PublicMovieId });
            }
            return View();
        }


        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Review review = db.Reviews.Find(id);

            if (review == null)
            {
                return HttpNotFound();
            }
            return View(review);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Review toDelete = db.Reviews.Find(id);

            var movieId = toDelete.PublicMovieId;

            db.Reviews.Remove(toDelete);
            db.SaveChanges();

            if (db.Reviews.Where(review => review.PublicMovieId == movieId).Count() == 0)
            {
                return RedirectToAction("MovieInfo", "PublicMovies", new { id = movieId });
            }

            return RedirectToAction("Index", new { id = movieId });
        }

        // GET: UserLists/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Review reviews = db.Reviews.Find(id);

            if (reviews == null)
            {
                return HttpNotFound();
            }
            return View(reviews);
        }

        // POST: UserLists/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Comment, ReviewScore, PublicMovieId, ReviewId, User")] Review review)
        {
            if (ModelState.IsValid)
            {
                db.Entry(review).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("MovieInfo", "PublicMovies", new { id = review.PublicMovieId });
            }
            return View(review);
        }
    }
}