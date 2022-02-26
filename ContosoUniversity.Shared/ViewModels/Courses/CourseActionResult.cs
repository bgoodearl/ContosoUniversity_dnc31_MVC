
namespace ContosoUniversity.Shared.ViewModels.Courses
{
    public class CourseActionResult
    {
        public string Action { get; set; }
        public int CourseID { get; set; }
        public int ChangeCount { get; set; }
        public string ErrorMessage { get; set; }
    }
}
