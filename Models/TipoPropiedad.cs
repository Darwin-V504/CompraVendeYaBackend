using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CompraVendeYaBackend.Models;

[Table("tipopropiedad")]
public class TipoPropiedad
{
    [Key]
    [Column("id_tipo")]
    public int IdTipo { get; set; }

    [Column("nombre")]
    public string Nombre { get; set; } = string.Empty;

    [Column("descripcion")]
    public string? Descripcion { get; set; }

    // Navigation
    public ICollection<Propiedad> Propiedades { get; set; } = new List<Propiedad>();
}