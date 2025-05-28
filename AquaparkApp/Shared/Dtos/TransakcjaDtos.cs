// Plik: AquaparkApp.Shared/Dtos/TransakcjaDtos.cs
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AquaparkApp.Shared.Dtos
{
    public class PozycjaZamowieniaDto
    {
        [Required]
        public int OfertaId { get; set; }
        [Required]
        [Range(1, 100)] // Przykładowy zakres dla ilości
        public int Ilosc { get; set; }
        // Możesz tu dodać np. ZastosowanaZnizkaId, jeśli zniżki są per pozycja
    }

    public class UtworzTransakcjeRequestDto
    {
        [Required]
        public int KlientId { get; set; }

        [Required]
        [MinLength(1, ErrorMessage = "Koszyk nie może być pusty.")]
        public List<PozycjaZamowieniaDto> PozycjeZamowienia { get; set; } = new List<PozycjaZamowieniaDto>();

        public int? ZnizkaId { get; set; } // ID globalnej zniżki dla całego zamówienia (opcjonalnie)

        [Required]
        [StringLength(50)]
        public string MetodaPlatnosci { get; set; } = string.Empty; // Np. "Karta", "Gotówka"

        // Możesz dodać inne pola, np. kwotaOtrzymana (dla gotówki), uwagi itp.
        // Kwota całkowita będzie liczona na serwerze, aby uniknąć manipulacji.
    }

    public class UtworzTransakcjeResponseDto
    {
        public bool Sukces { get; set; }
        public string Wiadomosc { get; set; } = string.Empty;
        public int? PlatnoscId { get; set; }
        public List<int> UtworzoneProduktyZakupioneIds { get; set; } = new List<int>();
        // Możesz dodać więcej informacji zwrotnych, np. szczegóły płatności, bilety
    }
}