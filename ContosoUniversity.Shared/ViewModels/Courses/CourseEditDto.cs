using System.ComponentModel.DataAnnotations;

namespace ContosoUniversity.Shared.ViewModels.Courses
{
    public class CourseEditDto
    {
        [Range(1,9999)]
        public int CourseID { get; set; }

        [StringLength(50, MinimumLength = 3)]
        public string Title { get; set; } = string.Empty;

        [Range(0, 5)]
        public int Credits { get; set; }

        public int DepartmentID { get; set; } = 0;
    }
}
