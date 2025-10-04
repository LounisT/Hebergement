using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HebergementManager.Api.Data;
using HebergementManager.Api.Models;

namespace HebergementManager.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ReservationsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ReservationsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/reservations - API JSON
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Reservation>>> GetReservations()
    {
        return await _context.Reservations
            .Include(r => r.Hebergement)
            .ThenInclude(h => h.Type)
            .ToListAsync();
    }
}