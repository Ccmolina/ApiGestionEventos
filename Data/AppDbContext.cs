using Microsoft.EntityFrameworkCore;
using GestionEventos.Models;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Evento> Eventos { get; set; }
    public DbSet<UsuarioEvento> UsuarioEventos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UsuarioEvento>()
            .HasKey(ue => new { ue.UsuarioId, ue.EventoId });

        modelBuilder.Entity<UsuarioEvento>()
            .HasOne(ue => ue.Usuario)
            .WithMany(u => u.UsuarioEventos)
            .HasForeignKey(ue => ue.UsuarioId);

        modelBuilder.Entity<UsuarioEvento>()
            .HasOne(ue => ue.Evento)
            .WithMany(e => e.UsuarioEventos)
            .HasForeignKey(ue => ue.EventoId);
    }
}
