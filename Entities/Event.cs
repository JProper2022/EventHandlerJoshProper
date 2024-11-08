using System.ComponentModel.DataAnnotations;

namespace EventHandlerJoshProper.Entities
{
    public class Event
    {
        public int EventId { get; set; }

        [Required(ErrorMessage = "Event name missing.")]
        public string? Name { get; set; }
        [Required(ErrorMessage = "Host name missing.")]
        public string? Host { get; set; }
        [Required(ErrorMessage = "Event date missing")]
        public DateTime Date { get; set; }
        [Required(ErrorMessage = "Event location missing")]
        //[RegularExpression("", ErrorMessage = "Must be a single digit, a single capital letter, and two digits.")]
        public string? RoomNum { get; set; }

        public List<Guest>? Guests { get; set; }
    }
}
