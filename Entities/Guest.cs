using System.ComponentModel.DataAnnotations;

namespace EventHandlerJoshProper.Entities
{
    public class Guest
    {
        public enum EnrollmentConfirmationStatus
        {
            ConfirmationMessageNotSent,
            ConfirmationMessageSent,
            EnrollmentConfirmed,
            EnrollmentDeclined
        }
        public int GuestId { get; set; }
       
        [Required(ErrorMessage = "Missing name.")]
        public string Name { get; set; }
        
        [Required(ErrorMessage = "Missing email address.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
       
        public string Email { get; set; }

        public EnrollmentConfirmationStatus  Status { get; set; } = EnrollmentConfirmationStatus.ConfirmationMessageNotSent;

        public int EventId { get; set; }

        public Event? Event { get; set; }
    }
}
