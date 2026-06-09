using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CompraVendeYaBackend.Models;

[Table("propiedad")]
public class Propiedad
{
    [Key]
    [Column("id_propiedad")]
    public int IdPropiedad { get; set; }

    [Column("id_tipo")]
    public int? IdTipo { get; set; }

    [Column("id_zona")]
    public int? IdZona { get; set; }

    [Column("id_propietario")]
    public int? IdPropietario { get; set; }

    [Column("id_agente")]
    public int? IdAgente { get; set; }

    [Column("id_estado")]
    public int? IdEstado { get; set; }

    [Column("id_operacion")]
    public int? IdOperacion { get; set; }

    [Column("titulo")]
    public string Titulo { get; set; } = string.Empty;

    [Column("descripcion")]
    public string? Descripcion { get; set; }

    [Column("precio")]
    public decimal Precio { get; set; }

    [Column("direccion")]
    public string Direccion { get; set; } = string.Empty;

    [Column("area_construida")]
    public decimal? AreaConstruida { get; set; }

    [Column("area_terreno")]
    public decimal? AreaTerreno { get; set; }

    [Column("habitaciones")]
    public int? Habitaciones { get; set; }

    [Column("banos")]
    public int? Banos { get; set; }

    [Column("garajes")]
    public int? Garajes { get; set; }

    [Column("ano_construccion")]
    public int? AnoConstruccion { get; set; }

    [Column("coordenadas")]
    public string? Coordenadas { get; set; }

    [Column("fecha_publicacion")]
    public DateOnly FechaPublicacion { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);

    [Column("fecha_actualizacion")]
    public DateTime FechaActualizacion { get; set; } = DateTime.UtcNow;

    // Navigation properties
    [ForeignKey("IdTipo")]
    public TipoPropiedad? TipoPropiedad { get; set; }

    [ForeignKey("IdZona")]
    public Zona? Zona { get; set; }

    [ForeignKey("IdPropietario")]
    public Propietario? Propietario { get; set; }

    [ForeignKey("IdAgente")]
    public Usuario? Agente { get; set; }

    [ForeignKey("IdEstado")]
    public EstadoPropiedad? Estado { get; set; }

    [ForeignKey("IdOperacion")]
    public Operacion? Operacion { get; set; }

    public ICollection<FotoPropiedad> Fotos { get; set; } = new List<FotoPropiedad>();
    public ICollection<Transaccion> Transacciones { get; set; } = new List<Transaccion>();
}