namespace CompraVendeYaBackend.Models;

public class Cliente
{
    public int      IdCliente     { get; set; }
    public string   Nombre        { get; set; } = string.Empty;
    public string   Apellido      { get; set; } = string.Empty;
    public string?  Dni           { get; set; }
    public string?  Telefono      { get; set; }
    public string?  Email         { get; set; }
    public string?  TipoCliente   { get; set; } // Comprador | Inquilino | Ambos
    public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;

    public ICollection<Transaccion> Transacciones { get; set; } = new List<Transaccion>();
}