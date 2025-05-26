using System;
using System.Collections.Generic;
using AquaparkApp.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace AquaparkApp.Server.Data;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Atrakcja> Atrakcjas { get; set; }

    public virtual DbSet<Bramka> Bramkas { get; set; }

    public virtual DbSet<Kara> Karas { get; set; }

    public virtual DbSet<Klient> Klients { get; set; }

    public virtual DbSet<LogDostepu> LogDostepus { get; set; }

    public virtual DbSet<OfertaCennikowa> OfertaCennikowas { get; set; }

    public virtual DbSet<Opaska> Opaskas { get; set; }

    public virtual DbSet<Platnosc> Platnoscs { get; set; }

    public virtual DbSet<PozycjaPlatnosci> PozycjaPlatnoscis { get; set; }

    public virtual DbSet<ProduktZakupiony> ProduktZakupionies { get; set; }

    public virtual DbSet<StatusWizyty> StatusWizyties { get; set; }

    public virtual DbSet<TypKary> TypKaries { get; set; }

    public virtual DbSet<Wizytum> Wizyta { get; set; }

    public virtual DbSet<Znizka> Znizkas { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=tcp:aquapark.database.windows.net,1433;Initial Catalog=aqua_04;Persist Security Info=False;User ID=admin123;Password=@dmin123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Bramka>(entity =>
        {
            entity.HasMany(d => d.Atrakcjas).WithMany(p => p.Bramkas)
                .UsingEntity<Dictionary<string, object>>(
                    "DostepAtrakcjiBramka",
                    r => r.HasOne<Atrakcja>().WithMany()
                        .HasForeignKey("AtrakcjaId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_DostepAtrakcjiBramka_Atrakcja"),
                    l => l.HasOne<Bramka>().WithMany()
                        .HasForeignKey("BramkaId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_DostepAtrakcjiBramka_Bramka"),
                    j =>
                    {
                        j.HasKey("BramkaId", "AtrakcjaId");
                        j.ToTable("DostepAtrakcjiBramka");
                        j.IndexerProperty<int>("BramkaId").HasColumnName("bramka_id");
                        j.IndexerProperty<int>("AtrakcjaId").HasColumnName("atrakcja_id");
                    });
        });

        modelBuilder.Entity<Kara>(entity =>
        {
            entity.HasOne(d => d.Oferta).WithMany(p => p.Karas).HasConstraintName("FK_Kara_OfertaCennikowa");

            entity.HasOne(d => d.TypKary).WithMany(p => p.Karas)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Kara_TypKary");

            entity.HasOne(d => d.Wizyta).WithMany(p => p.Karas)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Kara_Wizyta");
        });

        modelBuilder.Entity<LogDostepu>(entity =>
        {
            entity.HasOne(d => d.Bramka).WithMany(p => p.LogDostepus)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LogDostepu_Bramka");

            entity.HasOne(d => d.Wizyta).WithMany(p => p.LogDostepus)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LogDostepu_Wizyta");
        });

        modelBuilder.Entity<Platnosc>(entity =>
        {
            entity.HasIndex(e => e.KlientId, "IX_Platnosc_Klient").HasFilter("([klient_id] IS NOT NULL)");

            entity.HasOne(d => d.Klient).WithMany(p => p.Platnoscs).HasConstraintName("FK_Platnosc_Klient");
        });

        modelBuilder.Entity<PozycjaPlatnosci>(entity =>
        {
            entity.HasIndex(e => e.KaraId, "IX_PozycjaPlatnosci_Kara").HasFilter("([kara_id] IS NOT NULL)");

            entity.HasIndex(e => e.ProduktZakupionyId, "IX_PozycjaPlatnosci_Produkt").HasFilter("([produktZakupiony_id] IS NOT NULL)");

            entity.HasOne(d => d.Kara).WithMany(p => p.PozycjaPlatnoscis).HasConstraintName("FK_PozycjaPlatnosci_Kara");

            entity.HasOne(d => d.Platnosc).WithMany(p => p.PozycjaPlatnoscis)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PozycjaPlatnosci_Platnosc");

            entity.HasOne(d => d.ProduktZakupiony).WithMany(p => p.PozycjaPlatnoscis).HasConstraintName("FK_PozycjaPlatnosci_ProduktZakupiony");
        });

        modelBuilder.Entity<ProduktZakupiony>(entity =>
        {
            entity.HasOne(d => d.Klient).WithMany(p => p.ProduktZakupionies)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProduktZakupiony_Klient");

            entity.HasOne(d => d.Oferta).WithMany(p => p.ProduktZakupionies)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProduktZakupiony_OfertaCennikowa");

            entity.HasOne(d => d.Znizka).WithMany(p => p.ProduktZakupionies).HasConstraintName("FK_ProduktZakupiony_Znizka");
        });

        modelBuilder.Entity<Wizytum>(entity =>
        {
            entity.HasIndex(e => e.OpaskaId, "IX_Wizyta_AktywnaOpaskaUnikalna")
                .IsUnique()
                .HasFilter("([statusWizyty_id]=(1))");

            entity.HasOne(d => d.Klient).WithMany(p => p.Wizyta)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Wizyta_Klient");

            entity.HasOne(d => d.Opaska).WithOne(p => p.Wizytum)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Wizyta_Opaska");

            entity.HasOne(d => d.ProduktZakupiony).WithMany(p => p.Wizyta)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Wizyta_ProduktZakupiony");

            entity.HasOne(d => d.StatusWizyty).WithMany(p => p.Wizyta)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Wizyta_StatusWizyty");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
