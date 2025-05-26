using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AquaparkApp.Shared.Models;

[Table("Bramka")]
public partial class Bramka
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("nazwa")]
    [StringLength(50)]
    public string Nazwa { get; set; } = null!;

    [Column("typBramki")]
    [StringLength(20)]
    public string TypBramki { get; set; } = null!;

    [InverseProperty("Bramka")]
    public virtual ICollection<LogDostepu> LogDostepus { get; set; } = new List<LogDostepu>();

    [ForeignKey("BramkaId")]
    [InverseProperty("Bramkas")]
    public virtual ICollection<Atrakcja> Atrakcjas { get; set; } = new List<Atrakcja>();
}
