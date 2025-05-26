using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AquaparkApp.Shared.Models;

[Table("LogDostepu")]
[Index("BramkaId", Name = "IX_LogDostepu_Bramka")]
[Index("CzasZdarzenia", Name = "IX_LogDostepu_CzasZdarzenia")]
[Index("WizytaId", Name = "IX_LogDostepu_Wizyta")]
public partial class LogDostepu
{
    [Key]
    [Column("id")]
    public long Id { get; set; }

    [Column("wizyta_id")]
    public int WizytaId { get; set; }

    [Column("bramka_id")]
    public int BramkaId { get; set; }

    [Column("czasZdarzenia", TypeName = "datetime")]
    public DateTime CzasZdarzenia { get; set; }

    [Column("typZdarzenia")]
    [StringLength(10)]
    public string TypZdarzenia { get; set; } = null!;

    [ForeignKey("BramkaId")]
    [InverseProperty("LogDostepus")]
    public virtual Bramka Bramka { get; set; } = null!;

    [ForeignKey("WizytaId")]
    [InverseProperty("LogDostepus")]
    public virtual Wizytum Wizyta { get; set; } = null!;
}
