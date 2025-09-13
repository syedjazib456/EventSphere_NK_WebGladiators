using EventSphere.Data;
using EventSphere.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventSphere.Controllers
{
    [Authorize(Roles = "Organizer,Admin")]
    public class AttendanceController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AttendanceController(ApplicationDbContext context)
        {
            _context = context;
        }

      
        public IActionResult MarkAttendance(int eventId)
        {
            var attendanceList = _context.Attendance
                .Where(a => a.EventId == eventId)
                .Select(a => a.StudentId)
                .ToList();

            var registrations = _context.Registrations
                .Include(r => r.Student)
                .Where(r => r.EventId == eventId)
                .Select(r => new AttendanceViewModel
                {
                    StudentId = r.StudentId,
                    FullName = r.Student.FullName,
                    Email = r.Student.Email,
                    Attended = attendanceList.Contains(r.StudentId)
                })
                .ToList();

            ViewBag.EventId = eventId;
            return View(registrations);
        }

        [HttpPost]
        public IActionResult MarkPresent(int eventId, string studentId)
        {
            if (!_context.Attendance.Any(a => a.EventId == eventId && a.StudentId == studentId))
            {
                _context.Attendance.Add(new Attendance
                {
                    EventId = eventId,
                    StudentId = studentId,
                    Attended = true,
                    MarkedOn = DateTime.Now
                });
                _context.SaveChanges();
            }

            return RedirectToAction("MarkAttendance", new { eventId });
        }
    }
}
