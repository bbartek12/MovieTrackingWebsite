using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MovieTrackingWebsite.Models
{
    public class PublicMovie : Movie
    {
        public int PublicMovieId { get; set; }
        public string Description { get; set; }
        public int Year { get; set; }
    }
}