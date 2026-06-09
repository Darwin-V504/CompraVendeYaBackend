using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CompraVendeYaBackend.Services;
using CompraVendeYaBackend.DTOs;

namespace CompraVendeYaBackend.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TransaccionesController : ControllerBase
{
    private readonly TransaccionService _transaccionService;

    public TransaccionesController(TransaccionService transaccionService)
    {
        _transaccionService = transaccionService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _transaccionService.GetAllAsync());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var transaccion = await _transaccionService.GetByIdAsync(id);
        if (transaccion == null) return NotFound();
        return Ok(transaccion);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTransaccionDto dto)
    {
        var transaccion = await _transaccionService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = transaccion.IdTransaccion }, transaccion);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] CreateTransaccionDto dto)
    {
        var updated = await _transaccionService.UpdateAsync(id, dto);
        if (updated == null) return NotFound();
        return Ok(updated);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _transaccionService.DeleteAsync(id);
        if (!deleted) return NotFound();
        return NoContent();
    }
}