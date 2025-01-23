namespace AttendanceCRM.Models
{
    public class DailyAttendance
    {
        public DateTime? AttendanceDate {  get; set; }
        public bool? Attendance {  get; set; }
        public int? StudentId {  get; set; }
        public string? CourseId {  get; set; }
        public int? Id {  get; set; }
    }
}
