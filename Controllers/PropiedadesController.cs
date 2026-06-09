using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CompraVendeYaBackend.Services;
using CompraVendeYaBackend.DTOs;
using CompraVendeYaBackend.Data;

namespace CompraVendeYaBackend.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PropiedadesController : ControllerBase
{
    private readonly PropiedadService _propiedadService;
    private readonly BienesRaicesDbContext _context;

    public PropiedadesController(PropiedadService propiedadService, BienesRaicesDbContext context)
    {
        _propiedadService = propiedadService;
        _context = context;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll()
    {
        var propiedades = await _propiedadService.GetAllAsync();
        return Ok(propiedades);
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(int id)
    {
        var propiedad = await _propiedadService.GetByIdAsync(id);
        if (propiedad == null)
            return NotFound();
        return Ok(propiedad);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatePropiedadDto dto)
    {
        var agenteIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(agenteIdClaim))
            return Unauthorized();

        var agenteId = int.Parse(agenteIdClaim);
        try
        {
            var propiedad = await _propiedadService.CreateAsync(dto, agenteId);
            return CreatedAtAction(nameof(GetById), new { id = propiedad.IdPropiedad }, propiedad);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdatePropiedadDto dto)
    {
        if (id != dto.IdPropiedad)
            return BadRequest("El ID de la ruta no coincide con el DTO");

        var updated = await _propiedadService.UpdateAsync(dto);
        if (updated == null)
            return NotFound();
        return Ok(updated);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _propiedadService.DeleteAsync(id);
        if (!deleted)
            return NotFound();
        return NoContent();
    }

    [HttpGet("test-connection")]
    [AllowAnonymous]
    public async Task<IActionResult> TestConnection()
    {
        try
        {
            var canConnect = await _context.Database.CanConnectAsync();
            return Ok(new
            {
                connected = canConnect,
                database = "bienes_raices",
                message = canConnect ? "✅ Conexión exitosa" : "❌ No se pudo conectar",
                timestamp = DateTime.Now
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new
            {
                error = ex.Message,
                innerError = ex.InnerException?.Message
            });
        }
    }
}