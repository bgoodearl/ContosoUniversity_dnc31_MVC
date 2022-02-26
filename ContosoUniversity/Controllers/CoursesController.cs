using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using ContosoUniversity.Common.Interfaces;
using ContosoUniversity.DAL.Interfaces;
using ContosoUniversity.Models;
using ContosoUniversity.Shared.Interfaces;
using ContosoUniversity.Shared.ViewModels.Departments;
using ContosoUniversity.ViewModels.Courses;
using ContosoUniversity.ViewModels.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CUSVMC = ContosoUniversity.Shared.ViewModels.Courses;

namespace ContosoUniversity.Controllers
{
    [Route("[Controller]/[Action]")]
    public class CoursesController : CUControllerBase
    {
        public CoursesController(IHttpContextAccessor httpContextAccessor,
            ILogger<CoursesController> logger)
            : base(httpContextAccessor)
        {
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        #region read-only variables
        protected ILogger<CoursesController> Logger { get; }
        #endregion read-only variables
        [Route("~/[Controller]/{mode?}/{id?}")]
        public async Task<IActionResult> Index(int? mode, int? id)
        {
            using (ISchoolRepository repo = GetSchoolRepository())
            {
                CUSVMC.CoursesViewModel model = new CUSVMC.CoursesViewModel
                {
                    CourseID = id,
                    ViewMode = mode.HasValue ? mode.Value : 0
                };

                model.CourseList = await repo.GetCourseListItemsNoTrackingAsync();
                return View(model);
            }
        }

        // GET: Courses/Details/5
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Details(int id)
        {
            ISchoolViewDataRepository svRepo = GetSchoolViewDataRepository();
            CUSVMC.CourseItem courseItem = await svRepo.GetCourseDetailsNoTrackingAsync(id);
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
                Course = new CUSVMC.CourseEditDto(),
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
        public async Task<IActionResult> Create([Bind("CourseID,Credits,DepartmentID,Title")] CUSVMC.CourseEditDto course)
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
                            CUSVMC.CourseActionResult actionResult = await repo.AddNewCourseAsync(course);
                            if (!string.IsNullOrWhiteSpace(actionResult.ErrorMessage))
                            {
                                Logger.LogError(null, "Courses-Create - {0}", actionResult.ErrorMessage);
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
                        if ((ex2Log.InnerException != null) && (ex2Log is System.Data.Entity.Core.UpdateException))
                        {
                            ex2Log = ex2Log.InnerException;
                        }
                    }
                    Logger.LogError(ex, "Courses-Create[1] {0}: {1}", ex2Log.GetType().Name, ex2Log.Message);
                    ModelState.AddModelError("", "Unable to save changes[1]. Try again, and if the problem persists, see your system administrator.");
                }
                catch (RetryLimitExceededException ex)
                {
                    Logger.LogError(ex, "Courses-Create[2] {0}: {1}", ex.GetType().Name, ex.Message);
                    ModelState.AddModelError("", "Unable to save changes[2]. Try again, and if the problem persists, see your system administrator.");
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, "Courses-Create[3] {0}: {1}", ex.GetType().Name, ex.Message);
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
        public async Task<IActionResult> Edit(int id, [Bind("CourseID,Credits,DepartmentID,Title")] CUSVMC.CourseEditDto course)
        {
            if ((id == 0) || (course == null) || (course.CourseID != id))
            {
                Logger.LogError(null, $"id={id}, course.CourseID={(course != null ? course.CourseID : (int?)null)}");
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
                            CUSVMC.CourseActionResult actionResult = await repo.SaveCourseChangesAsync(course);
                            if (!string.IsNullOrWhiteSpace(actionResult.ErrorMessage))
                            {
                                Logger.LogError(null, "Courses-Edit - {0}", actionResult.ErrorMessage);
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
                    Logger.LogError(ex, "Courses-Edit[1] {0}: {1}", ex2Log.GetType().Name, ex2Log.Message);
                    ModelState.AddModelError("", "Unable to save changes[1]. Try again, and if the problem persists, see your system administrator.");
                }
                catch (RetryLimitExceededException ex)
                {
                    Logger.LogError(ex, "Courses-Edit[2] {0}: {1}", ex.GetType().Name, ex.Message);
                    ModelState.AddModelError("", "Unable to save changes[2]. Try again, and if the problem persists, see your system administrator.");
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, "Courses-Edit[3] {0}: {1}", ex.GetType().Name, ex.Message);
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

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            using (ISchoolRepository repo = GetSchoolRepository())
            {
                CUSVMC.CourseItem course = new CUSVMC.CourseItem(await repo.GetCourseListItemNoTrackingAsync(id));
                if (course == null)
                {
                    return NotFound();
                }
                else
                {
                    return View(course);
                }
            }
        }

        [HttpPost]
        [Route("{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, [Bind("CourseID,Title")] CUSVMC.CourseItem course)
        {
            if ((id == 0) || (course == null) || (course.CourseID != id))
            {
                Logger.LogError(null, $"id={id}, course.CourseID={(course != null ? course.CourseID : (int?)null)}");
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                ISchoolViewDataRepository svRepo = GetSchoolViewDataRepository();
                CUSVMC.CourseActionResult actionResult = await svRepo.DeleteCourseAsync(course.CourseID);
                if (!string.IsNullOrWhiteSpace(actionResult.ErrorMessage))
                {
                    Logger.LogError(null, "Courses-Delete - {0}", actionResult.ErrorMessage);
                    ModelState.AddModelError("", "Unable to delete course. Try again, and if the problem persists, see your system administrator.");
                }
                else
                {
                    Logger.LogInformation($"Courses-Delete CourseID = {course.CourseID}");
                    return RedirectToAction("Index");
                }
            }

            using (ISchoolRepository repo = GetSchoolRepository())
            {
                int courseID = course.CourseID;
                course = new CUSVMC.CourseItem(await repo.GetCourseListItemNoTrackingAsync(courseID));
                if (course == null)
                {
                    return NotFound();
                }
                else
                {
                    return View(course);
                }
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
                Logger.LogError(ex, "Courses-SeedData {0}: {1}", ex.GetType().Name, ex.Message);
                ErrorViewModel model = new ErrorViewModel
                {
                };
                return View("Error", model);
            }
        }

    }
}
