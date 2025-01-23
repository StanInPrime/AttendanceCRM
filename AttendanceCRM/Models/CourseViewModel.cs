using Microsoft.AspNetCore.Mvc.Rendering;

namespace AttendanceCRM.Models
{
    public class CourseViewModel
    {
        public CourseViewModel()
        {
            Course = new Courses();
            CourseYearList = new List<SelectListItem>();
            CourseSubjectList = new List<SelectListItem>();
            CourseSemesterList = new List<SelectListItem>();
            CourseStudentList = new List<SelectListItem>();
            CourseTeacherList = new List<SelectListItem>();
        }

        public Courses Course { get; set; }
        public List<SelectListItem> CourseSemesterList { get; set; }
        public List<SelectListItem> CourseYearList { get; set; }
        public List<SelectListItem> CourseSubjectList { get; set; }
        public List<SelectListItem> CourseStudentList { get; set; }
        public List<SelectListItem> CourseTeacherList { get; set; }
    }
}