using GestionPersonas.Application.Services;
using GestionPersonas.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace GestionPersonas.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PersonasController(PersonaService personaService) : ControllerBase
    {
        [HttpPost]
        public IActionResult Registrar([FromBody] Persona persona)
        {
            try
            {
                personaService.RegistrarPersona(persona);
                return Ok(new { Mensaje = "Persona registrada exitosamente." });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = "Error interno del servidor.", Detalles = ex.Message });
            }
        }
    }
}
