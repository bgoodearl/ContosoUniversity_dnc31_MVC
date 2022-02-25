using Ardalis.GuardClauses;
using ContosoUniversity.Common.Interfaces;
using ContosoUniversity.DAL.Interfaces;
using ContosoUniversity.Shared.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace ContosoUniversity.Controllers
{
    public class CUControllerBase : Controller
    {
        public CUControllerBase(IHttpContextAccessor httpContextAccessor)
        {
            Guard.Against.Null(httpContextAccessor, nameof(httpContextAccessor));
            HttpContextAccessor = httpContextAccessor;
            Guard.Against.Null(httpContextAccessor.HttpContext, nameof(httpContextAccessor.HttpContext));
            Guard.Against.Null(httpContextAccessor.HttpContext.RequestServices, nameof(httpContextAccessor.HttpContext.RequestServices));
            SchoolDbContextFactory = httpContextAccessor.HttpContext.RequestServices.GetRequiredService<ISchoolDbContextFactory>();
            SchoolRepositoryFactory = httpContextAccessor.HttpContext.RequestServices.GetRequiredService<ISchoolRepositoryFactory>();
            SchoolViewDataRepositoryFactory = httpContextAccessor.HttpContext.RequestServices.GetRequiredService<ISchoolViewDataRepositoryFactory>();
        }

        #region Read Only variables

        protected IHttpContextAccessor HttpContextAccessor { get; }

        #endregion Read Only variables


        #region Repository/Database

        ISchoolViewDataRepositoryFactory SchoolViewDataRepositoryFactory { get; }
        protected ISchoolViewDataRepository GetSchoolViewDataRepository()
        {
            return SchoolViewDataRepositoryFactory.GetViewDataRepository();
        }

        ISchoolDbContextFactory SchoolDbContextFactory { get; }
        protected ISchoolDbContext GetSchoolDbContext()
        {
            return SchoolDbContextFactory.GetSchoolDbContext();
        }

        protected ISchoolRepositoryFactory SchoolRepositoryFactory { get; }
        protected ISchoolRepository GetSchoolRepository()
        {
            return SchoolRepositoryFactory.GetSchoolRepository();
        }

        #endregion Repository/Database

    }
}
