using AttendanceCRM.Helpers.Constants;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AttendanceCRM.Models
{
    public class Users
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        [RegularExpression(RegularExpressions.HtmlTag, ErrorMessage = "Invalid Input.")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(100)]
        [RegularExpression(RegularExpressions.HtmlTag, ErrorMessage = "Invalid Input.")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Email")]
        [RegularExpression(RegularExpressions.Email, ErrorMessage = "Invalid Input.")]
        public string Email { get; set; }

        [Required]
        [StringLength(50)]
        [DataType(DataType.Password)]
        [RegularExpression(RegularExpressions.Password, ErrorMessage = Messages.PasswordCriteria)]
        public string Password { get; set; }

        public string? UserId { get; set; }

        [Required]
        [Display(Name = "Gender")]
        public int? GenderId { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? ModifiedDate { get; set; }
        public int? ModifiedBy { get; set; }

        public bool? IsSuperAdmin { get; set; }
        public bool? IsStudent { get; set; }

        public bool? Active { get; set; }

        [NotMapped]
        [StringLength(50)]
        [DataType(DataType.Password)]
        [Compare("Password")]
        [Display(Name = "Confirm Password")]
        [RegularExpression(RegularExpressions.Password, ErrorMessage = "Password must contain at least one Uppercase, one Lowercase, one Numeric and one special character.")]
        public string? ConfirmPassword { get; set; }

        [NotMapped]
        public string EncryptedId
        {
            get
            {
                return AttendanceCRM.Helpers.Services.EncryptionDecryption.GetEncrypt(Id.ToString());
            }
        }
    }
}
