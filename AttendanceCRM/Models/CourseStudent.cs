using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AttendanceCRM.Models
{
    public class CourseStudent
    {
        public int Id { get; set; }
        [Required]
        public int CourseId { get; set;}
        [Required]
        public int StudentId { get; set; }
        [ForeignKey("StudentId")]
        public Users? Student { get; set; }
        [ForeignKey("CourseId")]
        public Courses? Course { get; set; }
    }
}
