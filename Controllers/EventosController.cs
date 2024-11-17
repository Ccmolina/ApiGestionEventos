using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using GestionEventos.Models;

namespace GestionEventos.Controllers
{
    [Route("api/eventos")]
    [ApiController]
    public class EventosController : ControllerBase
    {
        private readonly GestionContext _context;

        public EventosController(GestionContext context)
        {
            _context = context;
        }

        // POST: api/eventos/create
        [HttpPost("create")]
        [Authorize]
        public async Task<IActionResult> CreateEvent([FromBody] EventDTO model)
        {
            if (model.Date <= DateTime.Now)
                return BadRequest("No se puede crear un evento en el pasado.");

            var newEvent = new Evento
            {
                Titulo = model.Title,
                Descripcion = model.Description,
                Fecha = model.Date
            };

            _context.Eventos.Add(newEvent);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Evento creado satisfactoriamente", Event = newEvent });
        }

        // GET: api/eventos/all
        [HttpGet("all")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllEvents()
        {
            var events = await _context.Eventos.ToListAsync();
            return Ok(events);
        }
    }
}
