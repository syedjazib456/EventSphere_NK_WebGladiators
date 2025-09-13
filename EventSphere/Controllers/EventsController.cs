using EventSphere.Data;
using EventSphere.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventSphere.Controllers
{
    [Authorize(Roles = "Admin,Organizer")]
    public class EventsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EventsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var query = _context.Events.Include(e => e.Organizer).AsQueryable();

            if (User.IsInRole("Organizer"))
            {
                // get current logged-in organizer id
                var userId = User.Claims.First(c =>
                    c.Type == "sub" ||
                    c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;

                string organizerId = userId; // adjust type if OrganizerId is string
                query = query.Where(e => e.OrganizerId == organizerId);
            }

            var eventsList = await query.ToListAsync();
            return View(eventsList);
        }
        [AllowAnonymous]
        public async Task<IActionResult> PublicIndex()
        {
         
            var eventsList = await _context.Events
                .OrderBy(e => e.Date) 
                .ToListAsync();

            return View(eventsList);
        }
        // 📌 Event Details
        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var eventDetails = await _context.Events
                .Include(e => e.Organizer)
                .FirstOrDefaultAsync(e => e.EventId == id);

            if (eventDetails == null) return NotFound();
            return View(eventDetails);
        }

       
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Event model)
        {
            
              
                var userId = User.Claims.First(c =>
                    c.Type == "sub" ||
                    c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;

                model.OrganizerId = userId; 

                _context.Events.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
         

        }


        public async Task<IActionResult> Edit(int id)
        {
            var eventItem = await _context.Events.FindAsync(id);
            if (eventItem == null) return NotFound();

            if (User.IsInRole("Organizer") && eventItem.OrganizerId != GetCurrentUserId())
                return Forbid();

            return View(eventItem);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Event model)
        {
            if (ModelState.IsValid)
            {
                if (User.IsInRole("Organizer") && model.OrganizerId != GetCurrentUserId())
                    return Forbid();

                _context.Events.Update(model);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }



        public async Task<IActionResult> Delete(int id)
        {
            var eventItem = await _context.Events
                .Include(e => e.Organizer)
                .FirstOrDefaultAsync(e => e.EventId == id);

            if (eventItem == null) return NotFound();

            if (User.IsInRole("Organizer") && eventItem.OrganizerId != GetCurrentUserId())
                return Forbid();

            return View(eventItem);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var eventItem = await _context.Events.FindAsync(id);
            if (eventItem == null) return NotFound();

            if (User.IsInRole("Organizer") && eventItem.OrganizerId != GetCurrentUserId())
                return Forbid();

            _context.Events.Remove(eventItem);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private string GetCurrentUserId()
{
            return User.Claims.First(c =>
            c.Type == "sub" ||
            c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;
            // adjust type if OrganizerId is string
        }

    }
}
