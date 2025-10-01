using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace HebergementManager.Api.Models;

public class Hebergement
{
    public int Id { get; set; }
    public string Nom { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Adresse { get; set; } = string.Empty;
    public string Ville { get; set; } = string.Empty;
    public string CodePostal { get; set; } = string.Empty;
    public int CapaciteMax { get; set; }
    public decimal PrixParNuit { get; set; }
    public bool EstActif { get; set; } = true;
    public DateTime DateCreation { get; set; } = DateTime.Now;
    public int TypeHebergementId { get; set; }

    [JsonIgnore]
    public virtual TypeHebergement? Type { get; set; }

    [JsonIgnore]
    public virtual List<Reservation> Reservations { get; set; } = new();

    // Relation many-to-many avec Equipements via la table de liaison
    [JsonIgnore]
    public virtual List<HebergementEquipement> HebergementEquipements { get; set; } = new();
}