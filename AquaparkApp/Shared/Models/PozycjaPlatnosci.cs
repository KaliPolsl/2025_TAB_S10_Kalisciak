using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AquaparkApp.Shared.Models;

[Table("PozycjaPlatnosci")]
[Index("PlatnoscId", Name = "IX_PozycjaPlatnosci_Platnosc")]
public partial class PozycjaPlatnosci
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("platnosc_id")]
    public int PlatnoscId { get; set; }

    [Column("produktZakupiony_id")]
    public int? ProduktZakupionyId { get; set; }

    [Column("kara_id")]
    public int? KaraId { get; set; }

    [Column("opisPozycji")]
    [StringLength(100)]
    public string OpisPozycji { get; set; } = null!;

    [Column("kwotaPozycji", TypeName = "decimal(10, 2)")]
    public decimal KwotaPozycji { get; set; }

    [ForeignKey("KaraId")]
    [InverseProperty("PozycjaPlatnoscis")]
    public virtual Kara? Kara { get; set; }

    [ForeignKey("PlatnoscId")]
    [InverseProperty("PozycjaPlatnoscis")]
    public virtual Platnosc Platnosc { get; set; } = null!;

    [ForeignKey("ProduktZakupionyId")]
    [InverseProperty("PozycjaPlatnoscis")]
    public virtual ProduktZakupiony? ProduktZakupiony { get; set; }
}
