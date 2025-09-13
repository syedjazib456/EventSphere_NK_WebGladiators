using System;
using System.ComponentModel.DataAnnotations;

namespace EventSphere.Models
{
    public class UserDetails
    {
        [Key]
        public int DetailId { get; set; }

        // Link to ApplicationUser
        [Required]
        public string UserId { get; set; }   // FK → AspNetUsers (Identity)
        public ApplicationUser User { get; set; }

        [Required, MaxLength(100)]
        public string FullName { get; set; }

        [MaxLength(15)]
        public string Mobile { get; set; }

        [MaxLength(100)]
        public string Department { get; set; }

        [MaxLength(50)]
        public string EnrollmentNo { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
