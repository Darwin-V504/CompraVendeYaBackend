namespace CompraVendeYaBackend.Models;

public class EstadoPropiedad
{
    public int    IdEstado { get; set; }
    public string Nombre   { get; set; } = string.Empty;

    public ICollection<Propiedad> Propiedades { get; set; } = new List<Propiedad>();
}