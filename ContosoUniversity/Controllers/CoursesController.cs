using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using ContosoUniversity.Common.Interfaces;
using ContosoUniversity.DAL.Interfaces;
using ContosoUniversity.Models;
using ContosoUniversity.Shared.Interfaces;
using ContosoUniversity.Shared.ViewModels.Courses;
using ContosoUniversity.Shared.ViewModels.Departments;
using ContosoUniversity.ViewModels.Courses;
using ContosoUniversity.ViewModels.Shared;
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

        // GET: Courses/Details/5
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Details(int id)
        {
            ISchoolViewDataRepository svRepo = GetSchoolViewDataRepository();
            CourseItem courseItem = await svRepo.GetCourseDetailsNoTrackingAsync(id);
            if (courseItem == null)
            {
                return NotFound();
            }
            return View(courseItem);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            CourseEditViewModel model = new CourseEditViewModel
            {
                Course = new CourseEditDto(),
                NewCourse = true
            };
            using (ISchoolRepository repo = GetSchoolRepository())
            {
                await PopulateDepartmentsDropDownList(repo, model);
            }
            return View(model);
        }


        // POST: Courses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CourseID,Credits,DepartmentID,Title")] CourseEditDto course)
        {
            using (ISchoolRepository repo = GetSchoolRepository())
            {

                try
                {
                    if (ModelState.IsValid)
                    {
                        if (string.IsNullOrWhiteSpace(course.Title))
                        {
                            ModelState.AddModelError("Course.Title", "Title is required");
                        }
                        else
                        {
#if DEBUG
                            int courseID =
#endif
                            await repo.AddNewCourseAsync(course);
                            return RedirectToAction(nameof(Index));
                        }
                    }
                }
                catch (DbUpdateException ex)
                {
                    Exception ex2Log = ex;
                    string message = ex.Message;
                    if (ex.InnerException != null)
                    {
                        ex2Log = ex.InnerException;
                    }
                    //TODO: Logger.LogError(ex, "Courses-Create[1] {0}: {1}", ex2Log.GetType().Name, ex2Log.Message);
                    ModelState.AddModelError("", "Unable to save changes[1]. Try again, and if the problem persists, see your system administrator.");
                }
                catch (RetryLimitExceededException ex)
                {
                    //TODO: Logger.LogError(ex, "Courses-Create[2] {0}: {1}", ex.GetType().Name, ex.Message);
                    ModelState.AddModelError("", "Unable to save changes[2]. Try again, and if the problem persists, see your system administrator.");
                }
                catch (Exception ex)
                {
                    //TODO: Logger.LogError(ex, "Courses-Create[3] {0}: {1}", ex.GetType().Name, ex.Message);
                    ModelState.AddModelError("", "Unable to save changes[3]. Try again, and if the problem persists, see your system administrator.");
                }
                CourseEditViewModel model = new CourseEditViewModel
                {
                    Course = course,
                    NewCourse = true
                };
                await PopulateDepartmentsDropDownList(repo, model);
                return View(model);
            }
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            using (ISchoolRepository repo = GetSchoolRepository())
            {
                CourseEditViewModel model = new CourseEditViewModel
                {
                    Course = await repo.GetCourseEditDtoNoTrackingAsync(id)
                };
                if (model.Course == null)
                {
                    return NotFound();
                }
                await PopulateDepartmentsDropDownList(repo, model);
                return View(model);
            }
        }

        [HttpPost]
        [Route("{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CourseID,Credits,DepartmentID,Title")] CourseEditDto course)
        {
            if ((id == 0) || (course == null) || (course.CourseID != id))
            {
                //TODO: Log this
                return BadRequest();
            }

            using (ISchoolRepository repo = GetSchoolRepository())
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        if (string.IsNullOrWhiteSpace(course.Title))
                        {
                            ModelState.AddModelError("Course.Title", "Title is required");
                        }
                        else
                        {
                            CourseActionResult actionResult = await repo.SaveCourseChangesAsync(course);
                            if (!string.IsNullOrWhiteSpace(actionResult.ErrorMessage))
                            {
                                //TODO: Log This
                                ModelState.AddModelError("", "Unable to save changes[1]. Try again, and if the problem persists, see your system administrator.");
                            }
                            else
                            {
                                return RedirectToAction(nameof(Index));
                            }
                        }
                    }
                }
                catch (DbUpdateException ex)
                {
                    Exception ex2Log = ex;
                    string message = ex.Message;
                    if (ex.InnerException != null)
                    {
                        ex2Log = ex.InnerException;
                    }
                    //TODO: Logger.LogError(ex, "Courses-Edit[1] {0}: {1}", ex2Log.GetType().Name, ex2Log.Message);
                    ModelState.AddModelError("", "Unable to save changes[1]. Try again, and if the problem persists, see your system administrator.");
                }
                catch (RetryLimitExceededException ex)
                {
                    //TODO: Logger.LogError(ex, "Courses-Edit[2] {0}: {1}", ex.GetType().Name, ex.Message);
                    ModelState.AddModelError("", "Unable to save changes[2]. Try again, and if the problem persists, see your system administrator.");
                }
                catch (Exception ex)
                {
                    //TODO: Logger.LogError(ex, "Courses-Edit[3] {0}: {1}", ex.GetType().Name, ex.Message);
                    ModelState.AddModelError("", "Unable to save changes[3]. Try again, and if the problem persists, see your system administrator.");
                }
                CourseEditViewModel model = new CourseEditViewModel
                {
                    Course = course
                };
                await PopulateDepartmentsDropDownList(repo, model);
                return View(model);
            }
        }

        private async Task PopulateDepartmentsDropDownList(ISchoolRepository repo, CourseSharedViewModel model)
        {
            List<DepartmentListItem> idItems = await repo.GetDepartmentListItemsNoTrackingAsync();
            model.Departments = idItems.Select(x => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
            {
                Text = x.Name,
                Value = x.DepartmentID.ToString()
            }).ToList();
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
