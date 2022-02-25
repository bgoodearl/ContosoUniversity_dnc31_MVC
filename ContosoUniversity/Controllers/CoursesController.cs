using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContosoUniversity.Common.Interfaces;
using ContosoUniversity.DAL.Interfaces;
using ContosoUniversity.Models;
using ContosoUniversity.Shared.ViewModels.Courses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ContosoUniversity.Controllers
{
    [Route("[Controller]/[Action]")]
    public class CoursesController : CUControllerBase
    {
        public CoursesController(IHttpContextAccessor httpContextAccessor)
            : base(httpContextAccessor)
        {
        }

        [Route("~/[Controller]")]
        public async Task<IActionResult> Index()
        {
            using (ISchoolRepository repo = GetSchoolRepository())
            {
                List<CourseListItem> courses = await repo.GetCourseListItemsNoTrackingAsync();
                return View(courses);
            }
        }

        public async Task<IActionResult> SeedData()
        {
            try
            {
                using (ISchoolDbContext ctx = GetSchoolDbContext())
                {
                    if ((ctx.Students.Count() == 0) || (ctx.Instructors.Count() == 0)
                        || (ctx.Courses.Count() == 0) || (ctx.Enrollments.Count() == 0))
                    {
                        int saveChangeCount = await ctx.SeedInitialDataAsync();
                    }
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                //TODO: Logger.LogError(ex, "Courses-SeedData {0}: {1}", ex.GetType().Name, ex.Message);
                ErrorViewModel model = new ErrorViewModel
                {
                };
                return View("Error", model);
            }
        }

    }
}
