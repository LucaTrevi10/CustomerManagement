using Microsoft.EntityFrameworkCore;
using ProjectService.Models;

namespace ProjectService.Data
{
    public class ProjectDbContext : DbContext
    {
        public ProjectDbContext(DbContextOptions<ProjectDbContext> options) : base(options)
        {            
        }

        public DbSet<Project> Projects { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Project>()
                .HasKey(p => p.ProjectId);

            modelBuilder.Entity<Project>()
                .Property(p => p.ProjectName)
                .IsRequired()
                .HasMaxLength(255);

            modelBuilder.Entity<Project>()
                .Property(p => p.StartDate)
                .IsRequired();

            modelBuilder.Entity<Project>()
                .Property(p => p.PaymentDueDate)
                .IsRequired();
        }
    }
}
