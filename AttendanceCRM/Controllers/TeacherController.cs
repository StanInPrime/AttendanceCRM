using AttendanceCRM.Helpers.Constants;
using AttendanceCRM.Helpers.Services;
using AttendanceCRM.Models;
using DataTables.AspNetCore.Mvc.Binder;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AttendanceCRM.Controllers
{
    [Authorize(Roles = "SuperAdmin,Teacher")]
    public class TeacherController : Controller
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly AttendanceCRMContext _context;
        private readonly IConfiguration _configuration;

        public TeacherController(AttendanceCRMContext context, IHttpContextAccessor contextAccessor, IConfiguration configuration)
        {
            _context = context;
            _contextAccessor = contextAccessor;
            _configuration = configuration;
        }

        [HttpGet]
        [ActionName("teacher")]
        public ActionResult Teacher()
        {
            UserViewModel model = new UserViewModel();
            return View(model);
        }

        [HttpPost]
        [ActionName("teacher")]
        public async Task<JsonResult> Teacher(string searchText = "")
        {
            DataTablesResponse<Users> result = new DataTablesResponse<Users>();
            List<Users> list = await _context.Users.Where(x => (x.IsStudent ?? false) == false).ToListAsync();
            if (!string.IsNullOrWhiteSpace(searchText) && list.Count > 0)
            {
                list = list.Where(x => x.FirstName.ToLower() == searchText.ToLower() || x.LastName.ToLower() == searchText.ToLower() || x.Email.ToLower() == searchText.ToLower()).OrderBy(x => x.FirstName).ToList();
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
        [ActionName("manageteacher")]
        public async Task<ActionResult> ManageTeacher(string data)
        {
            UserViewModel model = new UserViewModel();
            int decryptedId = 0;

            if (!string.IsNullOrWhiteSpace(data))
            {
                decryptedId = EncryptionDecryption.GetDecrypt(data).ToInteger();
            }

            if (decryptedId > 0)
            {
                Users user = await _context.Users.FirstOrDefaultAsync(x => x.Id == decryptedId && (x.IsStudent ?? false) == false);
                model.User = user;
                if (!string.IsNullOrEmpty(model.User.Password))
                {
                    model.User.Password = EncryptionDecryption.GetDecrypt(model.User.Password);
                    model.User.ConfirmPassword = model.User.Password;
                }
            }
            else
            {
                model.User.Active = true;
                model.User.IsStudent = false;
            }
            model.GenderList = new List<SelectListItem>()
                    {
                        new SelectListItem() { Text = "Male", Value = "1", Selected =  false },
                        new SelectListItem() { Text = "Female", Value = "2", Selected = false },
                        new SelectListItem() { Text = "Other", Value = "3", Selected = false }
                    };
            return this.View("ManageTeacher", model);
        }

        [HttpPost]
        [ActionName("manageteacher")]
        public async Task<ActionResult> ManageTeacher(UserViewModel userViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Users model = userViewModel.User;
                    bool isAlreadyExist = await _context.Users.AnyAsync(x => x.Email == model.Email && x.Id != model.Id);
                    if (isAlreadyExist)
                    {
                        return Json(new { success = false, message = string.Format("{0} already exists.", model.Email) }, new Newtonsoft.Json.JsonSerializerSettings());
                    }

                    var userIdClaim = _contextAccessor.HttpContext.User.FindFirst($"UserId");
                    var userId = Convert.ToInt32(userIdClaim == null ? "0" : userIdClaim.Value);

                    if (!string.IsNullOrEmpty(model.Password))
                    {
                        model.Password = EncryptionDecryption.GetEncrypt(model.Password);
                    }

                    if (model.Id > 0)
                    {
                        Users user = await _context.Users.FirstOrDefaultAsync(x => x.Id == model.Id);
                        user.FirstName = model.FirstName;
                        user.LastName = model.LastName;
                        user.Email = model.Email;
                        user.Password = model.Password;
                        user.GenderId = model.GenderId;
                        user.Active = model.Active;
                        user.IsStudent = false;
                        user.IsSuperAdmin = false;
                        user.ModifiedDate = DateTime.Now;
                        user.ModifiedBy = userId;
                        _context.Users.Update(user);
                        await _context.SaveChangesAsync();

                        this.TempData["SuccessMessage"] = string.Format(Messages.UpdateMessage, "Teacher details");
                        return this.Json(new { success = true, message = string.Format(Messages.UpdateMessage, "Teacher details") }, new Newtonsoft.Json.JsonSerializerSettings());
                    }
                    else
                    {
                        model.UserId = await RandomString(5);
                        model.IsSuperAdmin = false;
                        model.IsStudent = false;
                        model.CreatedDate = DateTime.Now;
                        model.CreatedBy = userId;
                        await _context.Users.AddAsync(model);
                        await _context.SaveChangesAsync();

                        this.TempData["SuccessMessage"] = string.Format(Messages.SaveSuccessMessage, "Teacher details");
                        return this.Json(new { success = true, message = string.Format(Messages.SaveSuccessMessage, "Teacher details") }, new Newtonsoft.Json.JsonSerializerSettings());

                    }
                }
                catch (Exception)
                {
                    return this.Json(new { success = false, message = string.Format(Messages.SaveErrorMessage, "Teacher details") }, new Newtonsoft.Json.JsonSerializerSettings());
                }
            }

            return this.Json(new { success = false, message = Messages.ModelIsNotValid }, new Newtonsoft.Json.JsonSerializerSettings());
        }

        [ActionName("deleteteacher")]
        public async Task<ActionResult> DeleteTeacher(string id)
        {
            try
            {
                int decryptedId = EncryptionDecryption.GetDecrypt(id).ToInteger();
                Users user = await _context.Users.FirstOrDefaultAsync(x => x.Id == decryptedId && (x.IsStudent ?? false) == false);
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return this.Json(new { success = false, message = string.Format(Messages.DeleteErrorMessage, "Teacher") }, new Newtonsoft.Json.JsonSerializerSettings());
            }
            return this.Json(new { success = true, message = string.Format(Messages.DeleteMessage, "Teacher") }, new Newtonsoft.Json.JsonSerializerSettings());
        }


        public async Task<string> RandomString(int length)
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            string userId = new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
            bool isAlreadyExistUserId = await _context.Users.AnyAsync(x => x.UserId == userId);
            if (isAlreadyExistUserId)
            {
                return await RandomString(length);
            }
            return userId;
        }
    }
}
