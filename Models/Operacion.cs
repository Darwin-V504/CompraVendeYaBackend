using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CompraVendeYaBackend.Models;

[Table("operacion")]
public class Operacion
{
    [Key]
    [Column("id_operacion")]
    public int IdOperacion { get; set; }

    [Column("nombre")]
    public string Nombre { get; set; } = string.Empty;  // Venta, Alquiler

    // Navigation
    public ICollection<Propiedad> Propiedades { get; set; } = new List<Propiedad>();
    public ICollection<Transaccion> Transacciones { get; set; } = new List<Transaccion>();
}