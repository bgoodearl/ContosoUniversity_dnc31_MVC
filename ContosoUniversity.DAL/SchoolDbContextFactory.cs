using ContosoUniversity.DAL.Interfaces;
using System;

namespace ContosoUniversity.DAL
{
    public class SchoolDbContextFactory : ISchoolDbContextFactory
    {
        protected string NameOrConnectionString { get; }

        public SchoolDbContextFactory(string nameOrConnectionString)
        {
            NameOrConnectionString = nameOrConnectionString ?? throw new ArgumentNullException(nameof(nameOrConnectionString));
        }

        public ISchoolDbContext GetSchoolDbContext()
        {
            return new SchoolDbContext(NameOrConnectionString);
        }
    }
}
