using Microsoft.AspNetCore.Mvc;

namespace EventSphere.Controllers
{
    public class OrganizerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
