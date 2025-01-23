using System.ComponentModel.DataAnnotations.Schema;

namespace AttendanceCRM.Models
{
    public class Notification
    {
        public int Id { get; set; }
        public int? TeacherId { get; set; }
        public string? Message { get; set; }
        public bool? IsAcknowledged { get; set; }
        public DateTime? AddedOn { get; set; }

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
