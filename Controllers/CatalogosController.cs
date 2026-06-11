using Microsoft.AspNetCore.Mvc;
using CompraVendeYaBackend.Services;

namespace CompraVendeYaBackend.Controllers;

[ApiController]
[Route("api")]
public class CatalogosController : ControllerBase
{
    private readonly CatalogoService _catalogoService;

    public CatalogosController(CatalogoService catalogoService)
    {
        _catalogoService = catalogoService;
    }

    [HttpGet("TiposPropiedad")]
    public async Task<IActionResult> GetTipos() =>
        Ok(await _catalogoService.GetTiposAsync());

    [HttpGet("EstadosPropiedad")]
    public async Task<IActionResult> GetEstados() =>
        Ok(await _catalogoService.GetEstadosAsync());

    [HttpGet("Operaciones")]
    public async Task<IActionResult> GetOperaciones() =>
        Ok(await _catalogoService.GetOperacionesAsync());
}
