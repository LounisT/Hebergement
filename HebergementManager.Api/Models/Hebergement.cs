using System.ComponentModel.DataAnnotations.Schema;

namespace HebergementManager.Api.Models;

public class Hebergement
{
    public int Id { get; set; }
    public string Nom { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Adresse { get; set; } = string.Empty;
    public string Ville { get; set; } = string.Empty;
    public string CodePostal { get; set; } = string.Empty;

    [ForeignKey("TypeHebergementId")]
    public int TypeHebergementId { get; set; }
    public int CapaciteMax { get; set; }
    public decimal PrixParNuit { get; set; }
    public bool EstActif { get; set; } = true;
    public DateTime DateCreation { get; set; } = DateTime.Now;
    
    public TypeHebergement Type { get; set; }
    public List<Reservation> Reservations { get; set; } = new();
}