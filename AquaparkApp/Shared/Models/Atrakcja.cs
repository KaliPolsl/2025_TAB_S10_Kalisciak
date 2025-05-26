using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AquaparkApp.Shared.Models;

[Table("Atrakcja")]
public partial class Atrakcja
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("nazwa")]
    [StringLength(100)]
    public string Nazwa { get; set; } = null!;

    [Column("opis")]
    public string? Opis { get; set; }

    [Column("maxOsób")]
    public int MaxOsób { get; set; }

    [Column("wymagaDodatkowejOplaty")]
    public bool WymagaDodatkowejOplaty { get; set; }

    [Column("cenaDodatkowa", TypeName = "decimal(10, 2)")]
    public decimal? CenaDodatkowa { get; set; }

    [ForeignKey("AtrakcjaId")]
    [InverseProperty("Atrakcjas")]
    public virtual ICollection<Bramka> Bramkas { get; set; } = new List<Bramka>();
}
