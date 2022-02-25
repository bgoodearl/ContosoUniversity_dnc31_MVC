using System;
using System.ComponentModel.DataAnnotations;

namespace ContosoUniversity.Shared.ViewModels.Instructors
{
    public class InstructorListItem
    {
        public int ID { get; set; }
        public string FirstMidName { get; set; }

        [Display(Name = "Full Name")]
        public string FullName
        {
            get { return LastName + ", " + FirstMidName; }
        }

        public DateTime HireDate { get; set; }
        public string LastName { get; set; }
        public string OfficeAssignment { get; set; }
    }
}
