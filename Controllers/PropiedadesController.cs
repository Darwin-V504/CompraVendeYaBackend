using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CompraVendeYaBackend.Data;
using CompraVendeYaBackend.Models;

namespace CompraVendeYaBackend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PropiedadesController : ControllerBase
{
    private readonly BienesRaicesDbContext _context;

    public PropiedadesController(BienesRaicesDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetPropiedades()
    {
        var propiedades = await _context.Propiedades
            .Include(p => p.TipoPropiedad)
            .Include(p => p.Zona)
            .Include(p => p.Estado)
            .Include(p => p.Operacion)
            .Include(p => p.Fotos)
            .ToListAsync();

        return Ok(propiedades);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPropiedad(int id)
    {
        var propiedad = await _context.Propiedades
            .Include(p => p.TipoPropiedad)
            .Include(p => p.Zona)
            .Include(p => p.Estado)
            .Include(p => p.Operacion)
            .Include(p => p.Fotos)
            .FirstOrDefaultAsync(p => p.IdPropiedad == id);

        if (propiedad == null)
        {
            return NotFound();
        }

        return Ok(propiedad);
    }
}