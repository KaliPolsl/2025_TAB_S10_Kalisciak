using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AquaparkApp.Shared.Models;

[Table("StatusWizyty")]
[Index("Nazwa", Name = "UQ_StatusWizyty_Nazwa", IsUnique = true)]
public partial class StatusWizyty
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("nazwa")]
    [StringLength(30)]
    public string Nazwa { get; set; } = null!;

    [Column("opis")]
    [StringLength(255)]
    public string? Opis { get; set; }

    [InverseProperty("StatusWizyty")]
    public virtual ICollection<Wizytum> Wizyta { get; set; } = new List<Wizytum>();
}
