using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CompraVendeYaBackend.Services;
using CompraVendeYaBackend.DTOs;

namespace CompraVendeYaBackend.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PropietariosController : ControllerBase
{
    private readonly PropietarioService _propietarioService;

    public PropietariosController(PropietarioService propietarioService)
    {
        _propietarioService = propietarioService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _propietarioService.GetAllAsync());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var propietario = await _propietarioService.GetByIdAsync(id);
        if (propietario == null) return NotFound();
        return Ok(propietario);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatePropietarioDto dto)
    {
        var propietario = await _propietarioService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = propietario.IdPropietario }, propietario);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] CreatePropietarioDto dto)
    {
        var updated = await _propietarioService.UpdateAsync(id, dto);
        if (updated == null) return NotFound();
        return Ok(updated);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _propietarioService.DeleteAsync(id);
        if (!deleted) return NotFound();
        return NoContent();
    }
}