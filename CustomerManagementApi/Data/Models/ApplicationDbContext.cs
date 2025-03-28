using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CustomerManagementApi.Data.Models;

public partial class ApplicationDbContext : IdentityDbContext<User>
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Tag> Tags { get; set; }

    public virtual DbSet<CustomerTags> CustomerTags { get; set; }

    public DbSet<Project> Projects { get; set; }

    public DbSet<Payment> Payments { get; set; }

    public DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=CustomerManagement;Trusted_Connection=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Customer__3214EC07FCD89499");

            entity.HasIndex(e => e.Email, "UQ__Customer__A9D1053464E75019").IsUnique();

            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.FirstName).HasMaxLength(100);
            entity.Property(e => e.LastName).HasMaxLength(100);
            entity.Property(e => e.Phone).HasMaxLength(15);

            // Relazione con CustomerTags
            entity.HasMany(e => e.CustomerTags)
                .WithOne()
                .HasForeignKey(ct => ct.CustomerId);
        });

        modelBuilder.Entity<Tag>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Tags__3214EC0726D99631");

            entity.Property(e => e.Name).HasMaxLength(100);
        });

        // Configurazione di CustomerTags
        modelBuilder.Entity<CustomerTags>(entity =>
        {
            entity.HasKey(e => new { e.CustomerId, e.TagId }).HasName("PK_CustomerTags");

            entity.HasOne(e => e.Customer)
                .WithMany(c => c.CustomerTags)
                .HasForeignKey(e => e.CustomerId)
                .HasConstraintName("FK_CustomerTags_Customers");

            entity.HasOne(e => e.Tag)
                .WithMany(t => t.CustomerTags)
                .HasForeignKey(e => e.TagId)
                .HasConstraintName("FK_CustomerTags_Tags");
        });

        // Configurazioni aggiuntive se necessarie, come le relazioni
        modelBuilder.Entity<Project>()
            .HasOne(p => p.Customer)
            .WithMany(c => c.Projects)
            .HasForeignKey(p => p.CustomerId);

        modelBuilder.Entity<Payment>()
            .HasOne(p => p.Project)
            .WithMany(pr => pr.Payments)
            .HasForeignKey(p => p.ProjectId);

        // Configurazione della chiave primaria per IdentityUserLogin
        modelBuilder.Entity<IdentityUserLogin<string>>()
            .HasKey(login => login.UserId);

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
