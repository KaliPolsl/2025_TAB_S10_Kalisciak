using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquaparkApp.Shared
{
    public class Atrakcja
    {
        public int Id { get; set; }
        public string Nazwa { get; set; }
        public string? Opis { get; set; }
        public int MaxOsób { get; set; }
        public bool WymagaDodatkowejOplaty { get; set; }
        public decimal? CenaDodatkowa { get; set; }
    }
}
