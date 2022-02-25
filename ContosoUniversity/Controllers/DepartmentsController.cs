using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContosoUniversity.Common.Interfaces;
using ContosoUniversity.Shared.ViewModels.Departments;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ContosoUniversity.Controllers
{
    [Route("[Controller]/[Action]")]
    public class DepartmentsController : CUControllerBase
    {
        public DepartmentsController(IHttpContextAccessor httpContextAccessor)
            : base(httpContextAccessor)
        {
        }

        [Route("~/[Controller]")]
        public async Task<IActionResult> Index()
        {
            using (ISchoolRepository repo = GetSchoolRepository())
            {
                List<DepartmentListItem> departments = await repo.GetDepartmentListItemsNoTrackingAsync();
                return View(departments);
            }
        }

    }
}
