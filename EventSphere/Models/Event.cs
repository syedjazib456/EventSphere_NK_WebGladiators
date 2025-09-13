using System;
using System.ComponentModel.DataAnnotations;

namespace EventSphere.Models
{
    public class Event
    {
        [Key]
        public int EventId { get; set; }

        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        public string Category { get; set; }

        public DateTime Date { get; set; }

        public string Venue { get; set; }

        public string OrganizerId { get; set; }
        public ApplicationUser Organizer { get; set; }
    }
}
