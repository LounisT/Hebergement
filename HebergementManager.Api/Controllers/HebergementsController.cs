using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HebergementManager.Api.Data;
using HebergementManager.Api.Models;

namespace HebergementManager.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HebergementsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public HebergementsController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<object>>> GetHebergements()
    {
        var hebergements = await _context.Hebergements
            .Include(h => h.HebergementEquipements)
            .ToListAsync();

        var result = hebergements.Select(h => new
        {
            h.Id,
            h.Nom,
            h.Description,
            h.Adresse,
            h.Ville,
            h.CodePostal,
            h.CapaciteMax,
            h.PrixParNuit,
            h.EstActif,
            h.TypeHebergementId,
            h.DateCreation,
            HebergementEquipements = h.HebergementEquipements.Select(he => new
            {
                he.EquipementId
            }).ToList()
        });

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<object>> GetHebergement(int id)
    {
        var hebergement = await _context.Hebergements
            .Include(h => h.HebergementEquipements)
            .FirstOrDefaultAsync(h => h.Id == id);

        if (hebergement == null)
            return NotFound();

        var result = new
        {
            hebergement.Id,
            hebergement.Nom,
            hebergement.Description,
            hebergement.Adresse,
            hebergement.Ville,
            hebergement.CodePostal,
            hebergement.CapaciteMax,
            hebergement.PrixParNuit,
            hebergement.EstActif,
            hebergement.TypeHebergementId,
            hebergement.DateCreation,
            HebergementEquipements = hebergement.HebergementEquipements.Select(he => new
            {
                he.EquipementId
            }).ToList()
        };

        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<Hebergement>> PostHebergement([FromBody] HebergementDto dto)
    {
        var hebergement = new Hebergement
        {
            Nom = dto.Nom,
            Description = dto.Description,
            Adresse = dto.Adresse,
            Ville = dto.Ville,
            CodePostal = dto.CodePostal,
            CapaciteMax = dto.CapaciteMax,
            PrixParNuit = dto.PrixParNuit,
            EstActif = dto.EstActif,
            TypeHebergementId = dto.TypeHebergementId,
            DateCreation = dto.DateCreation
        };

        if (dto.HebergementEquipements != null)
        {
            foreach (var equipDto in dto.HebergementEquipements)
            {
                hebergement.HebergementEquipements.Add(new HebergementEquipement
                {
                    EquipementId = equipDto.EquipementId
                });
            }
        }

        _context.Hebergements.Add(hebergement);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetHebergement), new { id = hebergement.Id }, hebergement);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutHebergement(int id, [FromBody] HebergementDto dto)
    {
        if (id != dto.Id)
            return BadRequest();

        var existingHebergement = await _context.Hebergements
            .Include(h => h.HebergementEquipements)
            .FirstOrDefaultAsync(h => h.Id == id);

        if (existingHebergement == null)
            return NotFound();

        // Mettre à jour les propriétés simples
        existingHebergement.Nom = dto.Nom;
        existingHebergement.Description = dto.Description;
        existingHebergement.Adresse = dto.Adresse;
        existingHebergement.Ville = dto.Ville;
        existingHebergement.CodePostal = dto.CodePostal;
        existingHebergement.CapaciteMax = dto.CapaciteMax;
        existingHebergement.PrixParNuit = dto.PrixParNuit;
        existingHebergement.EstActif = dto.EstActif;
        existingHebergement.TypeHebergementId = dto.TypeHebergementId;

        // Mettre à jour les équipements
        existingHebergement.HebergementEquipements.Clear();
        if (dto.HebergementEquipements != null)
        {
            foreach (var equipDto in dto.HebergementEquipements)
            {
                existingHebergement.HebergementEquipements.Add(new HebergementEquipement
                {
                    HebergementId = id,
                    EquipementId = equipDto.EquipementId
                });
            }
        }

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!HebergementExists(id))
                return NotFound();
            throw;
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteHebergement(int id)
    {
        var hebergement = await _context.Hebergements.FindAsync(id);
        if (hebergement == null)
            return NotFound();

        _context.Hebergements.Remove(hebergement);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<Hebergement>>> SearchHebergements(
        [FromQuery] string? ville,
        [FromQuery] string? type,
        [FromQuery] int? capaciteMin,
        [FromQuery] decimal? prixMax)
    {
        var query = _context.Hebergements.AsQueryable();

        if (!string.IsNullOrEmpty(ville))
            query = query.Where(h => h.Ville.Contains(ville));

        if (!string.IsNullOrEmpty(type))
            query = query.Where(h => h.Type.ToString() == type);

        if (capaciteMin.HasValue)
            query = query.Where(h => h.CapaciteMax >= capaciteMin.Value);

        if (prixMax.HasValue)
            query = query.Where(h => h.PrixParNuit <= prixMax.Value);

        return await query.ToListAsync();
    }

    [HttpGet("types")]
    public async Task<ActionResult<IEnumerable<TypeHebergement>>> GetTypeHebergements()
    {
        return await _context.TypeHebergements.ToListAsync();
    }

    private bool HebergementExists(int id)
    {
        return _context.Hebergements.Any(e => e.Id == id);
    }
}

public class HebergementDto
{
    public int Id { get; set; }
    public string Nom { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Adresse { get; set; } = string.Empty;
    public string Ville { get; set; } = string.Empty;
    public string CodePostal { get; set; } = string.Empty;
    public int CapaciteMax { get; set; }
    public decimal PrixParNuit { get; set; }
    public bool EstActif { get; set; }
    public DateTime DateCreation { get; set; }
    public int TypeHebergementId { get; set; }
    public List<HebergementEquipementDto>? HebergementEquipements { get; set; }
}

public class HebergementEquipementDto
{
    public int EquipementId { get; set; }
}