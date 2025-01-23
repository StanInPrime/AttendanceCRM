namespace AttendanceCRM.Models
{
    public class FillAttendanceViewModel
    {
        public FillAttendanceViewModel()
        {
            AttendanceList = new List<Attendance>();
            StudentList = new List<Users>();
        }

        public List<Attendance> AttendanceList { get; set; }
        public List<Users> StudentList { get; set; }
        public DateTime StartDate { get; set; }
        public int CourseId { get; set; }
        public DateTime EndDate { get; set; }
        public bool? IsActive { get; set; }
    }
}
