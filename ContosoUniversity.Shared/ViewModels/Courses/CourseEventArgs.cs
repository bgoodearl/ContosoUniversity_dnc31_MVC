using static ContosoUniversity.Shared.CommonDefs;

namespace ContosoUniversity.Shared.ViewModels.Courses
{
    public class CourseEventArgs
    {
        public int CourseID { get; set; }
        public UIMode UIMode { get; set; }
    }
}
