using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CompraVendeYaBackend.Models;

[Table("cliente")]
public class Cliente
{
    [Key]
    [Column("id_cliente")]
    public int IdCliente { get; set; }

    [Column("nombre")]
    public string Nombre { get; set; } = string.Empty;

    [Column("apellido")]
    public string Apellido { get; set; } = string.Empty;

    [Column("dni")]
    public string? Dni { get; set; }

    [Column("telefono")]
    public string? Telefono { get; set; }

    [Column("email")]
    public string? Email { get; set; }

    [Column("tipo_cliente")]
    public string? TipoCliente { get; set; }  // Comprador, Inquilino, Ambos

    [Column("fecha_registro")]
    public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;

    // Navigation
    public ICollection<Transaccion> Transacciones { get; set; } = new List<Transaccion>();
}