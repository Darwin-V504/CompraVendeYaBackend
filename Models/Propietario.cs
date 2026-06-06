namespace CompraVendeYaBackend.Models;

public class Propietario
{
    public int IdPropietario { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Apellido { get; set; } = string.Empty;
    public string? Dni { get; set; }
    public string? Telefono { get; set; }
    public string? Email { get; set; }
    public string? Direccion { get; set; }
    public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public ICollection<Propiedad> Propiedades { get; set; } = new List<Propiedad>();
}