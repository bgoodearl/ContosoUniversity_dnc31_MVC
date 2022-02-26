using ContosoUniversity.Models;
using ContosoUniversity.Shared.ViewModels;
using ContosoUniversity.Shared.ViewModels.Courses;
using ContosoUniversity.Shared.ViewModels.Departments;
using ContosoUniversity.Shared.ViewModels.Instructors;
using ContosoUniversity.Shared.ViewModels.Students;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CM = ContosoUniversity.Models;

namespace ContosoUniversity.Common.Interfaces
{
    public interface ISchoolRepository : IDisposable
    {
        Task<int> AddNewCourseAsync(CourseEditDto course);
        Task<CourseEditDto> GetCourseEditDtoNoTrackingAsync(int courseID);
        Task<List<IdItem>> GetCourseInstructorsNoTrackingAsync(int courseID);
        Task<CourseListItem> GetCourseListItemNoTrackingAsync(int courseID);
        Task<List<CourseListItem>> GetCourseListItemsNoTrackingAsync();
        IQueryable<Course> GetCoursesQueryable();
        Task<List<DepartmentListItem>> GetDepartmentListItemsNoTrackingAsync();
        IQueryable<Department> GetDepartmentsQueryable();
        Task<List<InstructorListItem>> GetInstructorListItemsNoTrackingAsync();
        Task<List<StudentListItem>> GetStudentListItemsNoTrackingAsync();
        CM.Course RemoveCourse(CM.Course course);
        Task<CourseActionResult> SaveCourseChangesAsync(CourseEditDto course);
        Task<int> SaveChangesAsync();
    }
}
