using EventHandlerJoshProper.Entities;

namespace EventHandlerJoshProper.Models
{
    public class ManageEventViewModel
    {
        public Event? Event { get; set; }

        public Guest? Guest { get; set; }

        public int CountConfirmationMessageNotSent { get; set; }
        public int CountConfirmationMessageSent { get; set; }
        public int CountEnrollmentConfirmed { get; set; }
        public int CountEnrollmentDeclined { get; set; }

    }
}
