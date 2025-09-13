using System;
using System.ComponentModel.DataAnnotations;

namespace EventSphere.Models
{
    public enum UserRole { Student, Participant, Organizer, Admin }

    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }  // Store hashed password

        public UserRole Role { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
