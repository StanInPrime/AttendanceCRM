using AttendanceCRM.Helpers.Constants;
using AttendanceCRM.Helpers.Contracts;
using AttendanceCRM.Helpers.Enums;
using AttendanceCRM.Helpers.Services;
using AttendanceCRM.Models;
using AttendanceCRM.Models.Custom;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace AttendanceCRM.Controllers
{
    public class AccountController : Controller
    {
        private readonly AttendanceCRMContext _context;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ICookieService _cookieService;

        public AccountController(AttendanceCRMContext context, IHttpContextAccessor contextAccessor, ICookieService cookieService)
        {
            _context = context;
            _contextAccessor = contextAccessor;
            _cookieService = cookieService;
        }

        /// <summary>
        /// The login method.
        /// </summary>
        /// <param name="returnUrl">The return url.</param>
        /// <returns>The login view.</returns>
        [HttpGet]
        [ActionName("login")]
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            LoginModel model = new()
            {
                ReturnUrl = returnUrl
            };

            var isRememberCookie = Request.Cookies[EnumHelper.GetDescription(CookieName.IsRemember)];
            var usernameCookie = Request.Cookies[EnumHelper.GetDescription(CookieName.UserName)];
            var passwordCookie = Request.Cookies[EnumHelper.GetDescription(CookieName.Password)];

            if (isRememberCookie != null && usernameCookie != null && passwordCookie != null)
            {
                model.RememberMe = ConvertTo.ToBoolean(EncryptionDecryption.GetDecrypt(isRememberCookie));

                if (model.RememberMe)
                {
                    model.Username = EncryptionDecryption.GetDecrypt(usernameCookie);
                    model.Password = EncryptionDecryption.GetDecrypt(passwordCookie);
                }
            }

            return View("login", model);
        }

        /// <summary>
        /// The post login method.
        /// </summary>
        /// <param name="model">The login model.</param>
        /// <returns>The login view.</returns>
        [AllowAnonymous]
        [HttpPost]
        [ActionName("login")]
        public async Task<IActionResult> Login(LoginModel model)
        {
            bool isSuccess = false;

            if (ModelState.IsValid)
            {
                var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == model.Username && (x.Active ?? false) == true);

                if (user == null)
                {
                    TempData["ErrorMessage"] = Messages.InvalidCredentials;
                }
                else
                {
                    if (user.Password == EncryptionDecryption.GetEncrypt(model.Password))
                    {
                        isSuccess = true;
                    }
                    else
                    {
                        TempData["ErrorMessage"] = Messages.InvalidCredentials;
                    }

                    if (isSuccess)
                    {
                        if (model.RememberMe)
                        {
                            _cookieService.AddCookie(EnumHelper.GetDescription(CookieName.UserName), model.Username);
                            _cookieService.AddCookie(EnumHelper.GetDescription(CookieName.Password), model.Password);
                            _cookieService.AddCookie(EnumHelper.GetDescription(CookieName.IsRemember), Convert.ToString(model.RememberMe));
                        }
                        else
                        {
                            _cookieService.RemoveCookie(EnumHelper.GetDescription(CookieName.UserName));
                            _cookieService.RemoveCookie(EnumHelper.GetDescription(CookieName.Password));
                            _cookieService.RemoveCookie(EnumHelper.GetDescription(CookieName.IsRemember));
                        }

                        string employeeShortNameF = !string.IsNullOrWhiteSpace(user.FirstName)
                                     ? user.FirstName.Substring(0, 1)
                                     : "A";
                        string employeeShortNameL = !string.IsNullOrWhiteSpace(user.LastName)
                                                 ? user.LastName.Substring(0, 1)
                                                 : "B";
                        string role = (user.IsSuperAdmin ?? false) ? "SuperAdmin" : ((user.IsStudent ?? false) ? "Student" : "Teacher");
                        var claims = new List<Claim>()
                        {
                            new Claim("Username", user.Email.ToString()),
                            new Claim("ShortName", employeeShortNameF + employeeShortNameL),
                            new Claim("FullName", user.FirstName + " " + user.LastName),
                            new Claim("UserId", user.Id.ToString()),
                            new Claim(ClaimTypes.Role, role)
                        };

                        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                        await _contextAccessor.HttpContext.SignInAsync(
                            CookieAuthenticationDefaults.AuthenticationScheme,
                            new ClaimsPrincipal(claimsIdentity),
                            new AuthenticationProperties());

                        if (!string.IsNullOrWhiteSpace(model.ReturnUrl))
                        {
                            return Redirect(model.ReturnUrl);
                        }

                        return RedirectToAction("dashboard", "home");
                    }
                }
            }

            return View("login", model);
        }

        /// <summary>
        /// The logout method.
        /// </summary>
        /// <returns>The login view.</returns>
        [Authorize(Roles = "SuperAdmin,Student,Teacher")]
        [HttpGet]
        [ActionName("logout")]
        public async Task<IActionResult> Logout()
        {
            await _contextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("login");
        }

        /// <summary>
        /// The change password method.
        /// </summary>
        /// <returns>The change password view.</returns>
        [Authorize(Roles = "SuperAdmin,Student,Teacher")]
        [HttpGet]
        [ActionName("changepassword")]       
        public ActionResult ChangePassword()
        {
            return View("changepassword");
        }

        /// <summary>
        /// The change password post method.
        /// </summary>
        /// <param name="model">The change password model.</param>
        /// <returns>The change password view.</returns>
        [Authorize(Roles = "SuperAdmin,Student,Teacher")]
        [HttpPost]
        [ActionName("changepassword")]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                var userIdClaim = _contextAccessor.HttpContext.User.FindFirst($"UserId");
                var userId = Convert.ToInt32(userIdClaim == null ? "0" : userIdClaim.Value);

                var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);

                if (user == null)
                {
                    TempData["ErrorMessage"] = Messages.GenericError;
                }
                else
                {
                    if (user.Password == EncryptionDecryption.GetEncrypt(model.OldPassword))
                    {
                        user.Password = EncryptionDecryption.GetEncrypt(model.NewPassword);
                        _context.Users.Update(user);
                        await _context.SaveChangesAsync();

                        TempData["SuccessMessage"] = Messages.PasswordChanged;

                        return RedirectToAction("dashboard", "home");
                    }
                    else
                    {
                        TempData["ErrorMessage"] = Messages.IncorrectOldPassword;
                    }
                }
            }

            return View("changepassword", model);
        }

        /// <summary>
        /// The error page.
        /// </summary>
        /// <returns>The error page.</returns>
        [AllowAnonymous]
        [HttpGet]
        [ActionName("error")]
        public ActionResult Error()
        {
            var userIdClaim = _contextAccessor.HttpContext.User.FindFirst($"UserId");
            ViewBag.UserId = Convert.ToInt32(userIdClaim == null ? "0" : userIdClaim.Value);

            return View("error");
        }
    }
}
