using System.ComponentModel.DataAnnotations;

namespace projektas.Models
{
    public class User
    {
        [Key] // Marks this property as the primary key
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Username { get; set; }

        [Required]
        [StringLength(100)]
        public string Password { get; set; }
    }
}
