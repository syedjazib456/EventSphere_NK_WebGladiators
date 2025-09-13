using EventSphere.Data;
using EventSphere.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QRCoder;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace EventSphere.Controllers
{
    [Authorize(Roles = "Student,Participant")]
    public class RegistrationController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public RegistrationController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        
        public async Task<IActionResult> Register(int eventId)
        {
            var user = await _userManager.GetUserAsync(User);

            if (_context.Registrations.Any(r => r.EventId == eventId && r.StudentId == user.Id))
            {
                TempData["Message"] = "You are already registered!";
                return RedirectToAction("Details", "Events", new { id = eventId });
            }

            // Create unique QR Code value
            string qrValue = $"{eventId}-{user.Id}-{Guid.NewGuid()}";

            var registration = new Registration
            {
                EventId = eventId,
                StudentId = user.Id,
                QrCodeValue = qrValue
            };

            _context.Registrations.Add(registration);
            await _context.SaveChangesAsync();

            TempData["Message"] = "Registered successfully!";
            return RedirectToAction("MyRegistrations");
        }


        public async Task<IActionResult> MyRegistrations()
        {
            var user = await _userManager.GetUserAsync(User);
            var regs = _context.Registrations.Where(r => r.StudentId == user.Id).ToList();
            return View(regs);
        }
      

    }
}
