using System.Text.Json.Serialization;

namespace HebergementManager.Api.Models;

/// <summary>
/// Table de liaison pour la relation many-to-many entre Hebergement et Equipements
/// </summary>
public class HebergementEquipement
{
    public int HebergementId { get; set; }

    [JsonIgnore]
    public virtual Hebergement Hebergement { get; set; } = null!;

    public int EquipementId { get; set; }

    [JsonIgnore]
    public virtual Equipements Equipement { get; set; } = null!;
}
