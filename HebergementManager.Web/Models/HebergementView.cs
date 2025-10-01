using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace HebergementManager.Web.Models;

public class HebergementViewModel
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "Le nom est obligatoire")]
    [StringLength(200, ErrorMessage = "Le nom ne peut pas dépasser 200 caractères")]
    [Display(Name = "Nom de l'hébergement")]
    public string Nom { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "La description est obligatoire")]
    [StringLength(1000, ErrorMessage = "La description ne peut pas dépasser 1000 caractères")]
    public string Description { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "L'adresse est obligatoire")]
    [StringLength(300, ErrorMessage = "L'adresse ne peut pas dépasser 300 caractères")]
    public string Adresse { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "La ville est obligatoire")]
    [StringLength(100, ErrorMessage = "La ville ne peut pas dépasser 100 caractères")]
    public string Ville { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Le code postal est obligatoire")]
    [RegularExpression(@"^\d{5}$", ErrorMessage = "Le code postal doit contenir 5 chiffres")]
    [Display(Name = "Code postal")]
    public string CodePostal { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Le type d'hébergement est obligatoire")]
    [Display(Name = "Type d'hébergement")]
    public int TypeHebergementId { get; set; }

    [JsonIgnore]
    public string TypeNom { get; set; } = string.Empty;

    [Required(ErrorMessage = "La capacité maximale est obligatoire")]
    [Range(1, 50, ErrorMessage = "La capacité doit être entre 1 et 50 personnes")]
    [Display(Name = "Capacité maximale")]
    public int CapaciteMax { get; set; }
    
    [Required(ErrorMessage = "Le prix par nuit est obligatoire")]
    [Range(0.01, 10000, ErrorMessage = "Le prix doit être entre 0,01€ et 10 000€")]
    [Display(Name = "Prix par nuit (€)")]
    public decimal PrixParNuit { get; set; }
    
    public bool EstActif { get; set; } = true;
    
    public DateTime DateCreation { get; set; } = DateTime.Now;

    // Liste des IDs des équipements sélectionnés
    public List<int> EquipementIds { get; set; } = new();

    // Propriétés calculées pour l'affichage (ignorées pour la sérialisation)
    [JsonIgnore]
    public string AdresseComplete => $"{Adresse}, {CodePostal} {Ville}";

    [JsonIgnore]
    public string PrixFormate => PrixParNuit.ToString("C");
}