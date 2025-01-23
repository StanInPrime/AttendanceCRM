using Microsoft.AspNetCore.Mvc.Rendering;

namespace AttendanceCRM.Models
{
    public class AttendanceViewModel
    {
        public AttendanceViewModel()
        {
            MonthYearList = new List<SelectListItem>();
        }

        public List<SelectListItem> MonthYearList { get; set; }
        public string CourseId { get; set; }
        public string CourseName { get; set; }
    }
}
