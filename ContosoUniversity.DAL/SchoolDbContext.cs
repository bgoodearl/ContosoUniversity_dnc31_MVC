using ContosoUniversity.DAL.Interfaces;
using ContosoUniversity.Models;
using System.Data.Entity;
using System.Threading;
using System.Threading.Tasks;

namespace ContosoUniversity.DAL
{
    public partial class SchoolDbContext : DbContext, ISchoolDbContext
    {
        public SchoolDbContext() : base("SchoolContext")
        {
        }

        public SchoolDbContext(string nameOrConnectionString) : base(nameOrConnectionString)
        {
        }

        #region Persistent Entities

        public DbSet<Course> Courses { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<OfficeAssignment> OfficeAssignments { get; set; }
        public DbSet<Student> Students { get; set; }

        #endregion Persistent Entities

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            return await base.SaveChangesAsync(cancellationToken);
        }

        public async Task<int> SeedInitialDataAsync()
        {
            return await SchoolDbContextSeed.SeedInitialData(this);
        }

    }
}
