using CompraVendeYaBackend.Data;
using CompraVendeYaBackend.DTOs;
using CompraVendeYaBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace CompraVendeYaBackend.Services;

public class FotoPropiedadService
{
    private readonly BienesRaicesDbContext _context;

    public FotoPropiedadService(BienesRaicesDbContext context)
    {
        _context = context;
    }

    public async Task<List<FotoPropiedadDto>> GetByPropiedadAsync(int idPropiedad) =>
        await _context.FotosPropiedad
            .Where(f => f.IdPropiedad == idPropiedad)
            .Select(f => MapToDto(f))
            .ToListAsync();

    public async Task<FotoPropiedadDto?> GetByIdAsync(int id)
    {
        var foto = await _context.FotosPropiedad.FindAsync(id);
        return foto == null ? null : MapToDto(foto);
    }

    public async Task<FotoPropiedadDto> CreateAsync(int idPropiedad, CreateFotoPropiedadDto dto)
    {
        var foto = new FotoPropiedad
        {
            IdPropiedad = idPropiedad,
            UrlFoto = dto.UrlFoto,
            EsPrincipal = dto.EsPrincipal,
            FechaSubida = DateTime.UtcNow
        };
        _context.FotosPropiedad.Add(foto);
        await _context.SaveChangesAsync();
        return MapToDto(foto);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var foto = await _context.FotosPropiedad.FindAsync(id);
        if (foto == null) return false;
        _context.FotosPropiedad.Remove(foto);
        await _context.SaveChangesAsync();
        return true;
    }

    private static FotoPropiedadDto MapToDto(FotoPropiedad f) => new()
    {
        IdFoto = f.IdFoto,
        IdPropiedad = f.IdPropiedad,
        UrlFoto = f.UrlFoto,
        EsPrincipal = f.EsPrincipal,
        FechaSubida = f.FechaSubida
    };
}
