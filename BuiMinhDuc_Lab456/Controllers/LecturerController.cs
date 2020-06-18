using BuiMinhDuc_Lab456.DTOs;
using BuiMinhDuc_Lab456.Models;
using BuiMinhDuc_Lab456.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BuiMinhDuc_Lab456.Controllers
{
    public class LecturerController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        public LecturerController()
        {
            _dbContext = new ApplicationDbContext();
        }

        // GET: Lecturer        

        [Authorize]
        public ActionResult Following()
        {
            var userId = User.Identity.GetUserId();

            var follows = _dbContext.Followings
                .Where(a => a.FollowerId == userId)
                .Select(a => a.Followee)
                .Include(l => l.Followees).ToList();

            var viewModel = new FollowingViewModel
            {
                UpcommingFollowing = follows,
                ShowAction = User.Identity.IsAuthenticated
            };
            return View(viewModel);
        }

        public ActionResult Unfollow(string FolloweeId)
        {
            if (!ModelState.IsValid)
            {                
                return Redirect(Request.UrlReferrer.ToString());
            }
            var userId = User.Identity.GetUserId();
            var following = new Following
            {
                FollowerId = userId,
                FolloweeId = FolloweeId
            };
            _dbContext.Followings.Attach(following);
            _dbContext.Followings.Remove(following);
            _dbContext.SaveChanges();
            return Redirect(Request.UrlReferrer.ToString());
        }
       


    }
}