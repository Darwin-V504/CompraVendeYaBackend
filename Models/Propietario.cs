using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CompraVendeYaBackend.Models;

[Table("propietario")]
public class Propietario
{
    [Key]
    [Column("id_propietario")]
    public int IdPropietario { get; set; }

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

    [Column("direccion")]
    public string? Direccion { get; set; }

    [Column("fecha_registro")]
    public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;

    // Navigation
    public ICollection<Propiedad> Propiedades { get; set; } = new List<Propiedad>();
}