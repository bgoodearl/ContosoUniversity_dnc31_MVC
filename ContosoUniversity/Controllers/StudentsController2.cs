using ContosoUniversity.DAL.Interfaces;
using ContosoUniversity.Shared.Queries;
using ContosoUniversity.Shared.ViewModels.Students;
using System.Linq;

namespace ContosoUniversity.Controllers
{
    public partial class StudentsController //StudentsController2.cs
    {
        private IQueryable<StudentListItem> GetProjectedStudents(ISchoolDbContext context, IStudentQuery studentQuery)
        {
            IQueryable<StudentListItem> studentsQueryable = null;

            if (string.IsNullOrWhiteSpace(studentQuery.NameSearch))
            {
                studentsQueryable = context.Students
                    .AsNoTracking()
                    .Select(s => new StudentListItem
                    {
                        EnrollmentDate = s.EnrollmentDate,
                        FirstMidName = s.FirstMidName,
                        ID = s.ID,
                        LastName = s.LastName
                    });
            }
            else
            {
                studentsQueryable = context.Students
                    .AsNoTracking()
                    .Where(s => s.LastName.Contains(studentQuery.NameSearch)
                                           || s.FirstMidName.Contains(studentQuery.NameSearch))
                    .Select(s => new StudentListItem
                    {
                        EnrollmentDate = s.EnrollmentDate,
                        FirstMidName = s.FirstMidName,
                        ID = s.ID,
                        LastName = s.LastName
                    });
            }
            switch (studentQuery.SortOrder)
            {
                case "name_desc":
                    studentsQueryable = studentsQueryable.OrderByDescending(s => s.LastName).ThenBy(s => s.FirstMidName);
                    break;
                case "Date":
                    studentsQueryable = studentsQueryable.OrderBy(s => s.EnrollmentDate);
                    break;
                case "date_desc":
                    studentsQueryable = studentsQueryable.OrderByDescending(s => s.EnrollmentDate);
                    break;
                default:
                    studentsQueryable = studentsQueryable.OrderBy(s => s.LastName).ThenBy(s => s.FirstMidName);
                    break;
            }

            return studentsQueryable;
        }
    }
}
