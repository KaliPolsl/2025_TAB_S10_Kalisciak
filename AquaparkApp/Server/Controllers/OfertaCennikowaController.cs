// Plik: AquaparkApp.Server/Controllers/OfertaCennikowaController.cs
using AquaparkApp.Server.Data;
using AquaparkApp.Shared.Models; // Upewnij się, że modele są tutaj
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AquaparkApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OfertaCennikowaController : ControllerBase
    {
        private readonly AppDbContext _context;

        public OfertaCennikowaController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/OfertaCennikowa
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OfertaCennikowa>>> GetOfertyCennikowe()
        {
            // W AppDbContext DbSet powinien nazywać się np. OfertaCennikowas lub OfertyCennikowe
            // Załóżmy, że nazywa się OfertaCennikowa (jak w Twoim poprzednim AppDbContext)
            // _context.OfertaCennikowa to DbSet z Twojego AppDbContext
            var oferty = await _context.OfertaCennikowas
                                    .OrderBy(o => o.Typ)
                                    .ThenBy(o => o.NazwaOferty)
                                    .ToListAsync();
            return Ok(oferty);
        }

        // Możesz dodać inne endpointy (GET by ID, POST, PUT, DELETE) później
    }
}