namespace AttendanceCRM.Models
{
    public class CheckInViewModel
    {
        public CheckInViewModel()
        {
            RecentAttendance = new List<RecentAttendance>();
        }

        public List<RecentAttendance> RecentAttendance { get; set; }
    }
}
