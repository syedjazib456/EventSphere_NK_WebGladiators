using System.ComponentModel.DataAnnotations;

namespace EventSphere.Models.ViewModels
{
    public class RegisterViewModel
    {
        [Required] public string FullName { get; set; }

        [Required, EmailAddress] public string Email { get; set; }

        [Required] public string Department { get; set; }

        [Required] public string Mobile { get; set; }

        public string EnrollmentNo { get; set; }

        [Required, DataType(DataType.Password)]
        public string Password { get; set; }

        [Compare("Password"), DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        [Required] public UserRole Role { get; set; }
    }
}
