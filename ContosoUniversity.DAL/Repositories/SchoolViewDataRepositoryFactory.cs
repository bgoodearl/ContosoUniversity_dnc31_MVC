using Ardalis.GuardClauses;
using ContosoUniversity.Common.Interfaces;
using ContosoUniversity.Shared.Interfaces;

namespace ContosoUniversity.DAL.Repositories
{
    public class SchoolViewDataRepositoryFactory : ISchoolViewDataRepositoryFactory
    {
        public SchoolViewDataRepositoryFactory(ISchoolRepositoryFactory schoolRepositoryFactory)
        {
            Guard.Against.Null(schoolRepositoryFactory, nameof(schoolRepositoryFactory));
            SchoolRepositoryFactory = schoolRepositoryFactory;
        }

        protected ISchoolRepositoryFactory SchoolRepositoryFactory { get; }

        public ISchoolViewDataRepository GetViewDataRepository()
        {
            return new SchoolViewDataRepository(SchoolRepositoryFactory);
        }
    }
}
