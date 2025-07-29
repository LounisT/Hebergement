using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HebergementManager.Api.Models
{
    public class Equipements
    {
        [Key]
        public int Id { get; set; }

        public string Nom { get; set; }

        [ForeignKey("CategorieEquipementId")]
        public int CategorieEquipementId { get; set; }

        public CategorieEquipement Categorie { get; set; }
    }
}
