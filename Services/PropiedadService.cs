using CompraVendeYaBackend.Data;
using CompraVendeYaBackend.DTOs;
using CompraVendeYaBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace CompraVendeYaBackend.Services;

public class PropiedadService
{
    private readonly BienesRaicesDbContext _context;

    public PropiedadService(BienesRaicesDbContext context)
    {
        _context = context;
    }

    public async Task<List<PropiedadDto>> GetAllAsync()
    {
        var propiedades = await _context.Propiedades
            .Include(p => p.TipoPropiedad)
            .Include(p => p.Zona)
            .Include(p => p.Propietario)
            .Include(p => p.Agente)
            .Include(p => p.Estado)
            .Include(p => p.Operacion)
            .Include(p => p.Fotos)
            .ToListAsync();

        return propiedades.Select(p => MapToDto(p)).ToList();
    }

    public async Task<PropiedadDto?> GetByIdAsync(int id)
    {
        var propiedad = await _context.Propiedades
            .Include(p => p.TipoPropiedad)
            .Include(p => p.Zona)
            .Include(p => p.Propietario)
            .Include(p => p.Agente)
            .Include(p => p.Estado)
            .Include(p => p.Operacion)
            .Include(p => p.Fotos)
            .FirstOrDefaultAsync(p => p.IdPropiedad == id);

        return propiedad == null ? null : MapToDto(propiedad);
    }

    public async Task<PropiedadDto> CreateAsync(CreatePropiedadDto dto, int agenteId)
    {
        var propiedad = new Propiedad
        {
            IdTipo = dto.IdTipo,
            IdZona = dto.IdZona,
            IdPropietario = dto.IdPropietario,
            IdAgente = agenteId,
            IdEstado = dto.IdEstado ?? 1, // Disponible por defecto
            IdOperacion = dto.IdOperacion,
            Titulo = dto.Titulo,
            Descripcion = dto.Descripcion,
            Precio = dto.Precio,
            Direccion = dto.Direccion,
            AreaConstruida = dto.AreaConstruida,
            AreaTerreno = dto.AreaTerreno,
            Habitaciones = dto.Habitaciones,
            Banos = dto.Banos,
            Garajes = dto.Garajes,
            AnoConstruccion = dto.AnoConstruccion,
            Coordenadas = dto.Coordenadas,
            FechaPublicacion = DateOnly.FromDateTime(DateTime.UtcNow),
            FechaActualizacion = DateTime.UtcNow
        };

        _context.Propiedades.Add(propiedad);
        await _context.SaveChangesAsync();

        // Agregar fotos
        bool esPrimera = true;
        foreach (var url in dto.FotosUrls)
        {
            _context.FotosPropiedad.Add(new FotoPropiedad
            {
                IdPropiedad = propiedad.IdPropiedad,
                UrlFoto = url,
                EsPrincipal = esPrimera
            });
            esPrimera = false;
        }
        await _context.SaveChangesAsync();

        return await GetByIdAsync(propiedad.IdPropiedad) ?? throw new Exception("Error al crear la propiedad");
    }

    public async Task<PropiedadDto?> UpdateAsync(UpdatePropiedadDto dto)
    {
        var propiedad = await _context.Propiedades
            .Include(p => p.Fotos)
            .FirstOrDefaultAsync(p => p.IdPropiedad == dto.IdPropiedad);

        if (propiedad == null) return null;

        propiedad.IdTipo = dto.IdTipo;
        propiedad.IdZona = dto.IdZona;
        propiedad.IdPropietario = dto.IdPropietario;
        propiedad.IdEstado = dto.IdEstado;
        propiedad.IdOperacion = dto.IdOperacion;
        propiedad.Titulo = dto.Titulo;
        propiedad.Descripcion = dto.Descripcion;
        propiedad.Precio = dto.Precio;
        propiedad.Direccion = dto.Direccion;
        propiedad.AreaConstruida = dto.AreaConstruida;
        propiedad.AreaTerreno = dto.AreaTerreno;
        propiedad.Habitaciones = dto.Habitaciones;
        propiedad.Banos = dto.Banos;
        propiedad.Garajes = dto.Garajes;
        propiedad.AnoConstruccion = dto.AnoConstruccion;
        propiedad.Coordenadas = dto.Coordenadas;
        propiedad.FechaActualizacion = DateTime.UtcNow;

        // Actualizar fotos: simplemente reemplazar (opcional)
        if (dto.FotosUrls.Any())
        {
            _context.FotosPropiedad.RemoveRange(propiedad.Fotos);
            bool esPrimera = true;
            foreach (var url in dto.FotosUrls)
            {
                _context.FotosPropiedad.Add(new FotoPropiedad
                {
                    IdPropiedad = propiedad.IdPropiedad,
                    UrlFoto = url,
                    EsPrincipal = esPrimera
                });
                esPrimera = false;
            }
        }

        await _context.SaveChangesAsync();
        return await GetByIdAsync(propiedad.IdPropiedad);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var propiedad = await _context.Propiedades.FindAsync(id);
        if (propiedad == null) return false;

        _context.Propiedades.Remove(propiedad);
        await _context.SaveChangesAsync();
        return true;
    }

    private PropiedadDto MapToDto(Propiedad p)
    {
        return new PropiedadDto
        {
            IdPropiedad = p.IdPropiedad,
            IdTipo = p.IdTipo,
            TipoNombre = p.TipoPropiedad?.Nombre,
            IdZona = p.IdZona,
            ZonaNombre = p.Zona?.Nombre,
            IdPropietario = p.IdPropietario,
            PropietarioNombre = p.Propietario != null ? $"{p.Propietario.Nombre} {p.Propietario.Apellido}" : null,
            IdAgente = p.IdAgente,
            AgenteNombre = p.Agente != null ? $"{p.Agente.Nombre} {p.Agente.Apellido}" : null,
            IdEstado = p.IdEstado,
            EstadoNombre = p.Estado?.Nombre,
            IdOperacion = p.IdOperacion,
            OperacionNombre = p.Operacion?.Nombre,
            Titulo = p.Titulo,
            Descripcion = p.Descripcion,
            Precio = p.Precio,
            Direccion = p.Direccion,
            AreaConstruida = p.AreaConstruida,
            AreaTerreno = p.AreaTerreno,
            Habitaciones = p.Habitaciones,
            Banos = p.Banos,
            Garajes = p.Garajes,
            AnoConstruccion = p.AnoConstruccion,
            Coordenadas = p.Coordenadas,
            FechaPublicacion = p.FechaPublicacion,
            FotosUrls = p.Fotos.Select(f => f.UrlFoto).ToList()
        };
    }
}