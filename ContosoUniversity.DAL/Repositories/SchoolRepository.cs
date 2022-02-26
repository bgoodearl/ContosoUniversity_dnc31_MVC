using Ardalis.GuardClauses;
using ContosoUniversity.Common.Interfaces;
using ContosoUniversity.Models;
using ContosoUniversity.Shared.ViewModels;
using ContosoUniversity.Shared.ViewModels.Courses;
using ContosoUniversity.Shared.ViewModels.Departments;
using ContosoUniversity.Shared.ViewModels.Instructors;
using ContosoUniversity.Shared.ViewModels.Students;
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

        public async Task<CourseActionResult> AddNewCourseAsync(CourseEditDto course)
        {
            Guard.Against.Null(course, nameof(course));

            CourseActionResult result = new CourseActionResult
            {
                Action = "AddNewCourseAsync",
                CourseID = course.CourseID
            };

            Course persistentCourse = new Course
            {
                CourseID = course.CourseID,
                Credits = course.Credits,
                DepartmentID = course.DepartmentID,
                Title = course.Title
            };
            SchoolDbContext.Courses.Add(persistentCourse);
            result.ChangeCount = await SaveChangesAsync();
            result.CourseIDNew = persistentCourse.CourseID;
            return result;
        }

        public async Task<CourseEditDto> GetCourseEditDtoNoTrackingAsync(int courseID)
        {
            CourseEditDto course = await SchoolDbContext.Courses
                .AsNoTracking()
                .Where(c => c.CourseID == courseID)
                .Select(c => new CourseEditDto
                {
                    CourseID = c.CourseID,
                    Credits = c.Credits,
                    DepartmentID = c.DepartmentID,
                    Title = c.Title
                })
                .SingleOrDefaultAsync();

            return course;
        }

        public async Task<List<IdItem>> GetCourseInstructorsNoTrackingAsync(int courseID)
        {
            Course course = await SchoolDbContext.Courses
                .Include(c => c.Instructors)
                .AsNoTracking()
                .Where(c => c.CourseID == courseID)
                .SingleOrDefaultAsync();
            if (course != null)
            {
                List<IdItem> instructorList = course.Instructors
                    .OrderBy(i => i.FullName)
                    .Select(i => new IdItem
                    {
                        Id = i.ID,
                        Name = i.FullName
                    })
                    .ToList();
                return instructorList;
            }
            return new List<IdItem>();
        }

        public async Task<CourseListItem> GetCourseListItemNoTrackingAsync(int courseID)
        {
            CourseListItem course = await SchoolDbContext.Courses
                .AsNoTracking()
                .Where(c => c.CourseID == courseID)
                .Select(c => new CourseListItem
                {
                    CourseID = c.CourseID,
                    Credits = c.Credits,
                    Department = c.Department.Name,
                    Title = c.Title
                })
                .SingleOrDefaultAsync();
            return course;
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

        public async Task<List<InstructorListItem>> GetInstructorListItemsNoTrackingAsync()
        {
            List<InstructorListItem> instructors = await SchoolDbContext.Instructors
                .Include(i => i.OfficeAssignment)
                .OrderBy(i => i.LastName)
                .ThenBy(i => i.FirstMidName)
                .Select(i => new InstructorListItem
                {
                    ID = i.ID,
                    FirstMidName = i.FirstMidName,
                    HireDate = i.HireDate,
                    LastName = i.LastName,
                    OfficeAssignment = i.OfficeAssignment != null ? i.OfficeAssignment.Location : null,
                })
                .ToListAsync();

            return instructors;
        }

        public async Task<List<StudentListItem>> GetStudentListItemsNoTrackingAsync()
        {
            List<StudentListItem> students = await SchoolDbContext.Students
                .OrderBy(s => s.LastName)
                .ThenBy(s => s.FirstMidName)
                .Select(s => new StudentListItem
                {
                    ID = s.ID,
                    EnrollmentDate = s.EnrollmentDate,
                    FirstMidName = s.FirstMidName,
                    LastName = s.LastName
                })
                .ToListAsync();

            return students;
        }

        public Course RemoveCourse(Course course)
        {
            return SchoolDbContext.Courses.Remove(course);
        }

        public async Task<CourseActionResult> SaveCourseChangesAsync(CourseEditDto course)
        {
            Guard.Against.Null(course, nameof(course));
            Guard.Against.Zero(course.CourseID, nameof(course.CourseID));

            CourseActionResult result = new CourseActionResult
            {
                Action = "SaveCourseChangesAsync",
                CourseID = course.CourseID
            };

            Course dbCourse = await SchoolDbContext.Courses
                .Where(c => c.CourseID == course.CourseID)
                .SingleOrDefaultAsync();
            if (dbCourse == null)
            {
                result.ErrorMessage = "Course not found";
            }
            else
            {
                dbCourse.Credits = course.Credits;
                dbCourse.DepartmentID = course.DepartmentID;
                dbCourse.Title = course.Title;
                result.ChangeCount = await SaveChangesAsync();
            }
            return result;
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
