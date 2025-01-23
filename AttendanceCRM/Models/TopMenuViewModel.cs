namespace AttendanceCRM.Models
{
    public class TopMenuViewModel
    {
        public TopMenuViewModel() 
        {
            Notification = new List<Notification>();
        }

        public List<Notification> Notification { get; set; }
        public bool IsShowNotification { get; set; }
    }
}
