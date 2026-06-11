using System.ComponentModel.DataAnnotations;

namespace CompraVendeYaBackend.DTOs;

public class PropiedadDto
{
    public int IdPropiedad { get; set; }
    public int? IdTipo { get; set; }
    public string? TipoNombre { get; set; }
    public int? IdZona { get; set; }
    public string? ZonaNombre { get; set; }
    public int? IdPropietario { get; set; }
    public string? PropietarioNombre { get; set; }
    public int? IdAgente { get; set; }
    public string? AgenteNombre { get; set; }
    public int? IdEstado { get; set; }
    public string? EstadoNombre { get; set; }
    public int? IdOperacion { get; set; }
    public string? OperacionNombre { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public decimal Precio { get; set; }
    public string Direccion { get; set; } = string.Empty;
    public decimal? AreaConstruida { get; set; }
    public decimal? AreaTerreno { get; set; }
    public int? Habitaciones { get; set; }
    public int? Banos { get; set; }
    public int? Garajes { get; set; }
    public int? AnoConstruccion { get; set; }
    public string? Coordenadas { get; set; }
    public DateOnly FechaPublicacion { get; set; }
    public List<string> FotosUrls { get; set; } = new();
}

public class CreatePropiedadDto
{
    public int? IdTipo { get; set; }
    public int? IdZona { get; set; }
    public int? IdPropietario { get; set; }
    public int? IdAgente { get; set; }
    public int? IdEstado { get; set; }
    public int? IdOperacion { get; set; }
    [Required] public string Titulo { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    [Range(0, double.MaxValue)] public decimal Precio { get; set; }
    [Required] public string Direccion { get; set; } = string.Empty;
    public decimal? AreaConstruida { get; set; }
    public decimal? AreaTerreno { get; set; }
    public int? Habitaciones { get; set; }
    public int? Banos { get; set; }
    public int? Garajes { get; set; }
    public int? AnoConstruccion { get; set; }
    public string? Coordenadas { get; set; }
    public List<string> FotosUrls { get; set; } = new();
}

public class UpdatePropiedadDto : CreatePropiedadDto
{
    public int IdPropiedad { get; set; }
}