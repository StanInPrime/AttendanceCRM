using AttendanceCRM.Helpers.Constants;
using System.ComponentModel.DataAnnotations;

namespace AttendanceCRM.Models.Custom
{
    public class ChangePasswordModel
    {
        [Required]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "New Password")]
        [RegularExpression(RegularExpressions.Password)]
        public string NewPassword { get; set; }

        [StringLength(100)]
        [Compare("NewPassword")]
        [Display(Name = "Confirm New Password")]
        [RegularExpression(RegularExpressions.Password, ErrorMessage = "Password must contain at least one Uppercase, one Lowercase, one Numeric and one special character.")]
        public string ConfirmPassword { get; set; }
    }
}
