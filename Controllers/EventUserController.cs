using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Linq;
using GestionEventos.Models;

namespace GestionEventos.Controllers
{
    [ApiController]
    [Route("api/event-user")]
    public class EventUserController : ControllerBase
    {
        private readonly GestionContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public EventUserController(GestionContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // POST: api/event-user/join
        [HttpPost("join")]
        [Authorize]
        public async Task<IActionResult> JoinEvent([FromBody] JoinEventDTO model)
        {
            var eventToJoin = await _context.Eventos.FindAsync(model.EventoId);
            if (eventToJoin == null)
                return NotFound("Evento no encontrado.");

            if (eventToJoin.Fecha <= DateTime.Now)
                return BadRequest("No se puede unir a un evento que ya ha pasado.");

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized("Usuario no autenticado.");

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return Unauthorized("Usuario no encontrado.");

            var userEventExists = await _context.UsuarioEventos
.AnyAsync(ue => ue.UsuarioId == int.Parse(userId) && ue.EventoId == model.EventoId);
                if (userEventExists)
                return BadRequest("Ya estás registrado en este evento.");

            var userEvent = new UsuarioEvento
            {
                UsuarioId = int.Parse(userId),
                EventoId = eventToJoin.EventoId
            };

            _context.UsuarioEventos.Add(userEvent);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Te has unido al evento satisfactoriamente." });
        }

        // GET: api/event-user/participants/{eventId}
        [HttpGet("participants/{eventId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetEventParticipants(int eventId)
        {
            var eventToView = await _context.Eventos
                .Include(e => e.UsuarioEventos)
                .ThenInclude(ue => ue.Usuario)
                .FirstOrDefaultAsync(e => e.EventoId == eventId);

            if (eventToView == null)
                return NotFound("Evento no encontrado.");

            var participants = eventToView.UsuarioEventos
                .Select(ue => new { ue.Usuario.UsuarioEventos, ue.Usuario.Correo })
                .ToList();

            return Ok(participants);
        }
    }
}
