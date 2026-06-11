using System.ComponentModel.DataAnnotations;

namespace CompraVendeYaBackend.DTOs;

public class ClienteDto
{
    public int IdCliente { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Apellido { get; set; } = string.Empty;
    public string? Dni { get; set; }
    public string? Telefono { get; set; }
    public string? Email { get; set; }
    public string? TipoCliente { get; set; }
    public DateTime FechaRegistro { get; set; }
}

public class CreateClienteDto
{
    [Required] public string Nombre { get; set; } = string.Empty;
    [Required] public string Apellido { get; set; } = string.Empty;
    public string? Dni { get; set; }
    public string? Telefono { get; set; }
    [EmailAddress] public string? Email { get; set; }
    public string? TipoCliente { get; set; }
}