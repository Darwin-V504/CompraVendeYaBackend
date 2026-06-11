namespace CompraVendeYaBackend.DTOs;

public class TipoPropiedadDto
{
    public int IdTipo { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
}

public class EstadoPropiedadDto
{
    public int IdEstado { get; set; }
    public string Nombre { get; set; } = string.Empty;
}

public class OperacionDto
{
    public int IdOperacion { get; set; }
    public string Nombre { get; set; } = string.Empty;
}
