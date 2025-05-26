using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AquaparkApp.Shared.Models;

[Table("ProduktZakupiony")]
[Index("KlientId", Name = "IX_ProduktZakupiony_Klient")]
[Index("OfertaId", Name = "IX_ProduktZakupiony_Oferta")]
public partial class ProduktZakupiony
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("klient_id")]
    public int KlientId { get; set; }

    [Column("oferta_id")]
    public int OfertaId { get; set; }

    [Column("znizka_id")]
    public int? ZnizkaId { get; set; }

    [Column("cenaZakupu", TypeName = "decimal(10, 2)")]
    public decimal CenaZakupu { get; set; }

    [Column("dataZakupu", TypeName = "datetime")]
    public DateTime DataZakupu { get; set; }

    [Column("status")]
    [StringLength(20)]
    public string Status { get; set; } = null!;

    [Column("pozostaloWejsc")]
    public int? PozostaloWejsc { get; set; }

    [Column("waznyOd", TypeName = "datetime")]
    public DateTime? WaznyOd { get; set; }

    [Column("waznyDo", TypeName = "datetime")]
    public DateTime? WaznyDo { get; set; }

    [ForeignKey("KlientId")]
    [InverseProperty("ProduktZakupionies")]
    public virtual Klient Klient { get; set; } = null!;

    [ForeignKey("OfertaId")]
    [InverseProperty("ProduktZakupionies")]
    public virtual OfertaCennikowa Oferta { get; set; } = null!;

    [InverseProperty("ProduktZakupiony")]
    public virtual ICollection<PozycjaPlatnosci> PozycjaPlatnoscis { get; set; } = new List<PozycjaPlatnosci>();

    [InverseProperty("ProduktZakupiony")]
    public virtual ICollection<Wizytum> Wizyta { get; set; } = new List<Wizytum>();

    [ForeignKey("ZnizkaId")]
    [InverseProperty("ProduktZakupionies")]
    public virtual Znizka? Znizka { get; set; }
}
