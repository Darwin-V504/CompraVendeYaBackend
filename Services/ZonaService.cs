using CompraVendeYaBackend.Data;
using CompraVendeYaBackend.DTOs;
using CompraVendeYaBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace CompraVendeYaBackend.Services;

public class ZonaService
{
    private readonly BienesRaicesDbContext _context;

    public ZonaService(BienesRaicesDbContext context)
    {
        _context = context;
    }

    public async Task<List<ZonaDto>> GetAllAsync()
    {
        var zonas = await _context.Zonas.ToListAsync();
        return zonas.Select(z => MapToDto(z)).ToList();
    }

    public async Task<ZonaDto?> GetByIdAsync(int id)
    {
        var zona = await _context.Zonas.FindAsync(id);
        return zona == null ? null : MapToDto(zona);
    }

    public async Task<ZonaDto> CreateAsync(CreateZonaDto dto)
    {
        var zona = new Zona
        {
            Nombre = dto.Nombre,
            Ciudad = dto.Ciudad,
            Descripcion = dto.Descripcion
        };
        _context.Zonas.Add(zona);
        await _context.SaveChangesAsync();
        return MapToDto(zona);
    }

    public async Task<ZonaDto?> UpdateAsync(int id, CreateZonaDto dto)
    {
        var zona = await _context.Zonas.FindAsync(id);
        if (zona == null) return null;

        zona.Nombre = dto.Nombre;
        zona.Ciudad = dto.Ciudad;
        zona.Descripcion = dto.Descripcion;

        await _context.SaveChangesAsync();
        return MapToDto(zona);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var zona = await _context.Zonas.FindAsync(id);
        if (zona == null) return false;
        _context.Zonas.Remove(zona);
        await _context.SaveChangesAsync();
        return true;
    }

    private ZonaDto MapToDto(Zona z)
    {
        return new ZonaDto
        {
            IdZona = z.IdZona,
            Nombre = z.Nombre,
            Ciudad = z.Ciudad,
            Descripcion = z.Descripcion
        };
    }
}