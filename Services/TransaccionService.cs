using CompraVendeYaBackend.Data;
using CompraVendeYaBackend.DTOs;
using CompraVendeYaBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace CompraVendeYaBackend.Services;

public class TransaccionService
{
    private readonly BienesRaicesDbContext _context;

    public TransaccionService(BienesRaicesDbContext context)
    {
        _context = context;
    }

    public async Task<List<TransaccionDto>> GetAllAsync()
    {
        var transacciones = await _context.Transacciones
            .Include(t => t.Propiedad)
            .Include(t => t.Cliente)
            .Include(t => t.Agente)
            .Include(t => t.Operacion)
            .ToListAsync();

        return transacciones.Select(t => MapToDto(t)).ToList();
    }

    public async Task<TransaccionDto?> GetByIdAsync(int id)
    {
        var transaccion = await _context.Transacciones
            .Include(t => t.Propiedad)
            .Include(t => t.Cliente)
            .Include(t => t.Agente)
            .Include(t => t.Operacion)
            .FirstOrDefaultAsync(t => t.IdTransaccion == id);

        return transaccion == null ? null : MapToDto(transaccion);
    }

    public async Task<TransaccionDto> CreateAsync(CreateTransaccionDto dto)
    {
        var transaccion = new Transaccion
        {
            IdPropiedad = dto.IdPropiedad,
            IdCliente = dto.IdCliente,
            IdAgente = dto.IdAgente,
            IdOperacion = dto.IdOperacion,
            FechaTransaccion = dto.FechaTransaccion,
            MontoTotal = dto.MontoTotal,
            ComisionAgente = dto.ComisionAgente,
            EstadoTransaccion = dto.EstadoTransaccion ?? "En Proceso",
            Detalles = dto.Detalles,
            FechaRegistro = DateTime.UtcNow
        };
        _context.Transacciones.Add(transaccion);
        await _context.SaveChangesAsync();
        return await GetByIdAsync(transaccion.IdTransaccion) ?? throw new Exception("Error al crear transacción");
    }

    public async Task<TransaccionDto?> UpdateAsync(int id, CreateTransaccionDto dto)
    {
        var transaccion = await _context.Transacciones.FindAsync(id);
        if (transaccion == null) return null;

        transaccion.IdPropiedad = dto.IdPropiedad;
        transaccion.IdCliente = dto.IdCliente;
        transaccion.IdAgente = dto.IdAgente;
        transaccion.IdOperacion = dto.IdOperacion;
        transaccion.FechaTransaccion = dto.FechaTransaccion;
        transaccion.MontoTotal = dto.MontoTotal;
        transaccion.ComisionAgente = dto.ComisionAgente;
        transaccion.EstadoTransaccion = dto.EstadoTransaccion;
        transaccion.Detalles = dto.Detalles;

        await _context.SaveChangesAsync();
        return await GetByIdAsync(id);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var transaccion = await _context.Transacciones.FindAsync(id);
        if (transaccion == null) return false;
        _context.Transacciones.Remove(transaccion);
        await _context.SaveChangesAsync();
        return true;
    }

    private TransaccionDto MapToDto(Transaccion t)
    {
        return new TransaccionDto
        {
            IdTransaccion = t.IdTransaccion,
            IdPropiedad = t.IdPropiedad,
            PropiedadTitulo = t.Propiedad?.Titulo,
            IdCliente = t.IdCliente,
            ClienteNombre = t.Cliente != null ? $"{t.Cliente.Nombre} {t.Cliente.Apellido}" : null,
            IdAgente = t.IdAgente,
            AgenteNombre = t.Agente != null ? $"{t.Agente.Nombre} {t.Agente.Apellido}" : null,
            IdOperacion = t.IdOperacion,
            OperacionNombre = t.Operacion?.Nombre,
            FechaTransaccion = t.FechaTransaccion,
            MontoTotal = t.MontoTotal,
            ComisionAgente = t.ComisionAgente,
            EstadoTransaccion = t.EstadoTransaccion,
            Detalles = t.Detalles
        };
    }
}