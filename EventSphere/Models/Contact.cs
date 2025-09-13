using System;
using System.ComponentModel.DataAnnotations;

namespace EventSphere.Models
{
    public class Contact
    {
        [Key]
        public int ContactId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [StringLength(150)]
        public string Subject { get; set; }

        [Required]
        public string Message { get; set; }

        public DateTime SubmittedOn { get; set; } = DateTime.Now;
    }
}
