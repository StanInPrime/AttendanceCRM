using AttendanceCRM.Controllers;
using AttendanceCRM.Helpers.Contracts;
using AttendanceCRM.Helpers.Services;
using AttendanceCRM.Models;
using AttendanceCRM.Models.Custom;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace AttendanceCRM.Test
{
    public class AccountControllerTest
    {
        private AttendanceCRMContext _db;
        private AccountController _account;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ICookieService _cookieService;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<AttendanceCRMContext>()
             .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
             .Options;
            _db = new AttendanceCRMContext(options);
            _account = new AccountController(_db, _contextAccessor, _cookieService);
        }

        [Test]
        public async Task LoginTest()
        {
            _db.Users.AddRange(new List<Users>() {
                new Users()
                {
                    FirstName = "Admin",
                    LastName = "Admin",
                    Email = "admin@admin.com",
                    Password="fpHbLXIIgVUUf3AAXX8dCA==",
                    GenderId = 1,
                    IsSuperAdmin = true,
                    IsStudent = false,
                    Active = true,
                    CreatedDate = DateTime.Now,
                    CreatedBy = 1,
                    ModifiedDate = DateTime.Now,
                    ModifiedBy = 1,
                    UserId = "ADMIN"
                },
                new Users()
                {
                    FirstName = "Student",
                    LastName = "Student",
                    Email = "student@admin.com",
                    Password="fpHbLXIIgVUUf3AAXX8dCA==",
                    GenderId = 1,
                    IsSuperAdmin = false,
                    IsStudent = true,
                    Active = true,
                    CreatedDate = DateTime.Now,
                    CreatedBy = 1,
                    ModifiedDate = DateTime.Now,
                    ModifiedBy = 1,
                    UserId = "CR2SI"
                },
                new Users()
                {
                    FirstName = "teacher",
                    LastName = "teacher",
                    Email = "teacher@admin.com",
                    Password="fpHbLXIIgVUUf3AAXX8dCA==",
                    GenderId = 2,
                    IsSuperAdmin = false,
                    IsStudent = false,
                    Active = true,
                    CreatedDate = DateTime.Now,
                    CreatedBy = 1,
                    ModifiedDate = DateTime.Now,
                    ModifiedBy = 1,
                    UserId = "VKDCB"
                }
            });
            _db.SaveChanges();

            string message = string.Empty;
            LoginModel model = new LoginModel()
            {
                Username = "teacher@admin.com",
                Password = "Admin@123"
            };

            var user = await _db.Users.FirstOrDefaultAsync(x => x.Email == model.Username && (x.Active ?? false) == true);

            Assert.IsInstanceOf<Users>(user);
            if (user != null)
            {
                if (user.Password == EncryptionDecryption.GetEncrypt(model.Password))
                {
                    message = "Success";
                }
            }
            Assert.AreEqual(message, "Success");
        }

        [Test]
        public async Task CheckInTest()
        {
            _db.Courses.AddRange(new List<Courses>() {
                new Courses()
                {
                    Name = "Test Course 1",
                    CourseCode = "42086",
                    CourseYearId = 1,
                    CourseSemesterId = 1,
                    CourseSubjectId = 1,
                    CreatedDate = DateTime.Now,
                    TeacherId = 1,
                    Active = true
                },
                new Courses()
                {
                    Name = "Test Course 2",
                    CourseCode = "81731",
                    CourseYearId = 1,
                    CourseSemesterId = 1,
                    CourseSubjectId = 1,
                    CreatedDate = DateTime.Now,
                    TeacherId = 1,
                    Active = true
                },
                new Courses()
                {
                    Name = "Test Course 3",
                    CourseCode = "15245",
                    CourseYearId = 1,
                    CourseSemesterId = 1,
                    CourseSubjectId = 1,
                    CreatedDate = DateTime.Now,
                    TeacherId = 1,
                    Active = true
                },
            });
            _db.SaveChanges();

            string message = string.Empty;
            Courses course = await _db.Courses.FirstOrDefaultAsync(x => x.CourseCode == "81731" && (x.Active ?? false) == true);
            Assert.IsInstanceOf<Courses>(course);
            if (course != null)
            {
                message = "Success";
            }
            Assert.AreEqual(message, "Success");
        }
    }
}