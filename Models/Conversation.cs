using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace projektas.Models
{
    public class Conversation
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Person name is required.")]
        [StringLength(100)]
        public string PersonName { get; set; }

        [Range(0, 240, ErrorMessage = "Duration must be between 0 and 240 minutes.")]
        public int Duration { get; set; }

        [Range(1, 5, ErrorMessage = "Success rating must be between 1 and 5.")]
        public int SuccessRating { get; set; }

        [StringLength(1000)]
        public string? Comment { get; set; }
        public int SessionId { get; set; }
        public Session Session { get; set; }
    }
}
