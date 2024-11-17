using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using GestionEventos.Models;

public class GestionContext : DbContext
{
    public GestionContext(DbContextOptions<GestionContext> options) : base(options) { }

    // DbSet para Eventos
    public DbSet<Evento> Eventos { get; set; }

    // DbSet para la relación muchos a muchos entre Usuario y Evento
    public DbSet<UsuarioEvento> UsuarioEventos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configuración de la clave compuesta en UsuarioEvento
        modelBuilder.Entity<UsuarioEvento>()
            .HasKey(eu => new { eu.EventoId, eu.UsuarioId });

        // Configuración de la relación entre Evento y UsuarioEvento
        modelBuilder.Entity<UsuarioEvento>()
            .HasOne(eu => eu.Evento)
            .WithMany(e => e.UsuarioEventos)  // La propiedad UsuarioEventos debe estar en la clase Evento
            .HasForeignKey(eu => eu.EventoId);

        // Configuración de la relación entre Usuario y UsuarioEvento
        modelBuilder.Entity<UsuarioEvento>()
            .HasOne(eu => eu.Usuario)  // Usamos Usuario en lugar de ApplicationUser
            .WithMany(u => u.UsuarioEventos)  // La propiedad UsuarioEventos debe estar en la clase Usuario
            .HasForeignKey(eu => eu.UsuarioId);
    }
}
