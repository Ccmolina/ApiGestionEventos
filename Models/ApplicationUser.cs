using GestionEventos.Models;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

public class ApplicationUser : IdentityUser
{
    // Relación muchos a muchos con Evento a través de EventoUsuario
    public List<UsuarioEvento> Eventos { get; set; } = new List<UsuarioEvento>();
}
