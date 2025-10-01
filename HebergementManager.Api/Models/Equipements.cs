using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace HebergementManager.Api.Models
{
    public class Equipements
    {
        [Key]
        public int Id { get; set; }

        public string Nom { get; set; } = string.Empty;

        [ForeignKey("CategorieEquipementId")]
        public int CategorieEquipementId { get; set; }

        [JsonIgnore]
        public virtual CategorieEquipement? Categorie { get; set; }

        // Relation many-to-many avec Hebergement via la table de liaison
        [JsonIgnore]
        public virtual List<HebergementEquipement> HebergementEquipements { get; set; } = new();
    }
}
