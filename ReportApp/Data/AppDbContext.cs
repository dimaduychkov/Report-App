using Microsoft.EntityFrameworkCore;
using ReportApp.Models;

namespace ReportApp.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Item> Items => Set<Item>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(
                    "Server=localhost;Database=ReportDb;Trusted_Connection=True;TrustServerCertificate=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Item>(entity =>
            {
                entity.ToTable("Items");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.Batch)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.ParentBatch)
                    .HasMaxLength(50);

                entity.Property(e => e.Comment)
                    .HasMaxLength(500);

                // партия должна быть уникальной
                entity.HasIndex(e => e.Batch)
                    .IsUnique();
            });
        }
    }
}