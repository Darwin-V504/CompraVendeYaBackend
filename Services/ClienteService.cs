using CompraVendeYaBackend.Data;
using CompraVendeYaBackend.DTOs;
using CompraVendeYaBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace CompraVendeYaBackend.Services;

public class ClienteService
{
    private readonly BienesRaicesDbContext _context;

    public ClienteService(BienesRaicesDbContext context)
    {
        _context = context;
    }

    public async Task<List<ClienteDto>> GetAllAsync()
    {
        var clientes = await _context.Clientes.ToListAsync();
        return clientes.Select(c => MapToDto(c)).ToList();
    }

    public async Task<ClienteDto?> GetByIdAsync(int id)
    {
        var cliente = await _context.Clientes.FindAsync(id);
        return cliente == null ? null : MapToDto(cliente);
    }

    public async Task<ClienteDto> CreateAsync(CreateClienteDto dto)
    {
        var cliente = new Cliente
        {
            Nombre = dto.Nombre,
            Apellido = dto.Apellido,
            Dni = dto.Dni,
            Telefono = dto.Telefono,
            Email = dto.Email,
            TipoCliente = dto.TipoCliente,
            FechaRegistro = DateTime.UtcNow
        };
        _context.Clientes.Add(cliente);
        await _context.SaveChangesAsync();
        return MapToDto(cliente);
    }

    public async Task<ClienteDto?> UpdateAsync(int id, CreateClienteDto dto)
    {
        var cliente = await _context.Clientes.FindAsync(id);
        if (cliente == null) return null;

        cliente.Nombre = dto.Nombre;
        cliente.Apellido = dto.Apellido;
        cliente.Dni = dto.Dni;
        cliente.Telefono = dto.Telefono;
        cliente.Email = dto.Email;
        cliente.TipoCliente = dto.TipoCliente;

        await _context.SaveChangesAsync();
        return MapToDto(cliente);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var cliente = await _context.Clientes.FindAsync(id);
        if (cliente == null) return false;
        _context.Clientes.Remove(cliente);
        await _context.SaveChangesAsync();
        return true;
    }

    private ClienteDto MapToDto(Cliente c)
    {
        return new ClienteDto
        {
            IdCliente = c.IdCliente,
            Nombre = c.Nombre,
            Apellido = c.Apellido,
            Dni = c.Dni,
            Telefono = c.Telefono,
            Email = c.Email,
            TipoCliente = c.TipoCliente,
            FechaRegistro = c.FechaRegistro
        };
    }
}