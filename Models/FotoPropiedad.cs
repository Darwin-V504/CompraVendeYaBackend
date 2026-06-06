namespace CompraVendeYaBackend.Models;

public class FotoPropiedad
{
    public int      IdFoto       { get; set; }
    public int      IdPropiedad  { get; set; }
    public string   UrlFoto      { get; set; } = string.Empty;
    public bool     EsPrincipal  { get; set; } = false;
    public DateTime FechaSubida  { get; set; } = DateTime.UtcNow;

    // Navigation
    public Propiedad Propiedad { get; set; } = null!;
}