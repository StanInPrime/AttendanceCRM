using AttendanceCRM.Helpers.Services;
using AttendanceCRM.Models;
using AttendanceCRM.Models.Custom;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.ComponentModel;
using System.Data;
using System.Net;
using System.Net.Http.Headers;

namespace AttendanceCRM.Controllers
{
    [Authorize(Roles = "SuperAdmin,Student,Teacher")]
    public class HomeController : Controller
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly AttendanceCRMContext _context;
        private readonly IConfiguration _configuration;

        public HomeController(AttendanceCRMContext context, IHttpContextAccessor contextAccessor, IConfiguration configuration)
        {
            _context = context;
            _contextAccessor = contextAccessor;
            _configuration = configuration;
        }

        /// <summary>
        /// The dashboard.
        /// </summary>
        /// <returns>The dashboard view.</returns>
        [Authorize]
        [ActionName("dashboard")]
        public async Task<IActionResult> Dashboard()
        {
            SearchViewModel model = new();
            return View(model);
        }
    }
}