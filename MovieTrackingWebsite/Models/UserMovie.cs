using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MovieTrackingWebsite.Models
{
    public enum Status
    {
        Plan_To_Watch,
        Watching,
        Finished,
        Dropped
    }

    public class UserMovie : Movie
    {
        public int UserMovieId { get; set; }
        public int PublicMovieId { get; set; }
        public Status Status { get; set; }
    }
}