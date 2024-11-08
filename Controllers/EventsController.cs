using Microsoft.AspNetCore.Mvc;
using EventHandlerJoshProper.Entities;
using EventHandlerJoshProper.Models;
using EventHandlerJoshProper.Services;

namespace EventHandlerJoshProper.Controllers
{
    public class EventsController : Controller
    {
        private readonly IEventManagerService _eventManagerService;

        public EventsController(IEventManagerService eventManagerService)
        {
            _eventManagerService = eventManagerService;
        }

        [HttpGet("/Events/All")]
        public IActionResult AllEvents()
        {
            var viewModel = new AllEventsViewModel()
            {
                Events = _eventManagerService.GetAllEvents()
            };

            return View("All", viewModel);
        }

        [HttpGet("/Events/add")]
        public IActionResult AddEvent()
        {
            var viewModel = new EventViewModel()
            {
                Event = new Event()
            };

            return View("Add", viewModel);
        }

        [HttpPost("/Events/add")]
        public IActionResult AddEvent(EventViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                _eventManagerService.AddEvent(viewModel.Event);
                TempData["message"] = $"New event {viewModel.Event.Name} added successfully";
                TempData["className"] = "success";
                return RedirectToAction("All");
            }

            return View("Add", viewModel);
        }

        [HttpGet("/Events/{eventId}/edit")]
        public IActionResult EditEvent(int eventId)
        {
            var activeEvent = _eventManagerService.GetEventById(eventId);

            if (activeEvent == null)
            {
                return NotFound();
            }
            var viewModel = new EventViewModel()
            {
                Event = activeEvent
            };

            return View("Edit", viewModel);
        }

        [HttpGet("/Events/{eventId}/manage")]
        public IActionResult ManageEvent(int eventId)
        {
            var activeEvent = _eventManagerService.GetEventById(eventId);

            if (activeEvent == null)
            {
                return NotFound();
            }

            var viewModel = new ManageEventViewModel()
            {
                Event = activeEvent,
                Guest = new Guest(),
                CountConfirmationMessageNotSent = activeEvent.Guests.Count(g => g.Status == Guest.EnrollmentConfirmationStatus.ConfirmationMessageNotSent),
                CountConfirmationMessageSent = activeEvent.Guests.Count(g => g.Status == Guest.EnrollmentConfirmationStatus.ConfirmationMessageSent),
                CountEnrollmentConfirmed = activeEvent.Guests.Count(g => g.Status == Guest.EnrollmentConfirmationStatus.EnrollmentConfirmed),
                CountEnrollmentDeclined = activeEvent.Guests.Count(g => g.Status == Guest.EnrollmentConfirmationStatus.EnrollmentDeclined),
            };

            return View("Manage", viewModel);
        }

        [HttpPost("/Events/{eventId}/manage/add-guest")]
        public IActionResult AddGuest(int eventId, ManageEventViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                var activeEvent = _eventManagerService.GetEventById(eventId);

                if (activeEvent == null)
                {
                    return NotFound();
                }

                viewModel.Event = activeEvent;

                viewModel.CountConfirmationMessageNotSent = activeEvent.Guests.Count(g => g.Status == Guest.EnrollmentConfirmationStatus.ConfirmationMessageNotSent);
                viewModel.CountConfirmationMessageSent = activeEvent.Guests.Count(g => g.Status == Guest.EnrollmentConfirmationStatus.ConfirmationMessageSent);
                viewModel.CountEnrollmentConfirmed = activeEvent.Guests.Count(g => g.Status == Guest.EnrollmentConfirmationStatus.EnrollmentConfirmed);
                viewModel.CountEnrollmentDeclined = activeEvent.Guests.Count(g => g.Status == Guest.EnrollmentConfirmationStatus.EnrollmentDeclined);
            }

            _eventManagerService.AddGuestToEvent(eventId, viewModel.Guest);
            return RedirectToAction("ManageEvent", new { eventId });
        }

        [HttpPost("/Events/{eventId}/manage/add-guest")]
        public IActionResult EnrollGuest(int eventId)
        {
            var hostName = Request.Host.ToString();
            var scheme = Request.Scheme;

            _eventManagerService.SendEnrollmentEmails(eventId, hostName, scheme);
            return RedirectToAction("ManageEvent", new { eventId });
        }
    }
}
