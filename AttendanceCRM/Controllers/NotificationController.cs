using AttendanceCRM.Helpers.Constants;
using AttendanceCRM.Helpers.Services;
using AttendanceCRM.Models;
using DataTables.AspNetCore.Mvc.Binder;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AttendanceCRM.Controllers
{
    public class NotificationController : Controller
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly AttendanceCRMContext _context;
        private readonly IConfiguration _configuration;

        public NotificationController(AttendanceCRMContext context, IHttpContextAccessor contextAccessor, IConfiguration configuration)
        {
            _context = context;
            _contextAccessor = contextAccessor;
            _configuration = configuration;
        }

        [HttpGet]
        [Authorize(Roles = "SuperAdmin,Teacher")]
        [ActionName("notification")]
        public ActionResult Notification()
        {
            NotificationViewModel model = new NotificationViewModel();
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin,Teacher")]
        [ActionName("notification")]
        public async Task<JsonResult> Notification(string searchText = "")
        {
            DataTablesResponse<Notification> result = new DataTablesResponse<Notification>();

            var userIdClaim = _contextAccessor.HttpContext.User.FindFirst($"UserId");
            var userId = Convert.ToInt32(userIdClaim == null ? "0" : userIdClaim.Value);

            List<Notification> notificationList = new List<Notification>();
            notificationList = await _context.Notification.Where(x => (x.IsAcknowledged ?? false) == false && x.TeacherId == userId).ToListAsync();

            result.Data = notificationList;
            if (result.Data != null && result.Data.Count() > 0)
            {
                int totalRecords = result.Data.Count();
                result.RecordsFiltered = totalRecords;
                result.RecordsTotal = totalRecords;
            }
            return Json(result, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [HttpPost]
        [ActionName("acknowledge")]
        public async Task<ActionResult> Acknowledge(string data)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(data))
                {
                    int decryptedId = 0;
                    decryptedId = EncryptionDecryption.GetDecrypt(data).ToInteger();
                    Notification notification = await _context.Notification.FirstOrDefaultAsync(x => x.Id == decryptedId);
                    if (notification != null)
                    {
                        notification.IsAcknowledged = true;
                        _context.Notification.Update(notification);
                        await _context.SaveChangesAsync();
                    }
                }
            }
            catch (Exception)
            {
                return this.Json(new { success = false, message = string.Format(Messages.SaveErrorMessage, "Notification") }, new Newtonsoft.Json.JsonSerializerSettings());
            }
            return this.Json(new { success = true, message = string.Format(Messages.AcknowledgedMessage, "Notification") }, new Newtonsoft.Json.JsonSerializerSettings());
        }
    }
}
