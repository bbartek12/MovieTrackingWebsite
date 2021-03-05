using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MovieTrackingWebsite.Models
{
    public class Review
    {
        public int ReviewId { get; set; }
        public int PublicMovieId { get; set; }
        public int ReviewScore { get; set; }
        public string Comment{ get; set; }   
        public ApplicationUser User { get; set; }
    }
}