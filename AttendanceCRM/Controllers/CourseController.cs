using AttendanceCRM.Helpers.Constants;
using AttendanceCRM.Helpers.Services;
using AttendanceCRM.Models;
using DataTables.AspNetCore.Mvc.Binder;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Resources;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace AttendanceCRM.Controllers
{
    
    public class CourseController : Controller
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly AttendanceCRMContext _context;
        private readonly IConfiguration _configuration;

        public CourseController(AttendanceCRMContext context, IHttpContextAccessor contextAccessor, IConfiguration configuration)
        {
            _context = context;
            _contextAccessor = contextAccessor;
            _configuration = configuration;
        }

        [HttpGet]
        [Authorize(Roles = "SuperAdmin,Student,Teacher")]
        [ActionName("course")]
        public ActionResult Course()
        {
            CourseViewModel model = new CourseViewModel();
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin,Student,Teacher")]
        [ActionName("course")]
        public async Task<JsonResult> Course(string searchText = "")
        {
            var roles = _contextAccessor.HttpContext.User.FindFirst($"http://schemas.microsoft.com/ws/2008/06/identity/claims/role");
            var roleName = roles == null ? "Student" : roles.Value;

            var userIdClaim = _contextAccessor.HttpContext.User.FindFirst($"UserId");
            var userId = Convert.ToInt32(userIdClaim == null ? "0" : userIdClaim.Value);

            DataTablesResponse<Courses> result = new DataTablesResponse<Courses>();
            List<Courses> list = await _context.Courses.Include(x=>x.CourseYear).Include(x => x.CourseSubject).Include(x => x.CourseSemester).ToListAsync();
            if (!string.IsNullOrWhiteSpace(searchText) && list.Count > 0)
            {
                list = list.Where(x => x.Name.ToLower() == searchText.ToLower() || x.CourseSemester?.Name == searchText.ToLower() || x.CourseYear?.Name == searchText.ToLower()
                || x.CourseSubject?.Name == searchText.ToLower()).OrderBy(x => x.Name).ToList();
            }

            if (roleName == "Student" && list.Count > 0)
            {
                List<CourseStudent> studentCourseId = await _context.CourseStudent.Where(x => x.StudentId == userId).ToListAsync();
                if(studentCourseId.Count > 0)
                {
                    list = list.Where(x => studentCourseId.Any(y => y.CourseId == x.Id)).ToList();
                }
                else
                {
                    list = new List<Courses>();
                }
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
        [Authorize(Roles = "SuperAdmin,Teacher")]
        [ActionName("managecourse")]
        public async Task<ActionResult> ManageCourse(string data)
        {
            CourseViewModel model = new CourseViewModel();
            int decryptedId = 0;

            if (!string.IsNullOrWhiteSpace(data))
            {
                decryptedId = EncryptionDecryption.GetDecrypt(data).ToInteger();
            }

            if (decryptedId > 0)
            {
                Courses course = await _context.Courses.FirstOrDefaultAsync(x => x.Id == decryptedId);
                model.Course = course;

                List<CourseStudent> listCourseStudent = await _context.CourseStudent.Where(x => x.CourseId == decryptedId).ToListAsync();
                model.Course.SelectedStudents = listCourseStudent.Select(x=>x.StudentId.ToString()).ToList();
            }
            else
            {
                model.Course.Active = true;
            }
            var allCourseYear = await _context.CourseYear.ToListAsync();
            List<SelectListItem> yearList = new List<SelectListItem>();
            
            if (allCourseYear.Any())
            {
                yearList.AddRange(allCourseYear.Select(x =>
                new SelectListItem()
                {
                    Text = x.Name,
                    Value = x.Id.ToString(),
                }).ToList());
            }

            var allCourseSemester = await _context.CourseSemester.ToListAsync();
            List<SelectListItem> semesterList = new List<SelectListItem>();
            
            if (allCourseSemester.Any())
            {
                semesterList.AddRange(allCourseSemester.Select(x =>
                new SelectListItem()
                {
                    Text = x.Name,
                    Value = x.Id.ToString(),
                }).ToList());
            }

            var allCourseSubject = await _context.CourseSubject.ToListAsync();
            List<SelectListItem> subjectList = new List<SelectListItem>();
            
            if (allCourseSubject.Any())
            {
                subjectList.AddRange(allCourseSubject.Select(x =>
                new SelectListItem()
                {
                    Text = x.Name,
                    Value = x.Id.ToString(),
                }).ToList());
				subjectList.AddRange(allCourseSubject.Select(x =>
				new SelectListItem()
				{
					Text = x.Name,
					Value = x.Id.ToString(),
				}).ToList());
				subjectList.AddRange(allCourseSubject.Select(x =>
				new SelectListItem()
				{
					Text = x.Name,
					Value = x.Id.ToString(),
				}).ToList());
				subjectList.AddRange(allCourseSubject.Select(x =>
				new SelectListItem()
				{
					Text = x.Name,
					Value = x.Id.ToString(),
				}).ToList());
				subjectList.AddRange(allCourseSubject.Select(x =>
				new SelectListItem()
				{
					Text = x.Name,
					Value = x.Id.ToString(),
				}).ToList());
				subjectList.AddRange(allCourseSubject.Select(x =>
				new SelectListItem()
				{
					Text = x.Name,
					Value = x.Id.ToString(),
				}).ToList());
			}

            List<Users> allStudentList = await _context.Users.Where(x => (x.IsStudent ?? false) == true).ToListAsync();
            List<SelectListItem> studentList = new List<SelectListItem>();
            if (allStudentList.Any())
            {
                studentList.AddRange(allStudentList.Select(x =>
                new SelectListItem()
                {
                    Text = x.FirstName + " " + x.LastName,
                    Value = x.Id.ToString(),
                }).ToList());
            }

            List<Users> allTeacherList = await _context.Users.Where(x => (x.IsStudent ?? false) == false && (x.IsSuperAdmin ?? false) == false).ToListAsync();
            List<SelectListItem> teacherList = new List<SelectListItem>();
            if (allTeacherList.Any())
            {
                teacherList.AddRange(allTeacherList.Select(x =>
                new SelectListItem()
                {
                    Text = x.FirstName + " " + x.LastName,
                    Value = x.Id.ToString(),
                }).ToList());
            }

            model.CourseYearList = yearList;
            model.CourseSemesterList = semesterList;
            model.CourseSubjectList = subjectList;
            model.CourseStudentList = studentList;
            model.CourseTeacherList = teacherList;

            return this.View("ManageCourse", model);
        }

        [Authorize(Roles = "SuperAdmin,Teacher")]
        [HttpPost]
        [ActionName("managecourse")]
        public async Task<ActionResult> ManageCourse(CourseViewModel courseViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Courses model = courseViewModel.Course;
                    //if (model.SelectedStudents == null)
                    //{

                    //}
                    bool isAlreadyExist = await _context.Courses.AnyAsync(x => x.Name == model.Name && x.Id != model.Id);
                    if (isAlreadyExist)
                    {
                        return Json(new { success = false, message = string.Format("{0} already exists.", model.Name) }, new Newtonsoft.Json.JsonSerializerSettings());
                    }

                    var userIdClaim = _contextAccessor.HttpContext.User.FindFirst($"UserId");
                    var userId = Convert.ToInt32(userIdClaim == null ? "0" : userIdClaim.Value);

                    if (model.Id > 0)
                    {

                        Courses course = await _context.Courses.FirstOrDefaultAsync(x => x.Id == model.Id);
                        course.Name = model.Name;
                        course.CourseSubjectId = model.CourseSubjectId;
                        course.CourseSemesterId = model.CourseSemesterId;
                        course.CourseYearId = model.CourseYearId;
                        course.TeacherId = model.TeacherId;
                        course.Active = model.Active;
                        course.ModifiedBy = userId;
                        course.ModifiedDate = DateTime.Now;
                        _context.Courses.Update(course);
                        await _context.SaveChangesAsync();

                        var oldStudentlist = await _context.CourseStudent.Where(x => x.CourseId == model.Id).ToListAsync();
                        if (oldStudentlist != null)
                        {
                            foreach (var oldStudent in oldStudentlist)
                            {
                                if (model.SelectedStudents != null && oldStudent.StudentId > 0)
                                {
                                    if (!model.SelectedStudents.ToList().Contains(oldStudent.StudentId.ToString()))
                                    {
                                        List<Attendance> listAttendance = await _context.Attendance.Where(x => x.CourseId == model.Id && x.StudentId == oldStudent.Id).ToListAsync();
                                        _context.Attendance.RemoveRange(listAttendance);
                                        await _context.SaveChangesAsync();

                                        CourseStudent courseStudent = await _context.CourseStudent.FirstOrDefaultAsync(x => x.Id == oldStudent.Id);
                                        _context.CourseStudent.Remove(courseStudent);
                                        await _context.SaveChangesAsync();
                                    }
                                }
                            }
                        }

                        if (model.SelectedStudents != null)
                        {
                            List<CourseStudent> Newlist = new List<CourseStudent>();

                            foreach (var studentId in model.SelectedStudents)
                            {
                                if (!oldStudentlist.Any(m => m.StudentId == studentId.ToInteger()))
                                {
                                    CourseStudent obj = new CourseStudent();
                                    obj.StudentId = studentId.ToInteger();
                                    obj.CourseId = model.Id;
                                    await _context.CourseStudent.AddAsync(obj);
                                    await _context.SaveChangesAsync();
                                }
                            }
                        }

                        this.TempData["SuccessMessage"] = string.Format(Messages.UpdateMessage, "Course details");
                        return this.Json(new { success = true, message = string.Format(Messages.UpdateMessage, "Course details") }, new Newtonsoft.Json.JsonSerializerSettings());
                    }
                    else
                    {
                        model.CourseCode = await RandomString();
                        model.CreatedDate = DateTime.Now;
                        model.CreatedBy = userId;
                        await _context.Courses.AddAsync(model);
                        await _context.SaveChangesAsync();

                        foreach (var studentId in model.SelectedStudents)
                        {
                            CourseStudent obj = new CourseStudent();
                            obj.StudentId = studentId.ToInteger();
                            obj.CourseId = model.Id;
                            await _context.CourseStudent.AddAsync(obj);
                            await _context.SaveChangesAsync();
                        }

                        this.TempData["SuccessMessage"] = string.Format(Messages.SaveSuccessMessage, "Course details");
                        return this.Json(new { success = true, message = string.Format(Messages.SaveSuccessMessage, "Course details") }, new Newtonsoft.Json.JsonSerializerSettings());

                    }
                }
                catch (Exception)
                {
                    return this.Json(new { success = false, message = string.Format(Messages.SaveErrorMessage, "Course details") }, new Newtonsoft.Json.JsonSerializerSettings());
                }
            }

            return this.Json(new { success = false, message = Messages.ModelIsNotValid }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [ActionName("deletecourse")]
        [Authorize(Roles = "SuperAdmin,Teacher")]
        public async Task<ActionResult> DeleteCourse(string id)
        {
            try
            {
                int decryptedId = EncryptionDecryption.GetDecrypt(id).ToInteger();

                List<CourseStudent> listCourseStudent = await _context.CourseStudent.Where(x => x.CourseId == decryptedId).ToListAsync();
                _context.CourseStudent.RemoveRange(listCourseStudent);
                await _context.SaveChangesAsync();

                List<Attendance> listAttendance = await _context.Attendance.Where(x => x.CourseId == decryptedId).ToListAsync();
                _context.Attendance.RemoveRange(listAttendance);
                await _context.SaveChangesAsync();

                Courses course = await _context.Courses.FirstOrDefaultAsync(x => x.Id == decryptedId);
                _context.Courses.Remove(course);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return this.Json(new { success = false, message = string.Format(Messages.DeleteErrorMessage, "Course") }, new Newtonsoft.Json.JsonSerializerSettings());
            }
            return this.Json(new { success = true, message = string.Format(Messages.DeleteMessage, "Course") }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [HttpPost]
        [ActionName("generatecoursecode")]
        public async Task<ActionResult> GenerateCourseCode(string data)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(data))
                {
                    int decryptedId = 0;
                    decryptedId = EncryptionDecryption.GetDecrypt(data).ToInteger();
                    Courses course = await _context.Courses.FirstOrDefaultAsync(x => x.Id == decryptedId);
                    if (course != null)
                    {
                        course.CourseCode = await RandomString();
                        _context.Courses.Update(course);
                        await _context.SaveChangesAsync();
                    }
                }
            }
            catch (Exception)
            {
                return this.Json(new { success = false, message = string.Format(Messages.SaveErrorMessage, "Course Checkin Code") }, new Newtonsoft.Json.JsonSerializerSettings());
            }
            return this.Json(new { success = true, message = string.Format(Messages.UpdateMessage, "Course Checkin Code") }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        public async Task<string> RandomString()
        {
            Random random = new Random();
            const string chars = "0123456789";
            string courseCheckInCode = new string(Enumerable.Repeat(chars, 5).Select(s => s[random.Next(s.Length)]).ToArray());
            bool isAlreadyExistUserId = await _context.Courses.AnyAsync(x => (x.Active ?? false) == true && x.CourseCode == courseCheckInCode);
            if (isAlreadyExistUserId)
            {
                return await RandomString();
            }
            return courseCheckInCode;
        }
    }
}
