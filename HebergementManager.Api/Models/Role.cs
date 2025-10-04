using System.ComponentModel.DataAnnotations;

namespace HebergementManager.Api.Models;

public class Role
{
    [Key]
    public int Id { get; set; }
    
    public string Nom { get; set; }
    
    public required ICollection<Utilisateurs> Utilisateurs { get; set; }
}