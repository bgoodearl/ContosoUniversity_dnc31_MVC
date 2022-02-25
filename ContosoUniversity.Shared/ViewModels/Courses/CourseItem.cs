using Ardalis.GuardClauses;
using System.Collections.Generic;

namespace ContosoUniversity.Shared.ViewModels.Courses
{
    public class CourseItem : CourseListItem
    {
        public CourseItem()
        {
            Instructors = new List<IdItem>();
        }

        public CourseItem(CourseListItem listItem)
            : this()
        {
            if ((listItem != null) && (listItem != this))
            {
                this.CourseID = listItem.CourseID;
                this.Credits = listItem.Credits;
                this.Department = listItem.Department;
                this.Title = listItem.Title;
            }
        }

        public void SetInstructors(List<IdItem> instructors)
        {
            Guard.Against.Null(instructors, nameof(instructors));
            Instructors = instructors;
        }

        public List<IdItem> Instructors { get; private set; }
    }
}
