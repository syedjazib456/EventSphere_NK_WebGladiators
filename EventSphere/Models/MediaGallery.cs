using System;
using System.ComponentModel.DataAnnotations;

namespace EventSphere.Models
{
    public class MediaGallery
    {
        [Key]
        public int MediaId { get; set; }

        // Event Reference
        [Required]
        public int EventId { get; set; }
        public Event Event { get; set; }

        // File details
        [Required]
        [MaxLength(10)]
        public string FileType { get; set; }   // "image" or "video"

        [Required]
        [MaxLength(255)]
        public string FileUrl { get; set; }    // Path or URL of file

        // Optional caption
        [MaxLength(150)]
        public string Caption { get; set; }

        // Who uploaded
        public string UploadedBy { get; set; }   // FK → ApplicationUser.Id
        public ApplicationUser Uploader { get; set; }

        // Upload date
        public DateTime UploadedOn { get; set; } = DateTime.Now;
    }
}
