
using ContosoUniversity.Shared.ViewModels.Courses;
using System.Threading.Tasks;

namespace ContosoUniversity.Shared.Interfaces
{
    public interface ISchoolViewDataRepository
    {
        Task<CourseItem> GetCourseDetailsNoTrackingAsync(int courseID);
    }
}
