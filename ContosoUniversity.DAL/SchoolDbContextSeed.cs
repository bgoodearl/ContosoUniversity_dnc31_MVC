using System;
using System.Collections.Generic;
using System.Linq;
using ContosoUniversity.Models;
using System.Data.Entity.Migrations;
using System.Threading.Tasks;

namespace ContosoUniversity.DAL
{
    internal static class SchoolDbContextSeed
    {
        internal static async Task<int> SeedInitialData(SchoolDbContext context)
        {
            int saveChangeCount = 0;

            if (context.Students.Count() == 0)
            {
                var students = new List<Student>
                {
                    new Student { FirstMidName = "Carson",   LastName = "Alexander",
                        EnrollmentDate = DateTime.Parse("2010-09-01") },
                    new Student { FirstMidName = "Meredith", LastName = "Alonso",
                        EnrollmentDate = DateTime.Parse("2020-09-01") },
                    new Student { FirstMidName = "Arturo",   LastName = "Anand",
                        EnrollmentDate = DateTime.Parse("2021-09-01") },
                    new Student { FirstMidName = "Gytis",    LastName = "Barzdukas",
                        EnrollmentDate = DateTime.Parse("2020-09-01") },
                    new Student { FirstMidName = "Yan",      LastName = "Li",
                        EnrollmentDate = DateTime.Parse("2020-09-01") },
                    new Student { FirstMidName = "Peggy",    LastName = "Justice",
                        EnrollmentDate = DateTime.Parse("2019-09-01") },
                    new Student { FirstMidName = "Laura",    LastName = "Norman",
                        EnrollmentDate = DateTime.Parse("2021-09-01") },
                    new Student { FirstMidName = "Nino",     LastName = "Olivetto",
                        EnrollmentDate = DateTime.Parse("2016-08-11") }
                };
                students.ForEach(s => context.Students.AddOrUpdate(p => p.LastName, s));
                saveChangeCount += await context.SaveChangesAsync();
            }
            bool createdInstructors = false;
            if (context.Instructors.Count() == 0)
            {
                var instructors = new List<Instructor>
                {
                    new Instructor { FirstMidName = "Kim",     LastName = "Abercrombie",
                        HireDate = DateTime.Parse("1995-03-11") },
                    new Instructor { FirstMidName = "Fadi",    LastName = "Fakhouri",
                        HireDate = DateTime.Parse("2002-07-06") },
                    new Instructor { FirstMidName = "Roger",   LastName = "Harui",
                        HireDate = DateTime.Parse("1998-07-01") },
                    new Instructor { FirstMidName = "Candace", LastName = "Kapoor",
                        HireDate = DateTime.Parse("2001-01-15") },
                    new Instructor { FirstMidName = "Roger",   LastName = "Zheng",
                        HireDate = DateTime.Parse("2004-02-12") }
                };
                instructors.ForEach(s => context.Instructors.AddOrUpdate(p => p.LastName, s));
                saveChangeCount += await context.SaveChangesAsync();
                createdInstructors = true;

                var departments = new List<Department>
                {
                    new Department { Name = "English",     Budget = 350000,
                        StartDate = DateTime.Parse("2007-09-01"),
                        InstructorID  = instructors.Single( i => i.LastName == "Abercrombie").ID },
                    new Department { Name = "Mathematics", Budget = 100000,
                        StartDate = DateTime.Parse("2007-09-01"),
                        InstructorID  = instructors.Single( i => i.LastName == "Fakhouri").ID },
                    new Department { Name = "Engineering", Budget = 350000,
                        StartDate = DateTime.Parse("2007-09-01"),
                        InstructorID  = instructors.Single( i => i.LastName == "Harui").ID },
                    new Department { Name = "Economics",   Budget = 100000,
                        StartDate = DateTime.Parse("2007-09-01"),
                        InstructorID  = instructors.Single( i => i.LastName == "Kapoor").ID }
                };
                departments.ForEach(s => context.Departments.AddOrUpdate(p => p.Name, s));
                saveChangeCount += await context.SaveChangesAsync();

                var officeAssignments = new List<OfficeAssignment>
                {
                    new OfficeAssignment {
                        InstructorID = instructors.Single( i => i.LastName == "Fakhouri").ID,
                        Location = "Smith 17" },
                    new OfficeAssignment {
                        InstructorID = instructors.Single( i => i.LastName == "Harui").ID,
                        Location = "Gowan 27" },
                    new OfficeAssignment {
                        InstructorID = instructors.Single( i => i.LastName == "Kapoor").ID,
                        Location = "Thompson 304" },
                };
                officeAssignments.ForEach(s => context.OfficeAssignments.AddOrUpdate(p => p.InstructorID, s));
                saveChangeCount += await context.SaveChangesAsync();
            }
            bool createdCourses = false;
            if (context.Courses.Count() == 0)
            {
                var courses = new List<Course>
                {
                    new Course {CourseID = 1050, Title = "Chemistry",      Credits = 3,
                      DepartmentID = context.Departments.Single( s => s.Name == "Engineering").DepartmentID,
                      Instructors = new List<Instructor>()
                    },
                    new Course {CourseID = 4022, Title = "Microeconomics", Credits = 3,
                      DepartmentID = context.Departments.Single( s => s.Name == "Economics").DepartmentID,
                      Instructors = new List<Instructor>()
                    },
                    new Course {CourseID = 4041, Title = "Macroeconomics", Credits = 3,
                      DepartmentID = context.Departments.Single( s => s.Name == "Economics").DepartmentID,
                      Instructors = new List<Instructor>()
                    },
                    new Course {CourseID = 1045, Title = "Calculus",       Credits = 4,
                      DepartmentID = context.Departments.Single( s => s.Name == "Mathematics").DepartmentID,
                      Instructors = new List<Instructor>()
                    },
                    new Course {CourseID = 3141, Title = "Trigonometry",   Credits = 4,
                      DepartmentID = context.Departments.Single( s => s.Name == "Mathematics").DepartmentID,
                      Instructors = new List<Instructor>()
                    },
                    new Course {CourseID = 2021, Title = "Composition",    Credits = 3,
                      DepartmentID = context.Departments.Single( s => s.Name == "English").DepartmentID,
                      Instructors = new List<Instructor>()
                    },
                    new Course {CourseID = 2042, Title = "Literature",     Credits = 4,
                      DepartmentID = context.Departments.Single( s => s.Name == "English").DepartmentID,
                      Instructors = new List<Instructor>()
                    },
                };
                courses.ForEach(s => context.Courses.AddOrUpdate(p => p.CourseID, s));
                saveChangeCount += await context.SaveChangesAsync();
                createdCourses = true;

            }
            if (createdCourses && createdInstructors)
            {
                AddOrUpdateInstructor(context, "Chemistry", "Kapoor");
                AddOrUpdateInstructor(context, "Chemistry", "Harui");
                AddOrUpdateInstructor(context, "Microeconomics", "Zheng");
                AddOrUpdateInstructor(context, "Macroeconomics", "Zheng");

                AddOrUpdateInstructor(context, "Calculus", "Fakhouri");
                AddOrUpdateInstructor(context, "Trigonometry", "Harui");
                AddOrUpdateInstructor(context, "Composition", "Abercrombie");
                AddOrUpdateInstructor(context, "Literature", "Abercrombie");

                saveChangeCount += await context.SaveChangesAsync();
            }

            if (context.Enrollments.Count() == 0)
            {

                var enrollments = new List<Enrollment>
                {
                    new Enrollment {
                        StudentID = context.Students.Single(s => s.LastName == "Alexander").ID,
                        CourseID = context.Courses.Single(c => c.Title == "Chemistry" ).CourseID,
                        Grade = Grade.A
                    },
                     new Enrollment {
                        StudentID = context.Students.Single(s => s.LastName == "Alexander").ID,
                        CourseID = context.Courses.Single(c => c.Title == "Microeconomics" ).CourseID,
                        Grade = Grade.C
                     },
                     new Enrollment {
                        StudentID = context.Students.Single(s => s.LastName == "Alexander").ID,
                        CourseID = context.Courses.Single(c => c.Title == "Macroeconomics" ).CourseID,
                        Grade = Grade.B
                     },
                     new Enrollment {
                         StudentID = context.Students.Single(s => s.LastName == "Alonso").ID,
                        CourseID = context.Courses.Single(c => c.Title == "Calculus" ).CourseID,
                        Grade = Grade.B
                     },
                     new Enrollment {
                         StudentID = context.Students.Single(s => s.LastName == "Alonso").ID,
                        CourseID = context.Courses.Single(c => c.Title == "Trigonometry" ).CourseID,
                        Grade = Grade.B
                     },
                     new Enrollment {
                        StudentID = context.Students.Single(s => s.LastName == "Alonso").ID,
                        CourseID = context.Courses.Single(c => c.Title == "Composition" ).CourseID,
                        Grade = Grade.B
                     },
                     new Enrollment {
                        StudentID = context.Students.Single(s => s.LastName == "Anand").ID,
                        CourseID = context.Courses.Single(c => c.Title == "Chemistry" ).CourseID
                     },
                     new Enrollment {
                        StudentID = context.Students.Single(s => s.LastName == "Anand").ID,
                        CourseID = context.Courses.Single(c => c.Title == "Microeconomics").CourseID,
                        Grade = Grade.B
                     },
                    new Enrollment {
                        StudentID = context.Students.Single(s => s.LastName == "Barzdukas").ID,
                        CourseID = context.Courses.Single(c => c.Title == "Chemistry").CourseID,
                        Grade = Grade.B
                     },
                     new Enrollment {
                        StudentID = context.Students.Single(s => s.LastName == "Li").ID,
                        CourseID = context.Courses.Single(c => c.Title == "Composition").CourseID,
                        Grade = Grade.B
                     },
                     new Enrollment {
                        StudentID = context.Students.Single(s => s.LastName == "Justice").ID,
                        CourseID = context.Courses.Single(c => c.Title == "Literature").CourseID,
                        Grade = Grade.B
                     }
                };

                foreach (Enrollment e in enrollments)
                {
                    var enrollmentInDataBase = context.Enrollments.Where(
                        s =>
                             s.Student.ID == e.StudentID &&
                             s.Course.CourseID == e.CourseID).SingleOrDefault();
                    if (enrollmentInDataBase == null)
                    {
                        context.Enrollments.Add(e);
                    }
                }
                saveChangeCount += await context.SaveChangesAsync();

            }


            return saveChangeCount;
        }
        static void AddOrUpdateInstructor(SchoolDbContext context, string courseTitle, string instructorName)
        {
            var crs = context.Courses.SingleOrDefault(c => c.Title == courseTitle);
            var inst = crs.Instructors.SingleOrDefault(i => i.LastName == instructorName);
            if (inst == null)
                crs.Instructors.Add(context.Instructors.Single(i => i.LastName == instructorName));
        }
    }
}
