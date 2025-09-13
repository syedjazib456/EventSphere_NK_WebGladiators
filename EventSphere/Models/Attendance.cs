using System;
using System.ComponentModel.DataAnnotations;

namespace EventSphere.Models
{
    public class Attendance
    {
        [Key]
        public int AttendanceId { get; set; }

        public int EventId { get; set; }
        public Event Event { get; set; }

        public string StudentId { get; set; }  // FK → ApplicationUser
        public ApplicationUser Student { get; set; }

        public bool Attended { get; set; } = false;

        public DateTime MarkedOn { get; set; } = DateTime.Now;
    }
}
