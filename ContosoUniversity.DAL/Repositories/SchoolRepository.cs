using ContosoUniversity.Common.Interfaces;
using ContosoUniversity.Models;
using ContosoUniversity.Shared.ViewModels.Courses;
using ContosoUniversity.Shared.ViewModels.Departments;
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

        public async Task<List<DepartmentListItem>> GetDepartmentListItemsNoTrackingAsync()
        {
            List<DepartmentListItem> departments = await SchoolDbContext.Departments
                .Include(d => d.Administrator)
                .AsNoTracking()
                .OrderBy(d => d.Name)
                .Select(d => new DepartmentListItem
                {
                    Administrator = d.Administrator != null ? d.Administrator.LastName + ", " + d.Administrator.FirstMidName : null,
                    Budget = d.Budget,
                    DepartmentID = d.DepartmentID,
                    InstructorID = d.InstructorID,
                    Name = d.Name,
                    StartDate = d.StartDate
                })
                .ToListAsync();
            return departments;
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
