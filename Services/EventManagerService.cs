using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Mail;
using EventHandlerJoshProper.Entities;
using static EventHandlerJoshProper.Entities.Guest;

namespace EventHandlerJoshProper.Services
{
	public class EventManagerService : IEventManagerService
	{
		public readonly EventManagerDbContext _dbContext;
		private readonly IConfiguration _configuration;

		public EventManagerService(EventManagerDbContext dbContext, IConfiguration configuration)
		{
			_dbContext = dbContext;
			_configuration = configuration;
		}

		public List<Event> GetAllEvents()
		{
			return _dbContext.Events.Include(e => e.Guests).OrderByDescending(e => e.Date).ToList();
		}

		public Event GetEventById(int eventId)
		{
			return _dbContext.Events.Include(e => e.Guests).FirstOrDefault(e => e.EventId == eventId);
		}

		public int AddEvent(Event NewEvent)
		{
			_dbContext.Events.Add(NewEvent);
			_dbContext.SaveChanges();

			return NewEvent.EventId;
		}

		public int UpdateEvent(Event activeEvent)
		{
			_dbContext.Events.Update(activeEvent);
			_dbContext.SaveChanges();

			return activeEvent.EventId;
		}

		public void AddGuestToEvent(int eventId, Guest newGuest)
		{
			var currentEvent = GetEventById(eventId);
			if (currentEvent == null) { return; }

			currentEvent.Guests.Add(newGuest);
			_dbContext.SaveChanges();
		}

		public void SendEnrollmentEmails(int eventId, string hostName, string scheme)
		{
			var currentEvent = GetEventById(eventId);
			if (currentEvent == null) { return; }

			var guests = currentEvent.Guests.Where(g => g.Status == Guest.EnrollmentConfirmationStatus.ConfirmationMessageSent).ToList();

			try
			{

				var smtpHost = _configuration["SmtpSettings:Host"];
				var smtpPort = _configuration["SmtpSettings:Port"];
				var smtpFromAddress = _configuration["SmtpSettings:FromAddress"];
				var smtpFromPassword = _configuration["SmtpSettings:FromPassword"];

				using var smtpClient = new SmtpClient(smtpHost);
				smtpClient.Port = string.IsNullOrEmpty(smtpPort) ? 587 : Convert.ToInt32(smtpPort);
				smtpClient.Credentials = new NetworkCredential(smtpFromAddress, smtpFromPassword);
				smtpClient.EnableSsl = true;

				foreach (var guest in guests)
				{
					var message = new MailMessage()
					{
						From = new MailAddress((smtpFromAddress)),
						Subject = $"[Action Required] {currentEvent.Name} enrollment",
						Body = @$"<h1>Hello {guest.Name}</h1> <p>:{scheme}://{hostName}/events/{eventId}/manage/{guest.GuestId}/enroll</p>",
						IsBodyHtml = true
					
					};

					//Send the email
					message.To.Add(new MailAddress(guest.Email));
					smtpClient.Send(message);
					guest.Status = EnrollmentConfirmationStatus.ConfirmationMessageSent;
					_dbContext.Update(guest);
					_dbContext.SaveChanges();
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}
	}
}
