using System;
using System.ComponentModel.DataAnnotations;

namespace EventSphere.Models
{
    public class Feedback
    {
        [Key]
        public int FeedbackId { get; set; }

        // Event Reference
        [Required]
        public int EventId { get; set; }
        public Event Event { get; set; }

        // Student Reference
        [Required]
        public string StudentId { get; set; }   // FK → ApplicationUser
        public ApplicationUser Student { get; set; }

        // Rating (1-5)
        [Range(1, 5)]
        public int Rating { get; set; }

        // Optional written feedback
        [MaxLength(1000)]
        public string Comments { get; set; }

        // Submission timestamp
        public DateTime SubmittedOn { get; set; } = DateTime.Now;
    }
}
