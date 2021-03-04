using MovieTrackingWebsite.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace MovieTrackingWebsite.Controllers
{
    public class PublicMoviesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: all PublicMovies
        public ActionResult Index()
        {
            return View(db.PublicMovies.ToList());
        }


        // Gets view
        public ActionResult AddMovie()
        {
            return View();
        }

        // Create the movie object
        // Also store any images selected
        // if none are selected the default image is chosen
        [HttpPost]
        public ActionResult AddMovie([Bind(Include = "PublicMovieId, Title, Description, Image, Year")] PublicMovie movie)
        {
            if (ModelState.IsValid)
            {
                HttpPostedFileBase file = Request.Files["Image"]; // Retrieve file

                saveMovieImage(movie, file);

                // Add and save movie inside database
                db.PublicMovies.Add(movie);
                db.SaveChanges();

                return RedirectToAction("Index", "PublicMovies");

            }
            return View(movie);
        }

        // Helper function which stores image in movie objects and also saves the image in specified folder
        private void saveMovieImage(PublicMovie movie, HttpPostedFileBase file)
        {
            string uploads = "~/Uploads";

            // Check if file directory Exists and if not create it
            if (!Directory.Exists(Server.MapPath(uploads)))
            {
                Directory.CreateDirectory(Server.MapPath(uploads));
            }

            if (file != null && file.ContentLength > 0)
            {
                // Create path for Image to be uploaded
                string relativePath = uploads + "/" + file.FileName;
                string physicalPath = Server.MapPath(relativePath);

                // Store path in movie object
                movie.Image = relativePath;

                // If folder does not contain image, add it to the folder
                if (!Server.MapPath(uploads).Contains(file.FileName))
                {
                    file.SaveAs(physicalPath);
                }
            }
            else // file is not selected
            {
                // If no image is selected make the image to default
                if (String.IsNullOrEmpty(file.FileName))
                {
                    movie.Image = uploads + "/default.jpg";
                }
            }
        }


        // Create a userlist for the current logged in user
        public void createUserList()
        {

            ApplicationUser currUser = db.Users.FirstOrDefault(user => user.UserName == User.Identity.Name);

            if (!db.UserLists.Where(user => user.User.UserName == User.Identity.Name).Any())
            {
                db.UserLists.Add(new UserList()
                {
                    User = currUser
                });
                db.SaveChanges();
            }

        }


        // Retrieve data from UserMovie and PublicMovie to store in a ViewModel
        public ActionResult MovieInfo(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }

            //    createUserList(); // Create user if one does not already exist

            PublicMovie publicMovie = db.PublicMovies.Find(id); // Get selected movie

            if (publicMovie == null)
            {
                return HttpNotFound();
            }

            // Create movie model to store current movie
            MovieDetailViewModel movieDetailViewModel = new MovieDetailViewModel()
            {
                Movie = publicMovie
            };

            // Get logged in user
            var currUser = db.UserLists.FirstOrDefault(user => user.User.UserName == User.Identity.Name);

            // If current user is logged in and contains a movie with title of selected movie update the database
            if (currUser != null && currUser.WatchList.Where(movie => movie.Title == publicMovie.Title).Any())
            {
                movieDetailViewModel.Status = currUser.WatchList.FirstOrDefault(movie => movie.Title == publicMovie.Title).Status;
                movieDetailViewModel.UserMovieId = currUser.UserListId;
            }

            movieDetailViewModel.ReviewsList = db.Reviews.Where(review => review.PublicMovieId == publicMovie.PublicMovieId).Take(3).ToList(); // Get review for current movie

            return View(movieDetailViewModel);
        }


        // When user selects and submits the movies status for the current movie it saves this inside the user's watchlist with the status selected
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MovieInfo([Bind(Include = "Movie, Status")] MovieDetailViewModel movieDetailViewModel)
        {

            if (ModelState.IsValid)
            {

                // Create User list if one does not already exist
                createUserList();

                // Get logged in user
                var currUser = db.UserLists.FirstOrDefault(movie => movie.User.UserName == User.Identity.Name);

                // If image is empty set it to default
                if (String.IsNullOrEmpty(movieDetailViewModel.Movie.Image))
                {
                    movieDetailViewModel.Movie.Image = "~/Uploads/defualt.jpg";
                }

                // If logged in add current movie to watchlist
                // Unnecessary since Authorization required
                if (currUser != null)
                {
                    addMovieToUserList(movieDetailViewModel, currUser);
                }

                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(movieDetailViewModel);
        }


        // Helper function which checks if movies exists in a user's watchlist
        // If it does not then it creates a new instance of it
        // If it does then it overwrites the data inside the database
        private static void addMovieToUserList(MovieDetailViewModel movieDetailViewModel, UserList currUser)
        {
            // If Movie is not in the list
            if (!currUser.WatchList.Where(movie => movie.Title == movieDetailViewModel.Movie.Title).Any())
            {
                currUser.WatchList.Add(new UserMovie()
                {
                    Title = movieDetailViewModel.Movie.Title,
                    Image = movieDetailViewModel.Movie.Image,
                    Status = movieDetailViewModel.Status,
                    PublicMovieId = movieDetailViewModel.Movie.PublicMovieId
                });
            }
            else
            {
                // Update status for current user
                currUser.WatchList.FirstOrDefault(movie => movie.PublicMovieId == movieDetailViewModel.Movie.PublicMovieId).Status = movieDetailViewModel.Status;
            }
        }

        // Get movie object to delete
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            PublicMovie publicMovie = db.PublicMovies.Find(id);

            if (publicMovie == null)
            {
                return HttpNotFound();
            }

            return View(publicMovie);
        }

        // Allow user to change everything besides the id
        // If no image is chosen then it sets to default
        [HttpPost]
        public ActionResult Edit([Bind(Include = "PublicMovieId, Title, Description, Year")] PublicMovie publicMovie)
        {
            if (ModelState.IsValid)
            {

                // Get List of all usermovies with same movie id
                var userMovie = db.UserMovies.Where(movie => movie.PublicMovieId == publicMovie.PublicMovieId).ToList();

                HttpPostedFileBase file = Request.Files["Image"];

                saveMovieImage(publicMovie, file);

                // Replace the image location for every instance of selected movie in UserMovie
                userMovie.ForEach(movie => movie.Image = publicMovie.Image);

                // Rename title for every instance of usermovie
                userMovie.ForEach(movie => movie.Title = publicMovie.Title);

                db.Entry(publicMovie).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(publicMovie);
        }
        
        // Get view
        public ActionResult Search()
        {
            return View();
        }

        // Search through titles and descriptions which contains the select word(s)
        [HttpPost]
        public ActionResult Search(string searchQuery)
        {
            // If search is empty return all movies list
            if (String.IsNullOrEmpty(searchQuery))
            {
                return RedirectToAction("Index");
            }

            // Otherwise apply the search string
            return View("Index", db.PublicMovies.Where(movie => movie.Title.Contains(searchQuery) || movie.Description.Contains(searchQuery)).ToList());
        }

        // Get movie object to delete
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            PublicMovie movie = db.PublicMovies.Find(id);

            if (movie == null)
            {
                return HttpNotFound();
            }

            return View(movie);
        }

        // Delete movie from database after user submits form
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PublicMovie publicMovie = db.PublicMovies.Find(id);

            // Remove from all user's lists
            db.UserMovies.RemoveRange(db.UserMovies.Where(movie => movie.PublicMovieId == id));

            // Remove the public version of movie
            db.PublicMovies.Remove(publicMovie);

            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // Release umanaged resources
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