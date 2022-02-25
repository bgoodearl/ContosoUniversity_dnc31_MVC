using Ardalis.GuardClauses;
using ContosoUniversity.Common.Interfaces;
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
            SchoolRepositoryFactory = httpContextAccessor.HttpContext.RequestServices.GetRequiredService<ISchoolRepositoryFactory>();
        }

        #region Read Only variables

        protected IHttpContextAccessor HttpContextAccessor { get; }

        #endregion Read Only variables


        #region Repository

        protected ISchoolRepositoryFactory SchoolRepositoryFactory { get; }
        protected ISchoolRepository GetSchoolRepository()
        {
            return SchoolRepositoryFactory.GetSchoolRepository();
        }

        #endregion Repository

    }
}
