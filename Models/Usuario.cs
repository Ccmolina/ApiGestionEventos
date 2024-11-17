namespace GestionEventos.Models{
public class Usuario
{
    public int UsuarioId { get; set; }
    public string Nombre { get; set; }
    public string Correo { get; set; }
    public string Contrasena { get; set; }

    public ICollection<UsuarioEvento> UsuarioEventos { get; set; } = new List<UsuarioEvento>();
}
}
