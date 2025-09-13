using EventSphere.Data;
using EventSphere.Models;
using Microsoft.AspNetCore.Mvc;

namespace EventSphere.Controllers
{
    public class ContactController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ContactController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Contact form
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Contact form submission
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Contact contact)
        {
            if (ModelState.IsValid)
            {
                contact.SubmittedOn = DateTime.Now;
                _context.Contacts.Add(contact);
                await _context.SaveChangesAsync();
                TempData["Message"] = "Thank you! Your message has been sent successfully.";
                return RedirectToAction("Create"); // Redirect to same page with success message
            }

            return View(contact);
        }
    }
}
