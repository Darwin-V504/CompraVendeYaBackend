using CompraVendeYaBackend.Data;
using CompraVendeYaBackend.DTOs;
using CompraVendeYaBackend.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace CompraVendeYaBackend.Services;

public class PropiedadGuardadaService
{
    private readonly BienesRaicesDbContext _context;

    public PropiedadGuardadaService(BienesRaicesDbContext context)
    {
        _context = context;
    }

    public async Task<List<PropiedadGuardadaDto>> GetByUsuarioIdAsync(int usuarioId)
    {
        var guardadas = await _context.PropiedadesGuardada
            .Where(p => p.UsuarioId == usuarioId)
            .OrderByDescending(p => p.FechaGuardado)
            .ToListAsync();

        return guardadas.Select(MapToDto).ToList();
    }

    public async Task<PropiedadGuardadaDto?> GetByIdAsync(int id, int usuarioId)
    {
        var guardada = await _context.PropiedadesGuardada
            .FirstOrDefaultAsync(p => p.IdGuardado == id && p.UsuarioId == usuarioId);

        return guardada == null ? null : MapToDto(guardada);
    }

    public async Task<PropiedadGuardadaDto> GuardarAsync(int usuarioId, GuardarPropiedadRequestDto request)
    {
        // Verificar si ya está guardada
        var existe = await _context.PropiedadesGuardada
            .AnyAsync(p => p.UsuarioId == usuarioId && p.PropiedadIdExterno == request.PropiedadIdExterno);

        if (existe)
        {
            throw new Exception("Esta propiedad ya está guardada");
        }

        var guardada = new PropiedadGuardada
        {
            UsuarioId = usuarioId,
            PropiedadIdExterno = request.PropiedadIdExterno,
            Titulo = request.Titulo,
            Precio = request.Precio,
            Direccion = request.Direccion,
            Ciudad = request.Ciudad,
            Barrio = request.Barrio,
            Habitaciones = request.Habitaciones,
            Banos = request.Banos,
            Area = request.Area,
            UrlImagen = request.UrlImagen,
            TipoOperacion = request.TipoOperacion,
            ClaseSocial = request.ClaseSocial,
            Descripcion = request.Descripcion,
            Caracteristicas = request.Caracteristicas != null ? JsonSerializer.Serialize(request.Caracteristicas) : null,
            FechaGuardado = DateTime.UtcNow
        };

        _context.PropiedadesGuardada.Add(guardada);
        await _context.SaveChangesAsync();

        return MapToDto(guardada);
    }

    public async Task<bool> EliminarAsync(int id, int usuarioId)
    {
        var guardada = await _context.PropiedadesGuardada
            .FirstOrDefaultAsync(p => p.IdGuardado == id && p.UsuarioId == usuarioId);

        if (guardada == null) return false;

        _context.PropiedadesGuardada.Remove(guardada);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> IsPropiedadGuardadaAsync(int usuarioId, string propiedadIdExterno)
    {
        return await _context.PropiedadesGuardada
            .AnyAsync(p => p.UsuarioId == usuarioId && p.PropiedadIdExterno == propiedadIdExterno);
    }

    private PropiedadGuardadaDto MapToDto(PropiedadGuardada p)
    {
        return new PropiedadGuardadaDto
        {
            IdGuardado = p.IdGuardado,
            PropiedadIdExterno = p.PropiedadIdExterno,
            Titulo = p.Titulo,
            Precio = p.Precio,
            Direccion = p.Direccion,
            Ciudad = p.Ciudad,
            Barrio = p.Barrio,
            Habitaciones = p.Habitaciones,
            Banos = p.Banos,
            Area = p.Area,
            UrlImagen = p.UrlImagen,
            TipoOperacion = p.TipoOperacion,
            ClaseSocial = p.ClaseSocial,
            Descripcion = p.Descripcion,
            Caracteristicas = string.IsNullOrEmpty(p.Caracteristicas) ? null : JsonSerializer.Deserialize<List<string>>(p.Caracteristicas),
            FechaGuardado = p.FechaGuardado
        };
    }
}