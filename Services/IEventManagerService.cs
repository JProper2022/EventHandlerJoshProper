using EventHandlerJoshProper.Entities;

namespace EventHandlerJoshProper.Services
{
    public interface IEventManagerService
    {
        public List<Event> GetAllEvents();

        public Event GetEventById(int eventId);

        public int AddEvent(Event NewEvent);

        public int UpdateEvent(Event activeEvent);

        public void AddGuestToEvent(int eventId, Guest newGuest);

        public void SendEnrollmentEmails(int eventId, string hostName, string scheme);
    }
}
