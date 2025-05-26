using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AquaparkApp.Shared.Models;

[Index("KlientId", Name = "IX_Wizyta_Klient")]
[Index("ProduktZakupionyId", Name = "IX_Wizyta_Produkt")]
public partial class Wizytum
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("klient_id")]
    public int KlientId { get; set; }

    [Column("opaska_id")]
    public int OpaskaId { get; set; }

    [Column("produktZakupiony_id")]
    public int ProduktZakupionyId { get; set; }

    [Column("czasWejscia", TypeName = "datetime")]
    public DateTime CzasWejscia { get; set; }

    [Column("czasWyjscia", TypeName = "datetime")]
    public DateTime? CzasWyjscia { get; set; }

    [Column("planowanyCzasWyjscia", TypeName = "datetime")]
    public DateTime? PlanowanyCzasWyjscia { get; set; }

    [Column("statusWizyty_id")]
    public int StatusWizytyId { get; set; }

    [InverseProperty("Wizyta")]
    public virtual ICollection<Kara> Karas { get; set; } = new List<Kara>();

    [ForeignKey("KlientId")]
    [InverseProperty("Wizyta")]
    public virtual Klient Klient { get; set; } = null!;

    [InverseProperty("Wizyta")]
    public virtual ICollection<LogDostepu> LogDostepus { get; set; } = new List<LogDostepu>();

    [ForeignKey("OpaskaId")]
    [InverseProperty("Wizytum")]
    public virtual Opaska Opaska { get; set; } = null!;

    [ForeignKey("ProduktZakupionyId")]
    [InverseProperty("Wizyta")]
    public virtual ProduktZakupiony ProduktZakupiony { get; set; } = null!;

    [ForeignKey("StatusWizytyId")]
    [InverseProperty("Wizyta")]
    public virtual StatusWizyty StatusWizyty { get; set; } = null!;
}
