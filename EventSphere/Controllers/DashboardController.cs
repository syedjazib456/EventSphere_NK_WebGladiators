using EventSphere.Data;
using EventSphere.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventSphere.Controllers
{

    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }
        [Authorize(Roles = "Student,Participant")]
        public IActionResult StudentDashboard()
        {
            var userId = User.Claims.First(c =>
                c.Type == "sub" ||
                c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;

            var events = _context.Events.ToList();

            var registeredEvents = _context.Registrations
                .Where(r => r.StudentId == userId)
                .Select(r => r.EventId)
                .ToList();

            ViewBag.RegisteredEvents = registeredEvents;

            return View(events);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(int EventId)
        {
            var userId = User.Claims.First(c =>
                c.Type == "sub" ||
                c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;

            
            var alreadyRegistered = _context.Registrations
                .Any(r => r.EventId == EventId && r.StudentId == userId);

            if (alreadyRegistered)
            {
                TempData["Message"] = "You are already registered for this event!";
                return RedirectToAction("StudentDashboard");
            }

            var registration = new Registration
            {
                EventId = EventId,
                StudentId = userId,
                RegisteredOn = DateTime.Now,
                Status = RegistrationStatus.Confirmed,
                QrCodeValue = Guid.NewGuid().ToString() 
            };

            _context.Registrations.Add(registration);
            await _context.SaveChangesAsync();

            TempData["Message"] = "Registered successfully!";
            return RedirectToAction("StudentDashboard");
        }

        [Authorize(Roles = "Organizer")]
        public async Task<IActionResult> OrganizerDashboard()
        {
            var userId = User.Claims.First(c =>
                c.Type == "sub" ||
                c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;

            string organizerId = userId; 

            var allEvents = await _context.Events
                .Where(e => e.OrganizerId == organizerId)
                .ToListAsync();

            var upcomingEvents = allEvents
                .Where(e => e.Date >= DateTime.Now)
                .OrderBy(e => e.Date)
                .Take(5)
                .ToList();

            var pastEvents = allEvents
                .Where(e => e.Date < DateTime.Now)
                .OrderByDescending(e => e.Date)
                .Take(5)
                .ToList();

            // Stats
            ViewBag.TotalEvents = allEvents.Count;
            ViewBag.UpcomingCount = upcomingEvents.Count;
            ViewBag.PastCount = pastEvents.Count;

            var attendanceStats = new List<object>();
            foreach (var ev in allEvents)
            {
                var registered = await _context.Registrations.CountAsync(r => r.EventId == ev.EventId);
                var attended = await _context.Attendance.CountAsync(a => a.EventId == ev.EventId);

                double percent = registered > 0 ? Math.Round((double)attended / registered * 100, 2) : 0;
                attendanceStats.Add(new { ev.Title, Percent = percent });
            }

            ViewBag.AttendanceStats = attendanceStats;

            ViewBag.UpcomingEvents = upcomingEvents;
            ViewBag.PastEvents = pastEvents;

            return View();
        }



        [Authorize(Roles = "Admin")]
        public IActionResult AdminDashboard()
        {
            return View();
        }
    }
}
