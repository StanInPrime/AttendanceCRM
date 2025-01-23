using AttendanceCRM.Helpers.Constants;
using AttendanceCRM.Helpers.Services;
using AttendanceCRM.Models;
using DataTables.AspNetCore.Mvc.Binder;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;

namespace AttendanceCRM.Controllers
{
    [Authorize(Roles = "SuperAdmin,Teacher")]
    public class AttendanceController : Controller
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly AttendanceCRMContext _context;
        private readonly IConfiguration _configuration;

        public AttendanceController(AttendanceCRMContext context, IHttpContextAccessor contextAccessor, IConfiguration configuration)
        {
            _context = context;
            _contextAccessor = contextAccessor;
            _configuration = configuration;
        }

        /// <summary>
        /// The dashboard.
        /// </summary>
        /// <returns>The dashboard view.</returns>
        [ActionName("attendance")]
        public async Task<IActionResult> Attendance()
        {
            AttendanceViewModel model = new();
            return View(model);
        }

        [HttpPost]
        [ActionName("attendance")]
        public async Task<JsonResult> Attendance(string searchText = "")
        {
            DataTablesResponse<Courses> result = new DataTablesResponse<Courses>();
            List<Courses> list = await _context.Courses.Include(x => x.CourseYear).Include(x => x.CourseSubject).Include(x => x.CourseSemester).ToListAsync();
            if (!string.IsNullOrWhiteSpace(searchText) && list.Count > 0)
            {
                list = list.Where(x => x.Name.ToLower() == searchText.ToLower() || x.CourseSemester?.Name == searchText.ToLower() || x.CourseYear?.Name == searchText.ToLower()
                || x.CourseSubject?.Name == searchText.ToLower()).OrderBy(x => x.Name).ToList();
            }

            result.Data = list;
            if (result.Data != null && result.Data.Count() > 0)
            {
                int totalRecords = result.Data.Count();
                result.RecordsFiltered = totalRecords;
                result.RecordsTotal = totalRecords;
            }
            return Json(result, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [HttpGet]
        [ActionName("manageattendance")]
        public async Task<ActionResult> ManageAttendance(string data)
        {
            AttendanceViewModel model = new AttendanceViewModel();

            int decryptedId = 0;

            if (!string.IsNullOrWhiteSpace(data))
            {
                decryptedId = EncryptionDecryption.GetDecrypt(data).ToInteger();
            }
            if (decryptedId > 0)
            {
                Courses course = await _context.Courses.FirstOrDefaultAsync(x => x.Id == decryptedId);
                model.CourseName = course.Name;
            }

            List<AttendanceMonthYear> allMonthYearList = await _context.AttendanceMonthYear.Where(x => (x.IsActive ?? false) == true).ToListAsync();
            List<SelectListItem> monthYearList = new List<SelectListItem>();

            if (allMonthYearList.Any())
            {
                monthYearList.AddRange(allMonthYearList.Select(x =>
                new SelectListItem()
                {
                    Text = x.Name,
                    Value = x.Value,
                    Selected = x.Value.Split("/")[0] == DateTime.Now.Date.ToString() && x.Value.Split("/")[1] == DateTime.Now.Year.ToString() ? true : false
                }).ToList());
            }
            model.MonthYearList = monthYearList;
            model.CourseId = data;
            return this.View("ManageAttendance", model);
        }

        [HttpGet]
        [ActionName("getattenadance")]
        public async Task<ActionResult> GetAttenadance(string courseId, string monthYear)
        {
            FillAttendanceViewModel model = new FillAttendanceViewModel();

            int decryptedId = 0;

            if (!string.IsNullOrWhiteSpace(courseId))
            {
                decryptedId = EncryptionDecryption.GetDecrypt(courseId).ToInteger();
            }

            if (decryptedId > 0 && !string.IsNullOrEmpty(monthYear))
            {
                int month = monthYear.Split("/")[0].ToInteger();
                int year = monthYear.Split("/")[1].ToInteger();

                var firstDayOfMonth = new DateTime(year, month, 1);
                var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

                Courses course = await _context.Courses.FirstOrDefaultAsync(x => x.Id == decryptedId);
                model.IsActive = course.Active ?? false;
                List<CourseStudent> listCourseStudent = await _context.CourseStudent.Where(x => x.CourseId == decryptedId).Include(x=>x.Student).ToListAsync();
                model.StudentList = listCourseStudent.Select(x=>x.Student).ToList();
                model.CourseId = course.Id;
                List<Attendance> listAttendance = await _context.Attendance.Where(x => x.Date >= firstDayOfMonth && x.Date <= lastDayOfMonth).ToListAsync();
                model.AttendanceList = listAttendance;

                model.StartDate = firstDayOfMonth;
                model.EndDate = lastDayOfMonth;

            }

            return PartialView("_FillAttendance", model);
        }

        [HttpPost]
        [ActionName("saveattenadance")]
        public async Task<ActionResult> SaveAttenadance(string data)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(data))
                {
                    List<DailyAttendance> model = JsonConvert.DeserializeObject<List<DailyAttendance>>(data)?.ToList();
                    int decryptedId = 0;

                    if (!string.IsNullOrWhiteSpace(model.FirstOrDefault().CourseId))
                    {
                        decryptedId = EncryptionDecryption.GetDecrypt(model.FirstOrDefault().CourseId).ToInteger();
                    }
                    if (decryptedId > 0)
                    {
                        Courses course = await _context.Courses.FirstOrDefaultAsync(x => x.Id == decryptedId);
                        if (course != null)
                        {
                            if(course.Active ?? false)
                            {
                                List<CourseStudent> listCourseStudent = await _context.CourseStudent.Where(x => x.CourseId == decryptedId).Include(x => x.Student).ToListAsync();
                                var studentList = listCourseStudent.Select(x => x.Student).ToList();

                                List<Attendance> listAttendance = await _context.Attendance.Where(x => x.Date == DateTime.Now.Date && x.CourseId == decryptedId).ToListAsync();

                                foreach (var item in model)
                                {
                                    if (studentList.Any(x => x.Id == item.StudentId) && listAttendance.Any(x => x.StudentId == item.StudentId))
                                    {
                                        Attendance obj = listAttendance.FirstOrDefault(x => x.StudentId == item.StudentId);
                                        obj.IsPresent = item.Attendance;
                                        _context.Attendance.Update(obj);
                                        await _context.SaveChangesAsync();
                                    }
                                    else if (studentList.Any(x => x.Id == item.StudentId))
                                    {
                                        Attendance obj = new Attendance();
                                        obj.IsPresent = item.Attendance;
                                        obj.StudentId = item.StudentId;
                                        obj.Date = DateTime.Now.Date;
                                        obj.CourseId = decryptedId;
                                        await _context.Attendance.AddAsync(obj);
                                        await _context.SaveChangesAsync();
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                return this.Json(new { success = false, message = string.Format(Messages.SaveErrorMessage, "Attedance") }, new Newtonsoft.Json.JsonSerializerSettings());
            }
            return this.Json(new { success = true, message = string.Format(Messages.UpdateMessage, "Attedance") }, new Newtonsoft.Json.JsonSerializerSettings());
        }
    }
}
