using ContosoUniversity.Models;
using System.Data.Entity;

namespace ContosoUniversity.DAL
{
    public partial class SchoolDbContext : DbContext
    {
        public SchoolDbContext() : base("SchoolContext")
        {
        }

        public SchoolDbContext(string nameOrConnectionString) : base(nameOrConnectionString)
        {
        }

        public DbSet<Course> Courses { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<OfficeAssignment> OfficeAssignments { get; set; }
        public DbSet<Student> Students { get; set; }
    }
}
