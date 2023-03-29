using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ArmadillosManager.Models;

public partial class Sistem21ClubdeportivoContext : DbContext
{
    public Sistem21ClubdeportivoContext()
    {
    }

    public Sistem21ClubdeportivoContext(DbContextOptions<Sistem21ClubdeportivoContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Categoria> Categoria { get; set; }

    public virtual DbSet<Jugador> Jugador { get; set; }

    public virtual DbSet<Liga> Liga { get; set; }

    public virtual DbSet<Movimientos> Movimientos { get; set; }

    public virtual DbSet<Pago> Pago { get; set; }

    public virtual DbSet<Responsable> Responsable { get; set; }

    public virtual DbSet<Temporada> Temporada { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8_general_ci")
            .HasCharSet("utf8");

        modelBuilder.Entity<Categoria>(entity =>
        {
            entity.HasKey(e => e.IdCategoria).HasName("PRIMARY");

            entity.ToTable("categoria");

            entity.Property(e => e.IdCategoria)
                .HasColumnType("int(11)")
                .HasColumnName("idCategoria");
            entity.Property(e => e.Categoria1)
                .HasMaxLength(20)
                .HasColumnName("Categoria");
        });

        modelBuilder.Entity<Jugador>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("jugador");

            entity.HasIndex(e => e.Categoria, "FkIdCategoria_idx");

            entity.HasIndex(e => e.Liga, "FkIdLiga_idx");

            entity.HasIndex(e => e.IdResponsable, "FkIdResponsable_idx");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.Categoria).HasColumnType("int(11)");
            entity.Property(e => e.Direccion).HasMaxLength(250);
            entity.Property(e => e.IdResponsable).HasColumnType("int(11)");
            entity.Property(e => e.Liga).HasColumnType("int(11)");
            entity.Property(e => e.Nombre).HasMaxLength(120);
            entity.Property(e => e.Telefono).HasMaxLength(20);

            entity.HasOne(d => d.CategoriaNavigation).WithMany(p => p.Jugador)
                .HasForeignKey(d => d.Categoria)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FkIdCategoria");

            entity.HasOne(d => d.IdResponsableNavigation).WithMany(p => p.Jugador)
                .HasForeignKey(d => d.IdResponsable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FkIdResponsable");

            entity.HasOne(d => d.LigaNavigation).WithMany(p => p.Jugador)
                .HasForeignKey(d => d.Liga)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FkIdLiga");
        });

        modelBuilder.Entity<Liga>(entity =>
        {
            entity.HasKey(e => e.IdLiga).HasName("PRIMARY");

            entity.ToTable("liga");

            entity.Property(e => e.IdLiga)
                .HasColumnType("int(11)")
                .HasColumnName("idLiga");
            entity.Property(e => e.Liga1)
                .HasMaxLength(45)
                .HasColumnName("Liga");
        });

        modelBuilder.Entity<Movimientos>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("movimientos");

            entity.HasIndex(e => e.IdPago, "FkIdPago_idx");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.Concepto).HasMaxLength(45);
            entity.Property(e => e.Fecha).HasColumnType("datetime");
            entity.Property(e => e.IdPago).HasColumnType("int(11)");
            entity.Property(e => e.Monto).HasMaxLength(45);
            entity.Property(e => e.Tipo).HasMaxLength(45);

            entity.HasOne(d => d.IdPagoNavigation).WithMany(p => p.Movimientos)
                .HasForeignKey(d => d.IdPago)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FkIdPago");
        });

        modelBuilder.Entity<Pago>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("pago");

            entity.HasIndex(e => e.IdJugador, "FkIdJugadaor_idx");

            entity.HasIndex(e => e.IdResponsable, "FkIdResposbale_idx");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.IdJugador).HasColumnType("int(11)");
            entity.Property(e => e.IdResponsable).HasColumnType("int(11)");
            entity.Property(e => e.MontoRestante).HasPrecision(10);
            entity.Property(e => e.MontoTotal).HasPrecision(10);

            entity.HasOne(d => d.IdJugadorNavigation).WithMany(p => p.Pago)
                .HasForeignKey(d => d.IdJugador)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FkIdJugadaor");

            entity.HasOne(d => d.IdResponsableNavigation).WithMany(p => p.Pago)
                .HasForeignKey(d => d.IdResponsable)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FkIdResposbale");
        });

        modelBuilder.Entity<Responsable>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("responsable");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.Contraseña).HasMaxLength(100);
            entity.Property(e => e.Correo).HasMaxLength(90);
            entity.Property(e => e.Direccion).HasMaxLength(250);
            entity.Property(e => e.Nombre).HasMaxLength(120);
            entity.Property(e => e.Rfc)
                .HasMaxLength(20)
                .HasColumnName("RFC");
            entity.Property(e => e.Telefono).HasMaxLength(20);
        });

        modelBuilder.Entity<Temporada>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("temporada");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.CostoTemporada).HasPrecision(10);
            entity.Property(e => e.CostoUniforme).HasPrecision(10);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
