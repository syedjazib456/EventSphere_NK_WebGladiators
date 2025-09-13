using EventSphere.Data;
using EventSphere.Models;
using EventSphere.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace EventSphere.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;
        public AccountController(UserManager<ApplicationUser> userManager,
                                 SignInManager<ApplicationUser> signInManager,
                                 RoleManager<IdentityRole> roleManager,
                                 ApplicationDbContext context
                                 )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _context = context;
        }

        // Register
        [HttpGet]
        public IActionResult Register() => View();
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    Role = model.Role
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    // Assign Role
                    await _userManager.AddToRoleAsync(user, model.Role.ToString());

                    // ✅ Save UserDetails
                    var details = new UserDetails
                    {
                        UserId = user.Id,
                        FullName = model.FullName,
                        Mobile = model.Mobile,
                        Department = model.Department,
                        EnrollmentNo = model.EnrollmentNo
                    };

                    _context.UserDetails.Add(details);
                    await _context.SaveChangesAsync();

                    await _signInManager.SignInAsync(user, isPersistent: false);

                    // Redirect to dashboard based on role
                    return model.Role switch
                    {
                        UserRole.Admin => RedirectToAction("AdminDashboard", "Dashboard"),
                        UserRole.Organizer => RedirectToAction("OrganizerDashboard", "Dashboard"),
                        _ => RedirectToAction("StudentDashboard", "Dashboard"),
                    };
                }

                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error.Description);
            }
            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> Login()
        {
            if (_signInManager.IsSignedIn(User))
            {
                var user = await _userManager.GetUserAsync(User);

                if (await _userManager.IsInRoleAsync(user, "Admin"))
                    return RedirectToAction("AdminDashboard", "Dashboard");
                if (await _userManager.IsInRoleAsync(user, "Organizer"))
                    return RedirectToAction("OrganizerDashboard", "Dashboard");
                return RedirectToAction("StudentDashboard", "Dashboard");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);

                    if (result.Succeeded)
                    {
                        // ✅ Role-based redirection
                        if (await _userManager.IsInRoleAsync(user, "Admin"))
                            return RedirectToAction("AdminDashboard", "Dashboard");

                        if (await _userManager.IsInRoleAsync(user, "Organizer"))
                            return RedirectToAction("OrganizerDashboard", "Dashboard");

                        if (await _userManager.IsInRoleAsync(user, "Participant") || await _userManager.IsInRoleAsync(user, "Student"))
                            return RedirectToAction("StudentDashboard", "Dashboard");

                        // Fallback → Home
                        return RedirectToAction("Index", "Home");
                    }
                }

                ModelState.AddModelError("", "Invalid login attempt");
            }
            return View(model);
        }


        // Logout
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult AccessDenied() => View();
    }
}
