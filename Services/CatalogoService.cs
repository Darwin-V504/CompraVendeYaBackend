using CompraVendeYaBackend.Data;
using CompraVendeYaBackend.DTOs;
using Microsoft.EntityFrameworkCore;

namespace CompraVendeYaBackend.Services;

public class CatalogoService
{
    private readonly BienesRaicesDbContext _context;

    public CatalogoService(BienesRaicesDbContext context)
    {
        _context = context;
    }

    public async Task<List<TipoPropiedadDto>> GetTiposAsync() =>
        await _context.TiposPropiedad
            .Select(t => new TipoPropiedadDto { IdTipo = t.IdTipo, Nombre = t.Nombre, Descripcion = t.Descripcion })
            .ToListAsync();

    public async Task<List<EstadoPropiedadDto>> GetEstadosAsync() =>
        await _context.EstadosPropiedad
            .Select(e => new EstadoPropiedadDto { IdEstado = e.IdEstado, Nombre = e.Nombre })
            .ToListAsync();

    public async Task<List<OperacionDto>> GetOperacionesAsync() =>
        await _context.Operaciones
            .Select(o => new OperacionDto { IdOperacion = o.IdOperacion, Nombre = o.Nombre })
            .ToListAsync();
}
