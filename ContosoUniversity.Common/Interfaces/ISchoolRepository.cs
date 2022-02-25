using ContosoUniversity.Models;
using ContosoUniversity.Shared.ViewModels.Courses;
using ContosoUniversity.Shared.ViewModels.Departments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContosoUniversity.Common.Interfaces
{
    public interface ISchoolRepository : IDisposable
    {
        Task<List<CourseListItem>> GetCourseListItemsNoTrackingAsync();
        IQueryable<Course> GetCoursesQueryable();
        Task<List<DepartmentListItem>> GetDepartmentListItemsNoTrackingAsync();
        IQueryable<Department> GetDepartmentsQueryable();
        Task<int> SaveChangesAsync();
    }
}
