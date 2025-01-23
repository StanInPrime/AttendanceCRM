using AttendanceCRM.Helpers.Constants;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AttendanceCRM.Models
{
    public class Courses
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        [RegularExpression(RegularExpressions.HtmlTag, ErrorMessage = "Invalid Input.")]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Subject")]
        public int? CourseSubjectId { get; set; }

        [ForeignKey("CourseSubjectId")]
        public CourseSubject? CourseSubject { get; set; }

        [Required]
        [Display(Name = "Semester")]
        public int? CourseSemesterId { get; set; }

        [ForeignKey("CourseSemesterId")]
        public CourseSemester? CourseSemester { get; set; }

        [ForeignKey("CourseYearId")]
        public CourseYear? CourseYear { get; set; }

        [Required]
        [Display(Name = "Teacher")]
        public int? TeacherId { get; set; }

        public string? CourseCode { get; set; }

        [Required]
        [Display(Name = "Year")]
        public int? CourseYearId { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? ModifiedDate { get; set; }

        [NotMapped]
        [Display(Name = "Students")]
        public List<string>? SelectedStudents { get; set; }
        public int? ModifiedBy { get; set; }

        public bool? Active { get; set; }

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
