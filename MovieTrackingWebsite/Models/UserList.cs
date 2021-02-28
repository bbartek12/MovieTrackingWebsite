using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MovieTrackingWebsite.Models
{
    public class UserList
    {
        public int UserListId { get; set; }
        public ApplicationUser User { get; set; }

        public virtual List<UserMovie> WatchList { get; set; }

        public UserList()
        {
            this.WatchList = new List<UserMovie>();
        }
    }
}