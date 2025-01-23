using Microsoft.AspNetCore.Mvc.Rendering;

namespace AttendanceCRM.Models
{
    public class CourseStudentViewModel
    {
        public CourseStudentViewModel()
        {
            CourseStudent = new CourseStudent();
        }

        public CourseStudent CourseStudent { get; set; }
    }
}
