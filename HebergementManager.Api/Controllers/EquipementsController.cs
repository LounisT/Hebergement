using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HebergementManager.Api.Data;
using HebergementManager.Api.Models;

namespace HebergementManager.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EquipementsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public EquipementsController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategorieEquipementDto>>> GetEquipements()
    {
        var categories = await _context.CategorieEquipement
            .Include(c => c.Equipement)
            .OrderBy(c => c.Id)
            .ToListAsync();

        var result = categories.Select(c => new CategorieEquipementDto
        {
            CategorieId = c.Id,
            CategorieNom = c.Nom,
            Equipements = c.Equipement.Select(e => new EquipementDto
            {
                Id = e.Id,
                Nom = e.Nom
            }).ToList()
        });

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Equipements>> GetEquipement(int id)
    {
        var equipement = await _context.Equipements
            .Include(e => e.Categorie)
            .FirstOrDefaultAsync(e => e.Id == id);

        if (equipement == null)
            return NotFound();

        return equipement;
    }
}

// DTOs pour les équipements
public class CategorieEquipementDto
{
    public int CategorieId { get; set; }
    public string CategorieNom { get; set; } = string.Empty;
    public List<EquipementDto> Equipements { get; set; } = new();
}

public class EquipementDto
{
    public int Id { get; set; }
    public string Nom { get; set; } = string.Empty;
}
