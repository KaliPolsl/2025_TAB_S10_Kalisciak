// Plik: AquaparkApp.Server/Controllers/TransakcjaController.cs
using AquaparkApp.Server.Data;
using AquaparkApp.Shared.Models;
using AquaparkApp.Shared.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AquaparkApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransakcjaController : ControllerBase
    {
        private readonly AppDbContext _context;
        // private readonly ILogger<TransakcjaController> _logger; // Opcjonalnie

        public TransakcjaController(AppDbContext context /*, ILogger<TransakcjaController> logger */)
        {
            _context = context;
            // _logger = logger;
        }

        [HttpPost("finalizuj")] // Endpoint: POST /api/Transakcja/finalizuj
        public async Task<ActionResult<UtworzTransakcjeResponseDto>> FinalizujTransakcje([FromBody] UtworzTransakcjeRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var klient = await _context.Klients.FindAsync(request.KlientId);
            if (klient == null)
            {
                return BadRequest(new UtworzTransakcjeResponseDto { Sukces = false, Wiadomosc = "Nie znaleziono klienta." });
            }

            Znizka? globalnaZnizka = null;
            if (request.ZnizkaId.HasValue)
            {
                globalnaZnizka = await _context.Znizkas.FirstOrDefaultAsync(z => z.Id == request.ZnizkaId.Value && z.CzyAktywna);
                if (globalnaZnizka == null)
                {
                    return BadRequest(new UtworzTransakcjeResponseDto { Sukces = false, Wiadomosc = "Nieprawidłowa lub nieaktywna zniżka globalna." });
                }
            }

            decimal sumaCalkowita = 0;
            var listaProduktowDoDodania = new List<ProduktZakupiony>();
            var listaPozycjiPlatnosci = new List<PozycjaPlatnosci>(); // Będzie powiązana z jedną płatnością
            var utworzoneProduktyIds = new List<int>();

            // Używamy using dla transakcji bazodanowej
            using var dbTransaction = await _context.Database.BeginTransactionAsync();

            try
            {
                foreach (var pozycjaZamowienia in request.PozycjeZamowienia)
                {
                    var oferta = await _context.OfertaCennikowas.FindAsync(pozycjaZamowienia.OfertaId);
                    if (oferta == null)
                    {
                        await dbTransaction.RollbackAsync();
                        return BadRequest(new UtworzTransakcjeResponseDto { Sukces = false, Wiadomosc = $"Nie znaleziono oferty o ID: {pozycjaZamowienia.OfertaId}." });
                    }

                    for (int i = 0; i < pozycjaZamowienia.Ilosc; i++)
                    {
                        decimal cenaPozycji = oferta.CenaPodstawowa;
                        int? zastosowanaZnizkaId = null;

                        // Na razie zakładamy tylko zniżkę globalną, można rozbudować o zniżki per produkt
                        if (globalnaZnizka != null)
                        {
                            zastosowanaZnizkaId = globalnaZnizka.Id;
                            if (globalnaZnizka.TypZnizki.Equals("Procentowa", StringComparison.OrdinalIgnoreCase))
                            {
                                cenaPozycji *= (1 - (globalnaZnizka.Wartosc / 100m));
                            }
                            else if (globalnaZnizka.TypZnizki.Equals("Kwotowa", StringComparison.OrdinalIgnoreCase))
                            {
                                cenaPozycji -= globalnaZnizka.Wartosc;
                                if (cenaPozycji < 0) cenaPozycji = 0; // Cena nie może być ujemna
                            }
                        }
                        cenaPozycji = Math.Round(cenaPozycji, 2); // Zaokrąglenie do 2 miejsc po przecinku
                        sumaCalkowita += cenaPozycji;

                        var produktZakupiony = new ProduktZakupiony
                        {
                            KlientId = request.KlientId,
                            OfertaId = oferta.Id,
                            ZnizkaId = zastosowanaZnizkaId,
                            CenaZakupu = cenaPozycji,
                            DataZakupu = DateTime.Now, // Użyj DateTime.UtcNow jeśli preferujesz UTC
                            Status = "Nowy", // Lub "Opłacony", jeśli płatność jest od razu
                            PozostaloWejsc = oferta.LiczbaWejsc, // Jeśli to karnet ilościowy
                            WaznyOd = null, // Może być ustawiane przy pierwszym użyciu lub od daty zakupu
                            WaznyDo = oferta.ObowiazujeDo ?? DateTime.Now.AddMonths(1) // Przykładowa logika ważności
                                                                                       // TODO: Lepsza logika dla WaznyOd/WaznyDo na podstawie typu oferty
                        };
                        listaProduktowDoDodania.Add(produktZakupiony);
                    }
                }

                // 1. Utwórz rekord Płatności
                var platnosc = new Platnosc
                {
                    KlientId = request.KlientId,
                    KwotaCalkowita = Math.Round(sumaCalkowita, 2),
                    DataPlatnosci = DateTime.Now,
                    MetodaPlatnosci = request.MetodaPlatnosci,
                    StatusPlatnosci = "Zakończona" // Zakładamy, że płatność jest od razu finalizowana
                };
                _context.Platnoscs.Add(platnosc); // Użyj poprawnej nazwy DbSet
                await _context.SaveChangesAsync(); // Zapisz płatność, aby uzyskać jej ID

                // 2. Utwórz rekordy ProduktZakupiony i PozycjaPlatnosci
                foreach (var produkt in listaProduktowDoDodania)
                {
                    _context.ProduktZakupionies.Add(produkt); // Użyj poprawnej nazwy DbSet
                    await _context.SaveChangesAsync(); // Zapisz produkt, aby uzyskać jego ID
                    utworzoneProduktyIds.Add(produkt.Id);

                    var pozycjaPlatnosci = new PozycjaPlatnosci
                    {
                        PlatnoscId = platnosc.Id,
                        ProduktZakupionyId = produkt.Id,
                        KaraId = null,
                        OpisPozycji = (await _context.OfertaCennikowas.FindAsync(produkt.OfertaId))?.NazwaOferty ?? "Nieznana oferta",
                        KwotaPozycji = produkt.CenaZakupu
                    };
                    _context.PozycjaPlatnoscis.Add(pozycjaPlatnosci); // Użyj poprawnej nazwy DbSet
                }
                await _context.SaveChangesAsync(); // Zapisz wszystkie pozycje płatności

                // TODO: Tutaj potencjalnie logika tworzenia Wizyta i przypisania Opaska,
                // jeśli zakupiony produkt od razu aktywuje wizytę.
                // To zależy od przepływu biznesowego (czy klient od razu wchodzi, czy kupuje na później).
                // Na razie pomijamy ten krok dla uproszczenia.

                await dbTransaction.CommitAsync(); // Zatwierdź transakcję bazodanową

                return Ok(new UtworzTransakcjeResponseDto
                {
                    Sukces = true,
                    Wiadomosc = "Transakcja została pomyślnie zakończona.",
                    PlatnoscId = platnosc.Id,
                    UtworzoneProduktyZakupioneIds = utworzoneProduktyIds
                });
            }
            catch (Exception ex)
            {
                await dbTransaction.RollbackAsync();
                // _logger?.LogError(ex, "Błąd podczas finalizacji transakcji dla klienta ID: {KlientId}", request.KlientId);
                Console.WriteLine($"Wyjątek (FinalizujTransakcje): {ex}");
                return StatusCode(500, new UtworzTransakcjeResponseDto { Sukces = false, Wiadomosc = "Wystąpił wewnętrzny błąd serwera podczas przetwarzania transakcji." });
            }
        }
    }
}