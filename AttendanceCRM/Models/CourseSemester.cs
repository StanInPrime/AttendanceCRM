using System.ComponentModel.DataAnnotations;

namespace AttendanceCRM.Models
{
    public class CourseSemester
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }
    }
}
