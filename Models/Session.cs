using projektas.Validators;
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
        [DataType(DataType.Date)]
        public DateTime SessionDate { get; set; }

        [Required(ErrorMessage = "Session type is required.")]
        public SessionType SessionType { get; set; }

        [Required]
        [TimeOnlyValidation]
        public TimeOnly TimeOfADayStart { get; set; }
        
        [Required]
        [TimeOnlyValidation]
        public TimeOnly TimeOfADayEnd { get; set; }

        [Required]
        public TimeSpan SessionDuration { get; set; }

        [Required(ErrorMessage = "Location is required.")]
        [StringLength(60, MinimumLength = 3, ErrorMessage = "Location must be between 3 and 60 characters.")]
        [RegularExpression(@"^[a-zA-Z0-9\s\-.,']+$", ErrorMessage = "Location can only contain letters, numbers, spaces, hyphens, commas, periods, and apostrophes.")]
        public string Place { get; set; } = string.Empty;


        [Required(ErrorMessage = "Goal is required.")]
        public string Goals { get; set; } = string.Empty;

        public int UserId { get; set; }
        public User? User { get; set; } // Foreign key relationship to User

        public List<Conversation> Conversations { get; set; } = new List<Conversation>();
        public List<InsightNote> InsightNotes { get; set; } = new List<InsightNote>();
    }
}
