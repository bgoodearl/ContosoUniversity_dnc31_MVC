using ContosoUniversity.Common.Interfaces;
using ContosoUniversity.Models;
using ContosoUniversity.Shared.ViewModels.Courses;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace ContosoUniversity.DAL.Repositories
{
    public class SchoolRepository : ISchoolRepository
    {
        private SchoolDbContext SchoolDbContext { get; }

        public SchoolRepository(SchoolDbContext schoolContext)
        {
            SchoolDbContext = schoolContext ?? throw new ArgumentNullException(nameof(schoolContext));
        }

        public async Task<List<CourseListItem>> GetCourseListItemsNoTrackingAsync()
        {
            List<CourseListItem> courses = await SchoolDbContext.Courses
                .AsNoTracking()
                .OrderBy(c => c.CourseID)
                .Select(c => new CourseListItem
                {
                    CourseID = c.CourseID,
                    Credits = c.Credits,
                    Department = c.Department.Name,
                    Title = c.Title
                })
                .ToListAsync();

            return courses;
        }

        public IQueryable<Course> GetCoursesQueryable()
        {
            return SchoolDbContext.Courses;
        }

        public IQueryable<Department> GetDepartmentsQueryable()
        {
            return SchoolDbContext.Departments;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await SchoolDbContext.SaveChangesAsync();
        }

        #region IDisposable

        public void Dispose()
        {
            SchoolDbContext.Dispose();
        }

        #endregion IDisposable

    }
}
