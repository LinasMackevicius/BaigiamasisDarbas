using System.ComponentModel.DataAnnotations;

namespace projektas.Models
{
    public class GeneralNote
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Content { get; set; } = string.Empty;

        public int UserId { get; set; }
    }
}



