using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CompraVendeYaBackend.Services;
using CompraVendeYaBackend.DTOs;

namespace CompraVendeYaBackend.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ZonasController : ControllerBase
{
    private readonly ZonaService _zonaService;

    public ZonasController(ZonaService zonaService)
    {
        _zonaService = zonaService;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _zonaService.GetAllAsync());
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(int id)
    {
        var zona = await _zonaService.GetByIdAsync(id);
        if (zona == null) return NotFound();
        return Ok(zona);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateZonaDto dto)
    {
        var zona = await _zonaService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = zona.IdZona }, zona);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] CreateZonaDto dto)
    {
        var updated = await _zonaService.UpdateAsync(id, dto);
        if (updated == null) return NotFound();
        return Ok(updated);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _zonaService.DeleteAsync(id);
        if (!deleted) return NotFound();
        return NoContent();
    }
}