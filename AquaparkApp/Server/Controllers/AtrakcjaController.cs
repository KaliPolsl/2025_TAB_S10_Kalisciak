// Plik: Server/Controllers/AtrakcjaController.cs
using AquaparkApp.Server.Data; // Dla AppDbContext
using AquaparkApp.Shared.Models; // Dla modelu Atrakcja (lub AquaparkApp.Shared.Models, jeśli tam są modele)
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AquaparkApp.Server.Controllers
{
    [Route("api/[controller]")] // Trasa będzie /api/Atrakcja
    [ApiController]
    public class AtrakcjaController : ControllerBase
    {
        private readonly AppDbContext _context;

        // Wstrzyknięcie AppDbContext przez konstruktor
        public AtrakcjaController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Atrakcja
        // Pobiera wszystkie atrakcje
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Atrakcja>>> GetAtrakcje()
        {
            // Upewnij się, że w AppDbContext.cs masz: public virtual DbSet<Atrakcja> Atrakcjas { get; set; }
            // Jeśli DbSet nazywa się inaczej (np. Atrakcja), zmień tutaj.
            var atrakcje = await _context.Atrakcjas.ToListAsync();
            return Ok(atrakcje); // Zwraca listę atrakcji z kodem 200 OK
        }

        // GET: api/Atrakcja/5
        // Pobiera atrakcję o konkretnym ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Atrakcja>> GetAtrakcja(int id)
        {
            var atrakcja = await _context.Atrakcjas.FindAsync(id);

            if (atrakcja == null)
            {
                return NotFound(); // Zwraca 404 Not Found, jeśli atrakcja nie istnieje
            }

            return Ok(atrakcja); // Zwraca znalezioną atrakcję z kodem 200 OK
        }

        // POST: api/Atrakcja
        // Tworzy nową atrakcję
        [HttpPost]
        public async Task<ActionResult<Atrakcja>> PostAtrakcja(Atrakcja atrakcja)
        {
            if (!ModelState.IsValid) // Podstawowa walidacja modelu
            {
                return BadRequest(ModelState);
            }

            _context.Atrakcjas.Add(atrakcja);
            await _context.SaveChangesAsync();

            // Zwraca odpowiedź 201 Created z lokalizacją nowo utworzonego zasobu i samym zasobem
            return CreatedAtAction(nameof(GetAtrakcja), new { id = atrakcja.Id }, atrakcja);
        }

        // PUT: api/Atrakcja/5
        // Aktualizuje istniejącą atrakcję
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAtrakcja(int id, Atrakcja atrakcja)
        {
            if (id != atrakcja.Id)
            {
                return BadRequest("ID w URL nie zgadza się z ID w ciele żądania.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Entry(atrakcja).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AtrakcjaExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw; // Rzuć wyjątek dalej, jeśli to inny błąd współbieżności
                }
            }

            return NoContent(); // Zwraca 204 No Content - sukces, brak treści do zwrócenia
        }

        // DELETE: api/Atrakcja/5
        // Usuwa atrakcję o konkretnym ID
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAtrakcja(int id)
        {
            var atrakcja = await _context.Atrakcjas.FindAsync(id);
            if (atrakcja == null)
            {
                return NotFound();
            }

            _context.Atrakcjas.Remove(atrakcja);
            await _context.SaveChangesAsync();

            return NoContent(); // Zwraca 204 No Content
        }

        // Prywatna metoda pomocnicza do sprawdzania, czy atrakcja istnieje
        private bool AtrakcjaExists(int id)
        {
            return _context.Atrakcjas.Any(e => e.Id == id);
        }
    }
}