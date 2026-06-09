using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CompraVendeYaBackend.Models;

[Table("zona")]
public class Zona
{
    [Key]
    [Column("id_zona")]
    public int IdZona { get; set; }

    [Column("nombre")]
    public string Nombre { get; set; } = string.Empty;

    [Column("ciudad")]
    public string Ciudad { get; set; } = string.Empty;

    [Column("descripcion")]
    public string? Descripcion { get; set; }

    // Navigation
    public ICollection<Propiedad> Propiedades { get; set; } = new List<Propiedad>();
}