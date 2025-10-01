                        using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HebergementManager.Api.Models
{
    public class TypeHebergement
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public required string Nom { get; set; }

        [Required]
        public required ICollection<Hebergement> Hebergement { get; set; }
    }
}
