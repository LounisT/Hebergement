namespace HebergementManager.Api.Models;

public class Hebergement
{
    public int Id { get; set; }
    public string Nom { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Adresse { get; set; } = string.Empty;
    public string Ville { get; set; } = string.Empty;
    public string CodePostal { get; set; } = string.Empty;
    public TypeHebergement Type { get; set; }
    public int CapaciteMax { get; set; }
    public decimal PrixParNuit { get; set; }
    public bool EstActif { get; set; } = true;
    public DateTime DateCreation { get; set; } = DateTime.Now;
    
    public List<Reservation> Reservations { get; set; } = new();
}

public enum TypeHebergement
{
    Appartement,
    Maison,
    Villa,
    Studio,
    Chambre
}