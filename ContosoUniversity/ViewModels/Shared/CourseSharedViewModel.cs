using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace ContosoUniversity.ViewModels.Shared
{
    public class CourseSharedViewModel
    {
        public IEnumerable<SelectListItem> Departments { get; set; } = new List<SelectListItem>();
    }
}
