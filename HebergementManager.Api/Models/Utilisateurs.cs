using System.ComponentModel.DataAnnotations;

namespace HebergementManager.Api.Models;

public class Utilisateurs
{
    [Key]
    public int Id { get; set; }
    
    public string Nom { get; set; }
    
    public string Prenom { get; set; }
    
    public string Email { get; set; }
    
    public string Password { get; set; }
    
    public int RoleId { get; set; }

    public virtual Role Role { get; set; }
}