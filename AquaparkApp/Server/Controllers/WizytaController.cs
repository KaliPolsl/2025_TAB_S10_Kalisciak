using AquaparkApp.Server.Data;
using AquaparkApp.Shared.Models; // Lub Shared.Dtos, jeśli zdecydujesz się na DTO
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace AquaparkApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WizytaController : ControllerBase
    {
        private readonly AppDbContext _context;

        public WizytaController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Wizyta/aktywne/liczba
        [HttpGet("aktywne/liczba")]
        public async Task<ActionResult<int>> GetLiczbaAktywnychWizyt()
        {
            // Zakładamy, że StatusWizyty dla 'Trwa' ma znane ID, np. 1
            // Lub pobierasz je dynamicznie:
            // var statusTrwa = await _context.StatusWizyties.FirstOrDefaultAsync(s => s.Nazwa == "Trwa");
            // if (statusTrwa == null) return StatusCode(500, "Status 'Trwa' nie znaleziony.");
            // int statusTrwaId = statusTrwa.Id;

            // Użyj ID statusu 'Trwa' (tutaj zahardkodowane jako 1 dla przykładu - ZMIEŃ JEŚLI TRZEBA)
            int statusTrwaId = 1;
            var liczbaAktywnych = await _context.Wizyta // Użyj poprawnej nazwy DbSet
                                             .CountAsync(w => w.StatusWizytyId == statusTrwaId);
            return Ok(liczbaAktywnych);
        }
    }
}