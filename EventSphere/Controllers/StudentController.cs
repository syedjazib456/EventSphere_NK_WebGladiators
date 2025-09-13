using EventSphere.Data;
using EventSphere.Models;
using Microsoft.AspNetCore.Mvc;

public class StudentController : Controller
{
    private readonly ApplicationDbContext _context;

    public StudentController(ApplicationDbContext context)
    {
        _context = context;
    }

    // Load available events
    public IActionResult Dashboard()
    {
        var events = _context.Events.ToList();
        return View(events);
    }

  
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(int EventId)
    {
        var userId = User.Claims.First(c =>
            c.Type == "sub" ||
            c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;

        // Check if already registered
        var alreadyRegistered = _context.Registrations
            .Any(r => r.EventId == EventId && r.StudentId == userId);

        if (!alreadyRegistered)
        {
            var registration = new Registration
            {
                EventId = EventId,
                StudentId = userId,
               
            };

            _context.Registrations.Add(registration);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction("Dashboard");
    }
}
