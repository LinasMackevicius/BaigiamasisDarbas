using Microsoft.EntityFrameworkCore;
using projektas.Models;

namespace projektas.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Define tables as DbSet properties
        public DbSet<User> Users { get; set; }
        public DbSet<Session> SessionsList { get; set; }
        public DbSet<Conversation> Conversations { get; set; }
        public DbSet<GeneralNote> GeneralNotes { get; set; }
    }
}
