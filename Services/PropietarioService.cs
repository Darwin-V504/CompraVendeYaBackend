using CompraVendeYaBackend.Data;
using CompraVendeYaBackend.DTOs;
using CompraVendeYaBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace CompraVendeYaBackend.Services;

public class PropietarioService
{
    private readonly BienesRaicesDbContext _context;

    public PropietarioService(BienesRaicesDbContext context)
    {
        _context = context;
    }

    public async Task<List<PropietarioDto>> GetAllAsync()
    {
        var propietarios = await _context.Propietarios.ToListAsync();
        return propietarios.Select(p => MapToDto(p)).ToList();
    }

    public async Task<PropietarioDto?> GetByIdAsync(int id)
    {
        var propietario = await _context.Propietarios.FindAsync(id);
        return propietario == null ? null : MapToDto(propietario);
    }

    public async Task<PropietarioDto> CreateAsync(CreatePropietarioDto dto)
    {
        var propietario = new Propietario
        {
            Nombre = dto.Nombre,
            Apellido = dto.Apellido,
            Dni = dto.Dni,
            Telefono = dto.Telefono,
            Email = dto.Email,
            Direccion = dto.Direccion,
            FechaRegistro = DateTime.UtcNow
        };
        _context.Propietarios.Add(propietario);
        await _context.SaveChangesAsync();
        return MapToDto(propietario);
    }

    public async Task<PropietarioDto?> UpdateAsync(int id, CreatePropietarioDto dto)
    {
        var propietario = await _context.Propietarios.FindAsync(id);
        if (propietario == null) return null;

        propietario.Nombre = dto.Nombre;
        propietario.Apellido = dto.Apellido;
        propietario.Dni = dto.Dni;
        propietario.Telefono = dto.Telefono;
        propietario.Email = dto.Email;
        propietario.Direccion = dto.Direccion;

        await _context.SaveChangesAsync();
        return MapToDto(propietario);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var propietario = await _context.Propietarios.FindAsync(id);
        if (propietario == null) return false;
        _context.Propietarios.Remove(propietario);
        await _context.SaveChangesAsync();
        return true;
    }

    private PropietarioDto MapToDto(Propietario p)
    {
        return new PropietarioDto
        {
            IdPropietario = p.IdPropietario,
            Nombre = p.Nombre,
            Apellido = p.Apellido,
            Dni = p.Dni,
            Telefono = p.Telefono,
            Email = p.Email,
            Direccion = p.Direccion,
            FechaRegistro = p.FechaRegistro
        };
    }
}