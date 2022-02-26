using ContosoUniversity.Shared.Interfaces;
using ContosoUniversity.Shared.ViewModels.Courses;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using static ContosoUniversity.Shared.CommonDefs;

namespace ContosoUniversity.Components
{
    public partial class Courses
    {
        [Parameter] 
        public CoursesViewModel CoursesVM { get; set; }

        protected CourseEditDto Course2Edit { get; set; }
        protected string Message { get; set; }
        protected UIMode UIMode { get; set; }
        protected CourseListItem SelectedCourse { get; set; }
        protected CourseItem SelectedCourseDetails { get; set; }

        [Inject] 
        protected ILogger<Courses> Logger { get; set; }

        [Inject]
        protected ISchoolViewDataRepositoryFactory SchoolViewDataRepositoryFactory { get; set; }

        protected async Task OnCreateCourse()
        {
            CourseEventArgs args = new CourseEventArgs
            {
                UIMode = UIMode.Create
            };
            await CourseAction(args);
        }

        public async Task CourseAction(CourseEventArgs args)
        {
            if (args != null)
            {
                Message = null;
                try
                {
                    ISchoolViewDataRepository dataHelper = SchoolViewDataRepositoryFactory.GetViewDataRepository();
                    if (args.CourseID != 0)
                    {
                        if (args.UIMode == UIMode.Details)
                        {
                            var details = await dataHelper.GetCourseDetailsNoTrackingAsync(args.CourseID);
                            if (details == null)
                            {
                                Message = "Course not found";
                            }
                            else
                            {
                                SelectedCourseDetails = details;
                                UIMode = args.UIMode;
                            }
                        }
                        else if (args.UIMode == UIMode.Edit)
                        {
                            Course2Edit = await dataHelper.GetCourse2EditAsync(args.CourseID);
                            if (Course2Edit == null)
                            {
                                Message = "Course not found";
                            }
                            else
                            {
                                UIMode = args.UIMode;
                            }
                        }
                        else if (args.UIMode == UIMode.Delete)
                        {
                            var details = await dataHelper.GetCourseDetailsNoTrackingAsync(args.CourseID);
                            if (details == null)
                            {
                                Message = "Course not found";
                            }
                            else
                            {
                                SelectedCourseDetails = details;
                                UIMode = args.UIMode;
                            }
                        }
                    }
                    else
                    {
                        if (args.UIMode == UIMode.List)
                        {
                            CoursesVM.ViewMode = 0; //Clear initial ViewMode from page load
                            CoursesVM.CourseList = await dataHelper.GetCourseListNoTrackingAsync();
                            UIMode = args.UIMode;
                        }
                        else if (args.UIMode == UIMode.Create)
                        {
                            UIMode = args.UIMode;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, "Courses-CourseAction id={0}, uiMode={1} - {2}: {3}",
                        args.CourseID, args.UIMode, ex.GetType().Name, ex.Message);
                    Message = $"Error setting up {args.UIMode} with CourseID = {args.CourseID} - contact Support";
                }
            }
        }

        protected async Task OnConfirmDelete()
        {
            try
            {
                if (SelectedCourseDetails == null)
                {
                    Message = $"Could not delete course - contact Support";
                }
                else
                {
                    ISchoolViewDataRepository dataHelper = SchoolViewDataRepositoryFactory.GetViewDataRepository();
                    CourseActionResult result = await dataHelper.DeleteCourseAsync(SelectedCourseDetails.CourseID);

                    if (result.ChangeCount < 1)
                    {
                        Logger.LogError("Courses-ConfirmDelete id={0}, action={1}, changeCount={2} | {3}",
                            SelectedCourseDetails.CourseID, result.Action, result.ChangeCount, result.ErrorMessage);
                        Message = $"Could not delete course {SelectedCourseDetails.CourseID} - contact Support";
                    }
                    else
                    {
                        SelectedCourseDetails = null;
                        CourseEventArgs args = new CourseEventArgs
                        {
                            UIMode = UIMode.List
                        };
                        await CourseAction(args);
                    }
                }
            }
            catch (Exception ex)
            {
                int? courseId = SelectedCourseDetails != null ? SelectedCourseDetails.CourseID : (int?)null;
                Logger.LogError(ex, "Courses-ConfirmDelete id={0}, uiMode={1} - {2}: {3}",
                    courseId, UIMode, ex.GetType().Name, ex.Message);
                Message = $"Error setting up {UIMode} with CourseID = {courseId} - contact Support";
            }
        }

        protected override async Task OnInitializedAsync()
        {
            ISchoolViewDataRepository dataHelper = SchoolViewDataRepositoryFactory.GetViewDataRepository();
            if (CoursesVM != null)
            {
                CourseItem details = null;

                if ((CoursesVM.ViewMode == (int)UIMode.Details) && (CoursesVM.CourseID.HasValue))
                {
                    details = await dataHelper.GetCourseDetailsNoTrackingAsync(CoursesVM.CourseID.Value);
                    if ((details == null) || (details.CourseID == 0))
                    {
                        CoursesVM.ViewMode = (int)UIMode.List;
                        Message = $"Course {CoursesVM.CourseID} not found";
                        details = null;
                        UIMode = UIMode.List;
                    }
                }
                if (details != null)
                {
                    SelectedCourseDetails = details;
                    UIMode = UIMode.Details;
                }
            }
            else
            {
                CoursesVM = new CoursesViewModel();
            }
            if ((CoursesVM.CourseList == null) || (CoursesVM.CourseList.Count() == 0))
            {
                CoursesVM.CourseList = await dataHelper.GetCourseListNoTrackingAsync();
            }
        }

        protected async Task OnReturnToList()
        {
            CourseEventArgs args = new CourseEventArgs
            {
                UIMode = UIMode.List
            };
            await CourseAction(args);
        }
    }
}
