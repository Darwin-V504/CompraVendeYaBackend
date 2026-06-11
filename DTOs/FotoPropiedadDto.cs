namespace CompraVendeYaBackend.DTOs;

public class FotoPropiedadDto
{
    public int IdFoto { get; set; }
    public int IdPropiedad { get; set; }
    public string UrlFoto { get; set; } = string.Empty;
    public bool EsPrincipal { get; set; }
    public DateTime FechaSubida { get; set; }
}

public class CreateFotoPropiedadDto
{
    public string UrlFoto { get; set; } = string.Empty;
    public bool EsPrincipal { get; set; } = false;
}
