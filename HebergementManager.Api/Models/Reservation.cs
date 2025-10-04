namespace HebergementManager.Api.Models;

public class Reservation
{
    public int Id { get; set; }
    public string NomClient { get; set; } = string.Empty;
    public string EmailClient { get; set; } = string.Empty;
    public string TelephoneClient { get; set; } = string.Empty;
    public DateTime DateArrivee { get; set; }
    public DateTime DateDepart { get; set; }
    public int NombrePersonnes { get; set; }
    public decimal PrixTotal { get; set; }
    public StatutReservation Statut { get; set; } = StatutReservation.EnAttente;
    public DateTime DateReservation { get; set; } = DateTime.Now;
    
    public int HebergementId { get; set; }
    public virtual Hebergement Hebergement { get; set; } = null!;
}

public enum StatutReservation
{
    EnAttente,
    Confirmee,
    Annulee,
    Terminee
}