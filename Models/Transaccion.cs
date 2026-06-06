using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CompraVendeYaBackend.Models;

[Table("transaccion")]
public class Transaccion
{
    [Key]
    [Column("id_transaccion")]
    public int IdTransaccion { get; set; }

    [Column("id_propiedad")]
    public int? IdPropiedad { get; set; }

    [Column("id_cliente")]
    public int? IdCliente { get; set; }

    [Column("id_agente")]
    public int? IdAgente { get; set; }

    [Column("id_operacion")]
    public int? IdOperacion { get; set; }

    [Column("fecha_transaccion")]
    public DateOnly FechaTransaccion { get; set; }

    [Column("monto_total")]
    public decimal MontoTotal { get; set; }

    [Column("comision_agente")]
    public decimal? ComisionAgente { get; set; }

    [Column("estado_transaccion")]
    public string? EstadoTransaccion { get; set; }

    [Column("detalles")]
    public string? Detalles { get; set; }

    [Column("fecha_registro")]
    public DateTime? FechaRegistro { get; set; }

    // Navigation properties
    public Propiedad? Propiedad { get; set; }
    public Cliente? Cliente { get; set; }
    public Usuario? Agente { get; set; }
    public Operacion? Operacion { get; set; }
}