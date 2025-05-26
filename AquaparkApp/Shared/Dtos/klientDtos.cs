using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquaparkApp.Shared.Dtos
{
    // DTO dla podstawowych informacji o produkcie zakupionym (do listy na profilu klienta)
    public class ProduktZakupionySlimDto
    {
        public int Id { get; set; }
        public string? NazwaOferty { get; set; } // Z OfertaCennikowa
        public DateTime DataZakupu { get; set; }
        public decimal CenaZakupu { get; set; }
        public string? Status { get; set; }
        public int? PozostaloWejsc { get; set; }
    }

    // DTO dla podstawowych informacji o wizie (do listy na profilu klienta)
    public class WizytaSlimDto
    {
        public int Id { get; set; }
        public DateTime CzasWejscia { get; set; }
        public DateTime? CzasWyjscia { get; set; }
        public string? StatusWizytyNazwa { get; set; } // Z StatusWizyty
        public int OpaskaId { get; set; } // Lub numer opaski, jeśli chcesz go tu wyświetlić
    }

    // Główne DTO dla profilu klienta
    public class KlientProfileDto
    {
        public int Id { get; set; }
        public string? Imię { get; set; }
        public string? Nazwisko { get; set; }
        public string? NrTelefonu { get; set; }
        public string? Email { get; set; }

        public List<ProduktZakupionySlimDto> ProduktyZakupione { get; set; } = new List<ProduktZakupionySlimDto>();
        public List<WizytaSlimDto> Wizyty { get; set; } = new List<WizytaSlimDto>();

        // Możesz tu dodać inne kolekcje jako DTO, jeśli potrzebujesz, np. uproszczone płatności
        // public List<PlatnoscSlimDto> Platnosci { get; set; } = new List<PlatnoscSlimDto>();
    }

    // DTO dla listy klientów (używane w ManageClients.razor)
    public class KlientListItemDto
    {
        public int Id { get; set; }
        public string? Imię { get; set; }
        public string? Nazwisko { get; set; }
        public string? NrTelefonu { get; set; }
        public string? Email { get; set; }
    }
}