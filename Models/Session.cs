using System;
using System.ComponentModel.DataAnnotations;

namespace projektas.Models
{
    public enum SessionType 
    {
        Day,
        Night
    }

    public class Session
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [StringLength(100)]
        public string Place { get; set; }

        public string Goals { get; set; }

        public int UserId { get; set; }
        public User User { get; set; } // Foreign key relationship to User

        public List<Conversation> Conversations { get; set; } // Navigation property

        [Required]
        public SessionType SessionType { get; set; } // Add this field
    }
}
