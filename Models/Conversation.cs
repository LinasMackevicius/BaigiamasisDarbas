using System;
using System.ComponentModel.DataAnnotations;

namespace projektas.Models
{
    public class Conversation
    {
        [Key]
        public int Id { get; set; }

        [StringLength(100)]
        public string PersonName { get; set; }

        public int Duration { get; set; } // Duration in minutes

        [Range(1, 5)]
        public int SuccessRating { get; set; } // 1 - Unsuccessful, 5 - Very Successful

        public string? Comment { get; set; }

        public int SessionId { get; set; }
        public Session Session { get; set; } // Foreign key relationship to Session

        // New fields
        public DateTime Date { get; set; } // Date of conversation
        public int UserId { get; set; } // User ID who owns the conversation
    }
}
