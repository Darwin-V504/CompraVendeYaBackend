using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CompraVendeYaBackend.Models;

[Table("propiedadguardada")]  // ← Nombre en minúsculas, sin guión bajo
public class PropiedadGuardada
{
    [Key]
    [Column("id_guardado")]
    public int IdGuardado { get; set; }

    [Column("usuario_id")]
    public int UsuarioId { get; set; }

    [Column("propiedad_id_externo")]
    public string PropiedadIdExterno { get; set; } = string.Empty;

    [Column("titulo")]
    public string Titulo { get; set; } = string.Empty;

    [Column("precio")]
    public decimal Precio { get; set; }

    [Column("direccion")]
    public string? Direccion { get; set; }

    [Column("ciudad")]
    public string? Ciudad { get; set; }

    [Column("barrio")]
    public string? Barrio { get; set; }

    [Column("habitaciones")]
    public int Habitaciones { get; set; }

    [Column("banos")]
    public int Banos { get; set; }

    [Column("area")]
    public int Area { get; set; }

    [Column("url_imagen")]
    public string? UrlImagen { get; set; }

    [Column("tipo_operacion")]
    public string? TipoOperacion { get; set; }

    [Column("clase_social")]
    public string? ClaseSocial { get; set; }

    [Column("descripcion")]
    public string? Descripcion { get; set; }

    [Column("caracteristicas")]
    public string? Caracteristicas { get; set; }

    [Column("fecha_guardado")]
    public DateTime FechaGuardado { get; set; } = DateTime.UtcNow;

    [ForeignKey("UsuarioId")]
    public Usuario Usuario { get; set; } = null!;
}