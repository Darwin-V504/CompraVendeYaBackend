namespace CompraVendeYaBackend.Models;

public class Zona
{
    public int     IdZona      { get; set; }
    public string  Nombre      { get; set; } = string.Empty;
    public string  Ciudad      { get; set; } = string.Empty;
    public string? Descripcion { get; set; }

    public ICollection<Propiedad> Propiedades { get; set; } = new List<Propiedad>();
}