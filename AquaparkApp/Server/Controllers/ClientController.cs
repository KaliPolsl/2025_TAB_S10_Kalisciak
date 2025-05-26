// Na górze KlientController.cs
using AquaparkApp.Shared.Dtos;
using AquaparkApp.Server.Data;
using AquaparkApp.Shared.Models; 
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AquaparkApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KlientController : ControllerBase
    {
        private readonly AppDbContext _context;

        public KlientController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Klient
        // Pozwala na wyszukiwanie klientów
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Klient>>> GetKlienci([FromQuery] string? searchTerm)
        {
            IQueryable<Klient> query = _context.Klients; // Użyj poprawnej nazwy DbSet

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = searchTerm.ToLower();
                query = query.Where(k =>
                    k.Nazwisko.ToLower().Contains(searchTerm) ||
                    k.Imię.ToLower().Contains(searchTerm) ||
                    (k.NrTelefonu != null && k.NrTelefonu.Contains(searchTerm)) ||
                    (k.Email != null && k.Email.ToLower().Contains(searchTerm))
                // Można też próbować parsować searchTerm jako int dla ID
                );
            }

            return await query.OrderBy(k => k.Nazwisko).ThenBy(k => k.Imię).ToListAsync();
        }

        // GET: api/Klient/5
        // ZMODYFIKOWANA METODA
        // GET: api/Klient/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Klient>> GetKlient(int id)
        {
            Console.WriteLine($"API - Pobieranie klienta o ID: {id}"); // Użyj ILogger, jeśli masz skonfigurowany
                                                                       // lub Console.WriteLine($"API - Pobieranie klienta o ID: {id}");

            var klient = await _context.Klients
                .Include(k => k.ProduktZakupionies)
                    .ThenInclude(pz => pz.Oferta)
                .Include(k => k.Wizyta)
                    .ThenInclude(w => w.StatusWizyty)
                .Include(k => k.Platnoscs)
                .FirstOrDefaultAsync(k => k.Id == id);

            if (klient == null)
            {
                Console.WriteLine($"API - Nie znaleziono klienta o ID: {id}");
                // Console.WriteLine($"API - Nie znaleziono klienta o ID: {id}");
                return NotFound();
            }

            Console.WriteLine($"API - Znaleziono klienta: {klient.Imię} {klient.Nazwisko}");
            // Console.WriteLine($"API - Znaleziono klienta: {klient.Imię} {klient.Nazwisko}");
            return Ok(klient);
        }

        // POST: api/Klient
        [HttpPost]
        public async Task<ActionResult<Klient>> PostKlient(Klient klient)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Sprawdzenie czy email już istnieje (jeśli email ma być unikalny)
            if (!string.IsNullOrEmpty(klient.Email) && await _context.Klients.AnyAsync(k => k.Email == klient.Email))
            {
                ModelState.AddModelError("Email", "Klient z tym adresem email już istnieje.");
                return BadRequest(ModelState);
            }

            _context.Klients.Add(klient); // Użyj poprawnej nazwy DbSet
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetKlient), new { id = klient.Id }, klient);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutKlient(int id, Klient klient)
        {
            if (id != klient.Id)
            {
                return BadRequest("ID w URL nie zgadza się z ID w ciele żądania.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Sprawdzenie unikalności email, jeśli inny klient już go używa
            if (!string.IsNullOrEmpty(klient.Email) &&
                await _context.Klients.AnyAsync(k => k.Email == klient.Email && k.Id != klient.Id))
            {
                ModelState.AddModelError("Email", "Inny klient z tym adresem email już istnieje.");
                return BadRequest(ModelState);
            }

            _context.Entry(klient).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!KlientExists(id)) // Użyj metody pomocniczej
                {
                    return NotFound();
                }
                else
                {
                    throw; // Rzuć wyjątek dalej, jeśli to inny błąd współbieżności
                }
            }
            catch (Exception ex)
            {
                // Logowanie błędu
                Console.WriteLine($"Błąd podczas aktualizacji klienta ID {id}: {ex.Message}");
                return StatusCode(500, "Wystąpił wewnętrzny błąd serwera podczas aktualizacji klienta.");
            }

            return NoContent(); // Zwraca 204 No Content - sukces, brak treści do zwrócenia
        }

        // Prywatna metoda pomocnicza do sprawdzania, czy klient istnieje
        private bool KlientExists(int id)
        {
            return _context.Klients.Any(e => e.Id == id); // Użyj poprawnej nazwy DbSet
        }
    }
}