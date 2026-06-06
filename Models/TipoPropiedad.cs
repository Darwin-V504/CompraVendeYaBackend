namespace CompraVendeYaBackend.Models;

public class TipoPropiedad
{
    public int IdTipo { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }

    // Navigation property
    public ICollection<Propiedad> Propiedades { get; set; } = new List<Propiedad>();
}