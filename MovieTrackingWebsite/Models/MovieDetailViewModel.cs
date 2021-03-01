using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MovieTrackingWebsite.Models
{
    public class MovieDetailViewModel
    {
        public virtual PublicMovie Movie { get; set; }
        public int UserMovieId { get; set; } // unnecessary?
        public Status Status { get; set; }
        public virtual List<Review> ReviewsList { get; set; }

        public MovieDetailViewModel()
        {
            ReviewsList = new List<Review>();
        }
    }
}