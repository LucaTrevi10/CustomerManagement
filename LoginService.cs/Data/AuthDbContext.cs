using LoginService.Models;
using Microsoft.EntityFrameworkCore;

namespace LoginService.cs.Data
{
    public class AuthDbContext : DbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("Users", schema: "dbo");
        }
    }
}
