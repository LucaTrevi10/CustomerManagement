using Microsoft.EntityFrameworkCore;
using PaymentService.Models;

namespace PaymentService.Data
{
    public class PaymentDbContext : DbContext
    {
        public PaymentDbContext(DbContextOptions<PaymentDbContext> options) : base(options)
        {
        }

        public DbSet<Payment> Payments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.HasKey(e => e.PaymentId);

                entity.Property(e => e.Invoice).HasMaxLength(255);
                entity.Property(e => e.PaymentAmount).HasColumnType("decimal(18, 2)");
                entity.Property(e => e.PaymentMethod).HasMaxLength(50);              
            });           
        }
    }
}
