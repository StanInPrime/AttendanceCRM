using AttendanceCRM.Helpers.Services;
using DataTables.AspNetCore.Mvc.Binder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Data;

namespace AttendanceCRM.Models
{
    [ViewComponent(Name = "TopMenu")]
    public class TopMenu : ViewComponent
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly AttendanceCRMContext _context;
        private readonly IConfiguration _configuration;

        public TopMenu(AttendanceCRMContext context, IHttpContextAccessor contextAccessor, IConfiguration configuration)
        {
            _context = context;
            _contextAccessor = contextAccessor;
            _configuration = configuration;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            TopMenuViewModel model = new TopMenuViewModel();

            var roles = _contextAccessor.HttpContext.User.FindFirst($"http://schemas.microsoft.com/ws/2008/06/identity/claims/role");
            var roleName = roles == null ? "Student" : roles.Value;

            if(roleName == "Teacher")
            {
                model.IsShowNotification = true;
                var userIdClaim = _contextAccessor.HttpContext.User.FindFirst($"UserId");
                var userId = Convert.ToInt32(userIdClaim == null ? "0" : userIdClaim.Value);

                List<Notification> notificationList = new List<Notification>();
                notificationList = await _context.Notification.Where(x => (x.IsAcknowledged ?? false) == false && x.TeacherId == userId).ToListAsync();
                model.Notification = notificationList;
            }
            else
            {
                model.IsShowNotification = false;
            }

            return View(model);
        }
    }
}
