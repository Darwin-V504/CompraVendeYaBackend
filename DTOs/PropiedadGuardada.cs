namespace CompraVendeYaBackend.DTOs;

public class PropiedadGuardadaDto
{
    public int IdGuardado { get; set; }
    public string PropiedadIdExterno { get; set; } = string.Empty;
    public string Titulo { get; set; } = string.Empty;
    public decimal Precio { get; set; }
    public string Direccion { get; set; } = string.Empty;
    public string Ciudad { get; set; } = string.Empty;
    public string Barrio { get; set; } = string.Empty;
    public int Habitaciones { get; set; }
    public int Banos { get; set; }
    public int Area { get; set; }
    public string UrlImagen { get; set; } = string.Empty;
    public string TipoOperacion { get; set; } = string.Empty;
    public string ClaseSocial { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public List<string>? Caracteristicas { get; set; }
    public DateTime FechaGuardado { get; set; }
}

public class GuardarPropiedadRequestDto
{
    public string PropiedadIdExterno { get; set; } = string.Empty;
    public string Titulo { get; set; } = string.Empty;
    public decimal Precio { get; set; }
    public string Direccion { get; set; } = string.Empty;
    public string Ciudad { get; set; } = string.Empty;
    public string Barrio { get; set; } = string.Empty;
    public int Habitaciones { get; set; }
    public int Banos { get; set; }
    public int Area { get; set; }
    public string UrlImagen { get; set; } = string.Empty;
    public string TipoOperacion { get; set; } = string.Empty;
    public string ClaseSocial { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public List<string>? Caracteristicas { get; set; }
}