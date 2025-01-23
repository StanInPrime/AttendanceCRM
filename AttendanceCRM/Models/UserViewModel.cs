using Microsoft.AspNetCore.Mvc.Rendering;

namespace AttendanceCRM.Models
{
    public class UserViewModel
    {
        public UserViewModel()
        {
            User = new Users();
            GenderList = new List<SelectListItem>();
        }

        public Users User { get; set; }
        public List<SelectListItem> GenderList { get; set; }
    }
}
