using ContosoUniversity.Shared.ViewModels.Courses;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using static ContosoUniversity.Shared.CommonDefs;

namespace ContosoUniversity.Components
{
    public partial class CourseDetails
    {
        [Parameter] public CourseItem Course { get; set; }
    }
}
