using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Plataforma_De_Recomendacion_De_Contenido.Models;

public partial class DbplataformaRecomendacionDeContenidoContext : DbContext
{
    public DbplataformaRecomendacionDeContenidoContext()
    {
    }

    public DbplataformaRecomendacionDeContenidoContext(DbContextOptions<DbplataformaRecomendacionDeContenidoContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Contenido> Contenidos { get; set; }

    public virtual DbSet<Genero> Generos { get; set; }

    public virtual DbSet<GeneroContenido> GeneroContenidos { get; set; }

    public virtual DbSet<Pelicula> Peliculas { get; set; }

    public virtual DbSet<Serie> Series { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    public virtual DbSet<UsuarioContenido> UsuarioContenidos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Contenido>(entity =>
        {
            entity.ToTable("Contenido");

            entity.Property(e => e.ContenidoId)
                .HasColumnName("ContenidoID")
                .ValueGeneratedOnAdd();
            entity.Property(e => e.Director)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Pais)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Sinopsis)
                .HasMaxLength(900)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Genero>(entity =>
        {
            entity.ToTable("Genero");

            entity.Property(e => e.GeneroId).HasColumnName("GeneroID");
            entity.Property(e => e.Detalle)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<GeneroContenido>(entity =>
        {
            entity.ToTable("GeneroContenido");

            entity.Property(e => e.GeneroContenidoId).HasColumnName("GeneroContenidoID");
            entity.Property(e => e.ContenidoId).HasColumnName("ContenidoID");
            entity.Property(e => e.GeneroId).HasColumnName("GeneroID");
        });

        modelBuilder.Entity<Pelicula>(entity =>
        {
            entity.ToTable("Pelicula");
            entity.HasBaseType<Contenido>();

            entity.Property(e => e.ContenidoId)
                .ValueGeneratedNever()
                .HasColumnName("ContenidoID")
                .ValueGeneratedOnAdd();

            entity.Property(e => e.DuracionMinutos);
        });

        modelBuilder.Entity<Serie>(entity =>
        {
            entity.ToTable("Serie");
            entity.HasBaseType<Contenido>();

            entity.Property(e => e.ContenidoId)
                .ValueGeneratedNever()
                .HasColumnName("ContenidoID")
                .ValueGeneratedOnAdd();

            entity.Property(e => e.CantidadTemporadas);
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.ToTable("Usuario");

            entity.Property(e => e.UsuarioId).HasColumnName("UsuarioID");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Nacionalidad)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.NombreUsuario)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Rol)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<UsuarioContenido>(entity =>
        {
            entity.ToTable("UsuarioContenido");

            entity.Property(e => e.UsuarioContenidoId).HasColumnName("UsuarioContenidoID");
            entity.Property(e => e.ContenidoId).HasColumnName("ContenidoID");
            entity.Property(e => e.UsuarioId).HasColumnName("UsuarioID");

            entity.HasOne(d => d.Contenido).WithMany(p => p.UsuarioContenidos)
                .HasForeignKey(d => d.ContenidoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UsuarioContenido_Usuario");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
