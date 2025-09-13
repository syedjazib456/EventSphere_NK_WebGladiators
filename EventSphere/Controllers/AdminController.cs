using Microsoft.AspNetCore.Mvc;

namespace EventSphere.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
