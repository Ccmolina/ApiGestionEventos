using GestionEventos.Models;

public class Evento
{
    public int EventoId { get; set; }
    public string Titulo { get; set; }
    public string Descripcion { get; set; }
    public DateTime Fecha { get; set; }

    public ICollection<UsuarioEvento> UsuarioEventos { get; set; } = new List<UsuarioEvento>();
}
