using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HebergementManager.Api.Models
{
    public class CategorieEquipement
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Nom { get; set; }

        public ICollection<Equipements> Equipement { get; set; }
    }
}
