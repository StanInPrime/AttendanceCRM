using System.ComponentModel.DataAnnotations;

namespace AttendanceCRM.Models.Custom
{
    public class LoginModel
    {
        [StringLength(100)]
        [Required]
        public string Username { get; set; }

        [StringLength(100)]
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }

        public string? ReturnUrl { get; set; }
    }
}
