using System;
using System.ComponentModel.DataAnnotations;

namespace EventSphere.Models
{
    public class Certificate
    {
        [Key]
        public int CertificateId { get; set; }

        // Event Reference
        [Required]
        public int EventId { get; set; }
        public Event Event { get; set; }

        // Student Reference
        [Required]
        public string StudentId { get; set; }   // FK → ApplicationUser
        public ApplicationUser Student { get; set; }

        // File/Path of Certificate
        [MaxLength(255)]
        public string CertificateUrl { get; set; }  // e.g. /certificates/Certificate_123.pdf

        // Issue Date
        public DateTime IssuedOn { get; set; } = DateTime.Now;

        // Status (Optional → if you want to track whether generated or pending)
        public bool IsIssued { get; set; } = true;
    }
}
