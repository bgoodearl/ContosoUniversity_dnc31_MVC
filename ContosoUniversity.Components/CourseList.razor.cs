using ContosoUniversity.Shared.ViewModels.Courses;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using static ContosoUniversity.Shared.CommonDefs;

namespace ContosoUniversity.Components
{
    public partial class CourseList
    {
        [Parameter] public CoursesViewModel CoursesVM { get; set; }

        [Parameter] public EventCallback<CourseEventArgs> CourseAction { get; set; }

        public async Task OnItemDelete(CourseListItem item)
        {
            CourseEventArgs args = new CourseEventArgs
            {
                CourseID = item.CourseID,
                UIMode = UIMode.Delete
            };
            await CourseAction.InvokeAsync(args);
        }

        public async Task OnItemDetails(CourseListItem item)
        {
            CourseEventArgs args = new CourseEventArgs
            {
                CourseID = item.CourseID,
                UIMode = UIMode.Details
            };
            await CourseAction.InvokeAsync(args);
        }

        public async Task OnItemEdit(CourseListItem item)
        {
            CourseEventArgs args = new CourseEventArgs
            {
                CourseID = item.CourseID,
                UIMode = UIMode.Edit
            };
            await CourseAction.InvokeAsync(args);
        }

    }
}
