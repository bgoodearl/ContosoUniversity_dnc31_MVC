using Ardalis.GuardClauses;
using ContosoUniversity.Common.Interfaces;
using ContosoUniversity.Shared.Interfaces;
using ContosoUniversity.Shared.ViewModels.Courses;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using CM = ContosoUniversity.Models;

namespace ContosoUniversity.DAL.Repositories
{
    public class SchoolViewDataRepository : ISchoolViewDataRepository
    {
        public SchoolViewDataRepository(ISchoolRepositoryFactory schoolRepositoryFactory)
        {
            Guard.Against.Null(schoolRepositoryFactory, nameof(schoolRepositoryFactory));
            SchoolRepositoryFactory = schoolRepositoryFactory;
        }

        protected ISchoolRepositoryFactory SchoolRepositoryFactory { get; }

        #region ISchoolViewDataRepository

        public async Task<CourseActionResult> DeleteCourseAsync(int courseID)
        {
            using (ISchoolRepository repo = SchoolRepositoryFactory.GetSchoolRepository())
            {
                CourseActionResult result = new CourseActionResult
                {
                    Action = "DeleteCourse",
                    CourseID = courseID
                };
                CM.Course course = await repo.GetCoursesQueryable()
                    .Where(x => x.CourseID == courseID)
                    .SingleOrDefaultAsync();
                if (course == null)
                {
                    result.ErrorMessage = $"Course {courseID} not found";
                }
                else
                {
#if DEBUG
                    CM.Course courseRemoved =
#endif
                    repo.RemoveCourse(course);
                    result.ChangeCount = await repo.SaveChangesAsync();
                }

                return result;
            }
        }

        public async Task<CourseItem> GetCourseDetailsNoTrackingAsync(int courseID)
        {
            using (ISchoolRepository repo = SchoolRepositoryFactory.GetSchoolRepository())
            {
                CourseItem course = new CourseItem(await repo.GetCourseListItemNoTrackingAsync(courseID));
                if (course != null)
                {
                    course.SetInstructors(await repo.GetCourseInstructorsNoTrackingAsync(courseID));
                }
                return course;
            }
        }

        #endregion ISchoolViewDataRepository

    }
}
