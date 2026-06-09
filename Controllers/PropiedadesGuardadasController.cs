using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CompraVendeYaBackend.Services;
using CompraVendeYaBackend.DTOs;
using System.Security.Claims;

namespace CompraVendeYaBackend.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PropiedadesGuardadasController : ControllerBase
{
    private readonly PropiedadGuardadaService _service;

    public PropiedadesGuardadasController(PropiedadGuardadaService service)
    {
        _service = service;
    }

    private int GetUsuarioId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim))
        {
            throw new UnauthorizedAccessException("Usuario no autenticado");
        }
        return int.Parse(userIdClaim);
    }

    [HttpGet]
    public async Task<IActionResult> GetMisGuardadas()
    {
        try
        {
            var usuarioId = GetUsuarioId();
            var guardadas = await _service.GetByUsuarioIdAsync(usuarioId);
            return Ok(guardadas);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var usuarioId = GetUsuarioId();
            var guardada = await _service.GetByIdAsync(id, usuarioId);
            if (guardada == null) return NotFound();
            return Ok(guardada);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost]
    public async Task<IActionResult> Guardar([FromBody] GuardarPropiedadRequestDto request)
    {
        try
        {
            if (request == null)
            {
                return BadRequest(new { message = "Datos no proporcionados" });
            }

            Console.WriteLine($"📥 Recibido guardar propiedad: {request.Titulo}");
            Console.WriteLine($"💰 Precio: {request.Precio}");
            Console.WriteLine($"📍 Ubicación: {request.Ciudad}, {request.Barrio}");

            var usuarioId = GetUsuarioId();
            var result = await _service.GuardarAsync(usuarioId, request);

            return Ok(result);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error: {ex.Message}");
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Eliminar(int id)
    {
        try
        {
            var usuarioId = GetUsuarioId();
            var deleted = await _service.EliminarAsync(id, usuarioId);
            if (!deleted) return NotFound();
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("check/{propiedadIdExterno}")]
    public async Task<IActionResult> IsGuardada(string propiedadIdExterno)
    {
        try
        {
            var usuarioId = GetUsuarioId();
            var isGuardada = await _service.IsPropiedadGuardadaAsync(usuarioId, propiedadIdExterno);
            return Ok(new { isGuardada });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message, isGuardada = false });
        }
    }
}