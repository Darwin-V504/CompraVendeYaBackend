namespace CompraVendeYaBackend.DTOs;

public class ZonaDto
{
    public int IdZona { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Ciudad { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
}

public class CreateZonaDto
{
    public string Nombre { get; set; } = string.Empty;
    public string Ciudad { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
}