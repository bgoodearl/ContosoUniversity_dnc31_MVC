using ContosoUniversity.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContosoUniversity.DAL.Repositories
{
    public class SchoolRepositoryFactory : ISchoolRepositoryFactory
    {
        private string NameOrConnectionString { get; }
        public SchoolRepositoryFactory(string nameOrConnectionString)
        {
            NameOrConnectionString = nameOrConnectionString ?? throw new ArgumentNullException(nameof(nameOrConnectionString));
        }

        public ISchoolRepository GetSchoolRepository()
        {
            return new SchoolRepository(new SchoolDbContext(NameOrConnectionString));
        }

    }
}
