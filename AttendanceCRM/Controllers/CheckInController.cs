using AttendanceCRM.Helpers.Constants;
using AttendanceCRM.Helpers.Services;
using AttendanceCRM.Models;
using AttendanceCRM.Models.Custom;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NuGet.Packaging;

namespace AttendanceCRM.Controllers
{
    public class CheckInController : Controller
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly AttendanceCRMContext _context;
        private readonly IConfiguration _configuration;

        public CheckInController(AttendanceCRMContext context, IHttpContextAccessor contextAccessor, IConfiguration configuration)
        {
            _context = context;
            _contextAccessor = contextAccessor;
            _configuration = configuration;
        }

        [HttpGet]
        [Authorize(Roles = "Student")]
        [ActionName("checkin")]
        public async Task<ActionResult> CheckIn()
        {
            CheckInViewModel model = new CheckInViewModel();
            List<RecentAttendance> recentAttendance = new List<RecentAttendance>();
            var userIdClaim = _contextAccessor.HttpContext.User.FindFirst($"UserId");
            var userId = Convert.ToInt32(userIdClaim == null ? "0" : userIdClaim.Value);

            DateTime startDate = DateTime.Now.Date;
            DateTime endDate = DateTime.Now.Date.AddDays(-7);

            List<Attendance> listAttendance = await _context.Attendance.Where(x => x.Date <= startDate && x.Date >= endDate && x.StudentId == userId && (x.IsPresent ?? false) == true).ToListAsync();

            foreach (var data in listAttendance)
            {
                Courses course = await _context.Courses.FirstOrDefaultAsync(x => x.Id == data.CourseId);
                if(course != null)
                {
                    if (recentAttendance.Any(x => x.AttendanceDate == data.Date))
                    {
                        recentAttendance.FirstOrDefault(x => x.AttendanceDate == data.Date).CourseName.Add(course.Name);
                    }
                    else
                    {
                        recentAttendance.Add(
                            new RecentAttendance
                            {
                                AttendanceDate = data.Date.Value,
                                CourseName = new List<string> { course.Name }
                            }
                        );
                    }
                }
            }

            recentAttendance = recentAttendance.Count > 0 ? recentAttendance.OrderByDescending(x=>x.AttendanceDate).ToList() : new List<RecentAttendance>();
            model.RecentAttendance = recentAttendance;
            return View(model);
        }

        [HttpPost]
        [ActionName("saveattenadance")]
        public async Task<ActionResult> SaveAttenadance(string data)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(data))
                {
                    Courses course = await _context.Courses.FirstOrDefaultAsync(x => x.CourseCode == data && (x.Active ?? false) == true);
                    if (course != null)
                    {
                        var userIdClaim = _contextAccessor.HttpContext.User.FindFirst($"UserId");
                        var userId = Convert.ToInt32(userIdClaim == null ? "0" : userIdClaim.Value);

                        var userFullNameClaim = _contextAccessor.HttpContext.User.FindFirst($"FullName");
                        var userFullName = userFullNameClaim == null ? "" : userFullNameClaim.Value;

                        List<CourseStudent> listCourseStudent = await _context.CourseStudent.Where(x => x.CourseId == course.Id && x.StudentId == userId).ToListAsync();
                        if (listCourseStudent.Count > 0) 
                        {
                            List<Attendance> listAttendance = await _context.Attendance.Where(x => x.Date == DateTime.Now.Date && x.CourseId == course.Id && x.StudentId == userId).ToListAsync();
                            if (listAttendance.Count == 1)
                            {
                                Attendance obj = listAttendance.FirstOrDefault();
                                obj.IsPresent = true;
                                _context.Attendance.Update(obj);
                                await _context.SaveChangesAsync();
                            }
                            else
                            {
                                Attendance obj = new Attendance();
                                obj.IsPresent = true;
                                obj.StudentId = userId;
                                obj.Date = DateTime.Now.Date;
                                obj.CourseId = course.Id;
                                await _context.Attendance.AddAsync(obj);
                                await _context.SaveChangesAsync();

                                if (!string.IsNullOrEmpty(userFullName))
                                {
                                    Notification objNotification = new Notification();
                                    objNotification.TeacherId = course.TeacherId;
                                    objNotification.Message = userFullName + " filled attendance for " + course.Name + " course.";
                                    objNotification.IsAcknowledged = false;
                                    objNotification.AddedOn = DateTime.Now;
                                    await _context.Notification.AddAsync(objNotification);
                                    await _context.SaveChangesAsync();
                                }
                            }
                        }
                        else
                        {
                            return this.Json(new { success = false, message = "Please enter valid checkin code." }, new Newtonsoft.Json.JsonSerializerSettings());
                        }
                    }
                    else
                    {
                        return this.Json(new { success = false, message = "Please enter valid checkin code." }, new Newtonsoft.Json.JsonSerializerSettings());
                    }
                }
            }
            catch (Exception)
            {
                return this.Json(new { success = false, message = string.Format(Messages.SaveErrorMessage, "Attedance") }, new Newtonsoft.Json.JsonSerializerSettings());
            }
            return this.Json(new { success = true, message = string.Format(Messages.SaveSuccessMessage, "Attedance") }, new Newtonsoft.Json.JsonSerializerSettings());
        }
    }
}
