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
            entity.Property(e => e.Nom)
                  .IsRequired()
                  .HasMaxLength(255);

            entity.Property(e => e.PrixParNuit)
                  .HasColumnType("decimal(18,2)");

            entity.HasOne(e => e.Type)
                  .WithMany(r => r.Hebergement)
                  .HasForeignKey(e => e.TypeHebergementId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasMany(e => e.Reservations)
                  .WithOne(r => r.Hebergement)
                  .HasForeignKey(r => r.HebergementId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<TypeHebergement>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Nom)
                  .IsRequired()
                  .HasMaxLength(255);

            entity.HasMany(e => e.Hebergement)
                  .WithOne(r => r.Type)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Reservation>(entity =>
        {
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.PrixTotal)
                  .HasColumnType("decimal(18,2)");
            
            entity.Property(e => e.Statut)
                  .HasConversion<string>();
        });

        modelBuilder.Entity<Equipements>(entity =>
        {
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.Nom)
                   .IsRequired()
                   .HasMaxLength(255);
            
            entity.HasOne(c => c.Categorie)
                  .WithMany(c => c.Equipement)
                  .HasForeignKey(c => c.CategorieEquipementId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<CategorieEquipement>(entity =>
        {
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.Nom)
                  .IsRequired()
                  .HasMaxLength(255);
            
            entity.HasMany(e => e.Equipement)
                  .WithOne(r => r.Categorie)
                  .OnDelete(DeleteBehavior.Restrict);
        });
       
    }
}