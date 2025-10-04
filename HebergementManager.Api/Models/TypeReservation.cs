using System.ComponentModel.DataAnnotations;

namespace HebergementManager.Api.Models;

public class TypeReservation
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public required string Nom { get; set; }
    
    public required ICollection<Reservation> Reservation { get; set; }
}