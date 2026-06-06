namespace CompraVendeYaBackend.Models;

public class Operacion
{
    public int IdOperacion { get; set; }
    public string Nombre { get; set; } = string.Empty; // Venta, Alquiler

    // Navigation properties
    public ICollection<Propiedad> Propiedades { get; set; } = new List<Propiedad>();
    public ICollection<Transaccion> Transacciones { get; set; } = new List<Transaccion>();
}