using Ardalis.GuardClauses;
using ContosoUniversity.Common.Interfaces;
using ContosoUniversity.Shared.Interfaces;
using ContosoUniversity.Shared.ViewModels.Courses;
using System.Threading.Tasks;

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
