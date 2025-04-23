using System.ComponentModel.DataAnnotations;

namespace projektas.Models
{
    public class InsightNote
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Content { get; set; } = string.Empty;

        public int UserId { get; set; }

        public int? SessionId { get; set; }
        public Session? Session { get; set; }
    }
}



