using Microsoft.EntityFrameworkCore;
using HebergementManager.Api.Models;

namespace HebergementManager.Api.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Hebergement> Hebergements { get; set; }
    public DbSet<TypeHebergement> TypeHebergements { get; set; }
    public DbSet<Reservation> Reservations { get; set; }
    public DbSet<Equipements> Equipements { get; set; }
    public DbSet<CategorieEquipement> CategorieEquipement { get; set; }
    public DbSet<HebergementEquipement> HebergementEquipements { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);

      // Configuration de la relation many-to-many
      modelBuilder.Entity<HebergementEquipement>()
          .HasKey(he => new { he.HebergementId, he.EquipementId });

      modelBuilder.Entity<HebergementEquipement>()
          .HasOne(he => he.Hebergement)
          .WithMany(h => h.HebergementEquipements)
          .HasForeignKey(he => he.HebergementId);

      modelBuilder.Entity<HebergementEquipement>()
          .HasOne(he => he.Equipement)
          .WithMany(e => e.HebergementEquipements)
          .HasForeignKey(he => he.EquipementId);

      modelBuilder.Entity<TypeHebergement>().HasData(
          new { Id = 1, Nom = "Appartement" },
          new { Id = 2, Nom = "Chambre" },
          new { Id = 3, Nom = "Maison" },
          new { Id = 4, Nom = "Villa" }
      );

      modelBuilder.Entity<CategorieEquipement>().HasData(
          new { Id = 1, Nom = "Confort & intérieur" },
          new { Id = 2, Nom = "Cuisine & repas" },
          new { Id = 3, Nom = "Services & commodités" },
          new { Id = 4, Nom = "Extérieur & loisirs" },
          new { Id = 5, Nom = "Famille & sécurité" }
      );
      
    modelBuilder.Entity<Equipements>().HasData(
        // Confort & intérieur
        new { Id = 1,  CategorieEquipementId = 1, Nom = "WiFi gratuit" },
        new { Id = 2,  CategorieEquipementId = 1, Nom = "Climatisation" },
        new { Id = 3,  CategorieEquipementId = 1, Nom = "Télévision" },
        new { Id = 4,  CategorieEquipementId = 1, Nom = "Chauffage" },
        new { Id = 5,  CategorieEquipementId = 1, Nom = "Machine à laver" },
        new { Id = 6,  CategorieEquipementId = 1, Nom = "Sèche-cheveux" },
        new { Id = 7,  CategorieEquipementId = 1, Nom = "Fer à repasser" },

        // Cuisine & repas
        new { Id = 8,  CategorieEquipementId = 2, Nom = "Cuisine équipée" },
        new { Id = 9,  CategorieEquipementId = 2, Nom = "Réfrigérateur" },
        new { Id = 10, CategorieEquipementId = 2, Nom = "Micro-ondes" },
        new { Id = 11, CategorieEquipementId = 2, Nom = "Lave-vaisselle" },
        new { Id = 12, CategorieEquipementId = 2, Nom = "Cafetière" },
        new { Id = 13, CategorieEquipementId = 2, Nom = "Ustensiles de cuisine" },

        // Services & commodités
        new { Id = 14, CategorieEquipementId = 3, Nom = "Parking gratuit" },
        new { Id = 15, CategorieEquipementId = 3, Nom = "Ascenseur" },
        new { Id = 16, CategorieEquipementId = 3, Nom = "Service de ménage" },

        // Extérieur & loisirs
        new { Id = 17, CategorieEquipementId = 4, Nom = "Balcon / Terrasse" },
        new { Id = 18, CategorieEquipementId = 4, Nom = "Jardin" },
        new { Id = 19, CategorieEquipementId = 4, Nom = "Piscine" },
        new { Id = 20, CategorieEquipementId = 4, Nom = "Salle de sport" },
        new { Id = 21, CategorieEquipementId = 4, Nom = "Barbecue" },

        // Famille & sécurité
        new { Id = 22, CategorieEquipementId = 5, Nom = "Lit bébé" },
        new { Id = 23, CategorieEquipementId = 5, Nom = "Chaise haute" },
        new { Id = 24, CategorieEquipementId = 5, Nom = "Détecteur de fumée" },
        new { Id = 25, CategorieEquipementId = 5, Nom = "Détecteur de monoxyde de carbone" },
        new { Id = 26, CategorieEquipementId = 5, Nom = "Trousse de premiers secours" }
    );

    modelBuilder.Entity<TypeReservation>().HasData(
        new { Id = 1, Nom = "En attente"},
        new { Id = 2, Nom = "Confirmée"},
        new { Id = 3, Nom = "Annulé"},
        new { Id = 4, Nom = "Terminée"}
        );
    }
}