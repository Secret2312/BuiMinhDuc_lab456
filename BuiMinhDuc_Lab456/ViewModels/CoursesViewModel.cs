using BuiMinhDuc_Lab456.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BuiMinhDuc_Lab456.ViewModels
{
    public class CoursesViewModel
    {
        public IEnumerable<Course> UpcommingCourses { get; set; }
        public bool ShowAction { get; set; }
        public IEnumerable<Following> UpcommingFollowings { get; set; }
        public IEnumerable<Attendance> UpcommingAttendances { get; set; }
    }
}