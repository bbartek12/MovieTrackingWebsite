using MovieTrackingWebsite.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MovieTrackingWebsite.Controllers
{
    public class PublicMoviesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: PublicMovie
        public ActionResult Index()
        {
            return View(db.PublicMovies.ToList());
        }

        public ActionResult AddMovie()
        {
            return View();
        }
        

        // Create the movie object
        // Also store any images selected
        // if none are selected the default image is chosen
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddMovie([Bind(Include = "PublicMovieId, Title, Description, Image, Year")] PublicMovie movie)
        {
            if (ModelState.IsValid)
            {
                HttpPostedFileBase file = Request.Files["Image"]; // Retrieve file

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

                    Debug.WriteLine(relativePath);

                    // Store path in movie object
                    movie.Image = relativePath;

                    // If folder does not contain image, add it to the folder
                    if (!Server.MapPath(uploads).Contains(file.FileName))
                    {
                        file.SaveAs(physicalPath);
                    }
                }
                else // File was not selected
                {
                    // If no image is selected make the image to default
                    if (String.IsNullOrEmpty(movie.Image))
                    {
                        movie.Image = uploads + "/default.jpg";
                    }
                }

                // Add and save movie inside database
                db.PublicMovies.Add(movie);
                db.SaveChanges();

                return RedirectToAction("Index", "PublicMovies");

            }
            return View(movie);
        }


        public ActionResult MovieInfo(int ? id)
        {
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MovieInfo([Bind()] MovieDetailViewModel movieDetailViewModel)
        {

            return View();
        }
    }
}