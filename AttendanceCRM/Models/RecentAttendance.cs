namespace AttendanceCRM.Models
{
    public class RecentAttendance
    {
        public RecentAttendance()
        {
            CourseName = new List<string>();
        }
        public DateTime AttendanceDate { get; set; }
        public List<string> CourseName { get; set; }
    }
}
