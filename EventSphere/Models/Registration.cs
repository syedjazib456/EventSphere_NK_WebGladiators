using System;
using System.ComponentModel.DataAnnotations;

namespace EventSphere.Models
{
    public enum RegistrationStatus { Confirmed, Cancelled, Waitlist }

    public class Registration
    {
        [Key]
        public int RegistrationId { get; set; }

        public int EventId { get; set; }
        public Event Event { get; set; }

        public string StudentId { get; set; }   // Identity User Id
        public ApplicationUser Student { get; set; }

        public DateTime RegisteredOn { get; set; } = DateTime.Now;

        public RegistrationStatus Status { get; set; } = RegistrationStatus.Confirmed;

        // QR Code (unique per registration)
        public string? QrCodeValue { get; set; }
    }
}
