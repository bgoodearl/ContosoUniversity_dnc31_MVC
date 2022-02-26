
using ContosoUniversity.Shared.ViewModels;
using ContosoUniversity.Shared.ViewModels.Courses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ContosoUniversity.Shared.Interfaces
{
    public interface ISchoolViewDataRepository
    {
        Task<CourseActionResult> DeleteCourseAsync(int courseID);
        Task<CourseEditDto> GetCourse2EditAsync(int courseID);
        Task<CourseItem> GetCourseDetailsNoTrackingAsync(int courseID);
        Task<IEnumerable<CourseListItem>> GetCourseListNoTrackingAsync();
        Task<List<IdItem>> GetDepartmentsListAsync();
        Task<CourseActionResult> SaveCourseEditChangesAsync(int courseID, CourseEditDto model);
        Task<CourseActionResult> SaveNewCourseAsync(CourseEditDto model);
    }
}
