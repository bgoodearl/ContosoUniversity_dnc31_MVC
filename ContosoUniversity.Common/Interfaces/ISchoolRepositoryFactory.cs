
using ContosoUniversity.Shared.ViewModels.Courses;
using System.Threading.Tasks;

namespace ContosoUniversity.Common.Interfaces
{
    public interface ISchoolRepositoryFactory
    {
        ISchoolRepository GetSchoolRepository();
    }
}
