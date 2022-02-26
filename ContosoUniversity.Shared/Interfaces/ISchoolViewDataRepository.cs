
using ContosoUniversity.Shared.ViewModels.Courses;
using System.Threading.Tasks;

namespace ContosoUniversity.Shared.Interfaces
{
    public interface ISchoolViewDataRepository
    {
        Task<CourseActionResult> DeleteCourseAsync(int courseID);
        Task<CourseItem> GetCourseDetailsNoTrackingAsync(int courseID);
    }
}
