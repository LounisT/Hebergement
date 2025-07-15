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
    public async Task<ActionResult<IEnumerable<Hebergement>>> GetHebergements()
    {
        return await _context.Hebergements.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Hebergement>> GetHebergement(int id)
    {
        var hebergement = await _context.Hebergements
            .Include(h => h.Reservations)
            .FirstOrDefaultAsync(h => h.Id == id);

        if (hebergement == null)
            return NotFound();

        return hebergement;
    }

    [HttpPost]
    public async Task<ActionResult<Hebergement>> PostHebergement(Hebergement hebergement)
    {
        _context.Hebergements.Add(hebergement);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetHebergement), new { id = hebergement.Id }, hebergement);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutHebergement(int id, Hebergement hebergement)
    {
        if (id != hebergement.Id)
            return BadRequest();

        _context.Entry(hebergement).State = EntityState.Modified;

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

    private bool HebergementExists(int id)
    {
        return _context.Hebergements.Any(e => e.Id == id);
    }
}