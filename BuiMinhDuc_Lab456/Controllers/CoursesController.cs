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
    public class CoursesController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        public CoursesController()
        {
            _dbContext = new ApplicationDbContext();
        }
        // GET: Courses
        [Authorize]
        public ActionResult Create()
        {
            var viewModel = new CourseViewModel
            {
                Categories = _dbContext.Categories.ToList(),
                Heading = "Add Course"
            };
            return View(viewModel);
        }
        
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CourseViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Categories = _dbContext.Categories.ToList();
                return View("Create", viewModel);
            }
            var course = new Course
            {
                LecturerId = User.Identity.GetUserId(),
                DateTime = viewModel.GetDateTime(),
                CategoryID = viewModel.Category,
                Place = viewModel.Place
            };
            _dbContext.Courses.Add(course);
            _dbContext.SaveChanges();

            return RedirectToAction("Mine", "Courses");
        }
        [Authorize]
        public ActionResult Edit(int id)
        {
            var userId = User.Identity.GetUserId();
            var course = _dbContext.Courses.Single(c => c.Id == id && c.LecturerId == userId);

            var viewModel = new CourseViewModel
            {
                Categories = _dbContext.Categories.ToList(),
                Date = course.DateTime.ToString("dd/M/yyy"),
                Time = course.DateTime.ToString("HH:mm"),
                Category = course.CategoryID,
                Place = course.Place,
                Heading = "Edit Course",
                Id = course.Id
            };
            return View("Create", viewModel);
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(CourseViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Categories = _dbContext.Categories.ToList();
                return View("Create", viewModel);
            }
            var userId = User.Identity.GetUserId();
            var course = _dbContext.Courses.Single(c => c.Id == viewModel.Id && c.LecturerId == userId);
            course.Place = viewModel.Place;
            course.DateTime = viewModel.GetDateTime();
            course.CategoryID = viewModel.Category;
            UpdateModel(course);
            _dbContext.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
        [Authorize]
        public ActionResult Attending()
        {
            var userId = User.Identity.GetUserId();
            var courses = _dbContext.Attendances
                .Where(a => a.AttendeeId == userId)
                .Select(a => a.Course)
                .Include(l => l.Lecturer)
                .Include(l => l.Category).ToList();

            var upcommingFollowing = _dbContext.Followings
               .Where(a => a.FollowerId == userId).ToList();

            var upcommingAttendance = _dbContext.Attendances
                .Where(a => a.AttendeeId == userId).ToList();

            var viewModel = new CoursesViewModel
            {
                UpcommingCourses = courses,
                ShowAction = User.Identity.IsAuthenticated,
                UpcommingFollowings = upcommingFollowing,
                UpcommingAttendances = upcommingAttendance
            };
            return View(viewModel);
        }        

        [Authorize]
        public ActionResult Mine()
        {
            var userId = User.Identity.GetUserId();
            var courses = _dbContext.Courses
                .Where(a => a.LecturerId == userId && a.DateTime > DateTime.Now)
                .Include(l=>l.Lecturer)
                .Include(l=>l.Category).Where(a=>a.IsCanceled == false).ToList();
            
            var viewModel = new CoursesViewModel
            {
                UpcommingCourses = courses,
                ShowAction = User.Identity.IsAuthenticated,
            };
            
            return View(viewModel);
        }

        [Authorize]
        public ActionResult Unjoin(int courseid)
        {
            if (!ModelState.IsValid)
            {
                return Redirect(Request.UrlReferrer.ToString());
            }
            var userId = User.Identity.GetUserId();
            var attandance = new Attendance
            {
                CourseId = courseid,
                AttendeeId = userId
            };
            _dbContext.Attendances.Attach(attandance);
            _dbContext.Attendances.Remove(attandance);
            _dbContext.SaveChanges();
            return Redirect(Request.UrlReferrer.ToString());
        }

        

    }
}