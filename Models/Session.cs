﻿using projektas.Validators;
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
        public Session()
        {
            Conversations = new List<Conversation>(); // Initialize it
        }

        [Key]
        public int Id { get; set; }


        [Required]
        [DataType(DataType.Date)]
        public DateTime SessionDate { get; set; }

        [Required]
        public SessionType SessionType { get; set; }

        [Required]
        [TimeOnlyValidation(ErrorMessage = "Please enter a valid time.")]
        public TimeOnly TimeOfADayStart { get; set; }

        [Required]
        public TimeOnly TimeOfADayEnd { get; set; }


        [Required(ErrorMessage = "Place is required.")]
        [MinLength(3, ErrorMessage = "Place must be at least 3 characters.")]
        [StringLength(60, ErrorMessage = "Place cannot be longer than 60 characters.")]
        public string Place { get; set; } = string.Empty;

        
        [Required(ErrorMessage = "Goal is required.")]
        [MinLength(3, ErrorMessage = "Goal must be at least 3 characters.")]
        public string Goals { get; set; } = string.Empty;

        public int UserId { get; set; }
        public User? User { get; set; } // Foreign key relationship to User

        public List<Conversation> Conversations { get; set; } // Navigation property        
    }
}
