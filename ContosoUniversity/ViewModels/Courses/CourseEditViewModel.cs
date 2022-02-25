using ContosoUniversity.Shared.ViewModels.Courses;

namespace ContosoUniversity.ViewModels.Courses
{
    public class CourseEditViewModel : Shared.CourseSharedViewModel
    {
        public CourseEditDto Course { get; set; } = new CourseEditDto();
    }
}
