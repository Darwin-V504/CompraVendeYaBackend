using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CompraVendeYaBackend.DTOs;
using CompraVendeYaBackend.Services;

namespace CompraVendeYaBackend.Controllers;

[ApiController]
[Route("api/Propiedades/{idPropiedad}/Fotos")]
[Authorize]
public class FotosPropiedadController : ControllerBase
{
    private readonly FotoPropiedadService _fotoService;

    public FotosPropiedadController(FotoPropiedadService fotoService)
    {
        _fotoService = fotoService;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll(int idPropiedad) =>
        Ok(await _fotoService.GetByPropiedadAsync(idPropiedad));

    [HttpPost]
    public async Task<IActionResult> Create(int idPropiedad, [FromBody] CreateFotoPropiedadDto dto)
    {
        var foto = await _fotoService.CreateAsync(idPropiedad, dto);
        return CreatedAtAction(nameof(GetAll), new { idPropiedad }, foto);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int idPropiedad, int id)
    {
        var deleted = await _fotoService.DeleteAsync(id);
        if (!deleted) return NotFound();
        return NoContent();
    }
}
