using Microsoft.EntityFrameworkCore;
using HebergementManager.Api.Models;

namespace HebergementManager.Api.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Hebergement> Hebergements { get; set; }
    public DbSet<Reservation> Reservations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Hebergement>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Nom).IsRequired().HasMaxLength(200);
            entity.Property(e => e.PrixParNuit).HasColumnType("decimal(18,2)");
            entity.Property(e => e.Type).HasConversion<string>();
            
            entity.HasMany(e => e.Reservations)
                  .WithOne(r => r.Hebergement)
                  .HasForeignKey(r => r.HebergementId);
        });

        modelBuilder.Entity<Reservation>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.PrixTotal).HasColumnType("decimal(18,2)");
            entity.Property(e => e.Statut).HasConversion<string>();
        });

        // Données de test
       
    }
}