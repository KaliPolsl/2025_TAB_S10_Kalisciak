using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AquaparkApp.Shared.Models;

[Table("Kara")]
[Index("WizytaId", Name = "IX_Kara_Wizyta")]
public partial class Kara
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("wizyta_id")]
    public int WizytaId { get; set; }

    [Column("typKary_id")]
    public int TypKaryId { get; set; }

    [Column("oferta_id")]
    public int? OfertaId { get; set; }

    [Column("kwota", TypeName = "decimal(10, 2)")]
    public decimal Kwota { get; set; }

    [Column("opis")]
    [StringLength(200)]
    public string? Opis { get; set; }

    [Column("dataNaliczenia", TypeName = "datetime")]
    public DateTime DataNaliczenia { get; set; }

    [Column("statusPlatnosci")]
    [StringLength(20)]
    public string StatusPlatnosci { get; set; } = null!;

    [ForeignKey("OfertaId")]
    [InverseProperty("Karas")]
    public virtual OfertaCennikowa? Oferta { get; set; }

    [InverseProperty("Kara")]
    public virtual ICollection<PozycjaPlatnosci> PozycjaPlatnoscis { get; set; } = new List<PozycjaPlatnosci>();

    [ForeignKey("TypKaryId")]
    [InverseProperty("Karas")]
    public virtual TypKary TypKary { get; set; } = null!;

    [ForeignKey("WizytaId")]
    [InverseProperty("Karas")]
    public virtual Wizytum Wizyta { get; set; } = null!;
}
