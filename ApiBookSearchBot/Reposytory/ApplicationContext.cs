using ApiBookSearchBot.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiBookSearchBot
{
    
    public class ApplicationContext : DbContext
    {
        public DbSet<TelegramUser> Users { get; set; } = null!;
        public DbSet<UserBook> Books { get; set; } = null!;    
        public ApplicationContext()
        {
            Database.EnsureCreatedAsync();  
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=repos.db");
        }
    }
}
