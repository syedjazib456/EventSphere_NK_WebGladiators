using Microsoft.AspNetCore.Identity;

namespace EventSphere.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? FullName { get; set; }
        public string? Department { get; set; }
        public string? EnrollmentNo { get; set; }
        public UserRole Role { get; set; }
    }

   
}
