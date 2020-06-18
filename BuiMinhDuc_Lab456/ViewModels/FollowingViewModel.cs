using BuiMinhDuc_Lab456.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BuiMinhDuc_Lab456.ViewModels
{
    public class FollowingViewModel
    {
        public IEnumerable<ApplicationUser> UpcommingFollowing { get; set; }
        public bool ShowAction { get; set; }
    }
}