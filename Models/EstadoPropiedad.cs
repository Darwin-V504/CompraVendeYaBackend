using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CompraVendeYaBackend.Models;

[Table("estadopropiedad")]
public class EstadoPropiedad
{
    [Key]
    [Column("id_estado")]
    public int IdEstado { get; set; }

    [Column("nombre")]
    public string Nombre { get; set; } = string.Empty;

    // Navigation
    public ICollection<Propiedad> Propiedades { get; set; } = new List<Propiedad>();
}