using BuiMinhDuc_Lab456.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using BuiMinhDuc_Lab456.ViewModels;
using Microsoft.AspNet.Identity;

namespace BuiMinhDuc_Lab456.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext _dbContext;
        public HomeController()
        {
            _dbContext = new ApplicationDbContext();
        }

        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var upcommingCourses = _dbContext.Courses.Include(x => x.Lecturer).Include(c => c.Category).Where(c => c.DateTime > DateTime.Now).ToList();

            var upcommingAttendance = _dbContext.Attendances
                .Where(a => a.AttendeeId == userId).ToList();

            var upcommingFollowing = _dbContext.Followings
               .Where(a => a.FollowerId == userId).ToList();

            var viewModel = new CoursesViewModel
            {
                UpcommingCourses = upcommingCourses,
                ShowAction = User.Identity.IsAuthenticated,
                UpcommingAttendances = upcommingAttendance,
                UpcommingFollowings = upcommingFollowing
            };
            return View(viewModel);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}