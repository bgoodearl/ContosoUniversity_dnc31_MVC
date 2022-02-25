using System;
using System.ComponentModel.DataAnnotations;

namespace ContosoUniversity.Shared.ViewModels.Departments
{
    public class DepartmentListItem
    {
        public int DepartmentID { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Budget { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }
        public int? InstructorID { get; set; }
        public string Administrator { get; set; } = string.Empty;
    }
}
