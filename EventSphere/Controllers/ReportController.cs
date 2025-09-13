using Microsoft.AspNetCore.Mvc;
using EventSphere.Data;

namespace EventSphere.Controllers
{
    public class ReportsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReportsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var reportData = _context.Events
                .Select(e => new
                {
                    e.Title,
                    e.Date,
                    TotalRegistered = _context.Registrations.Count(r => r.EventId == e.EventId),
                    TotalPresent = _context.Attendance.Count(a => a.EventId == e.EventId && a.Attended)
                })
                .ToList();

            return View(reportData);
        }
    }
}
