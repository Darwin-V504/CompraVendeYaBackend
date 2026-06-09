using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CompraVendeYaBackend.Models;

[Table("fotopropiedad")]
public class FotoPropiedad
{
    [Key]
    [Column("id_foto")]
    public int IdFoto { get; set; }

    [Column("id_propiedad")]
    public int IdPropiedad { get; set; }

    [Column("url_foto")]
    public string UrlFoto { get; set; } = string.Empty;

    [Column("es_principal")]
    public bool EsPrincipal { get; set; } = false;

    [Column("fecha_subida")]
    public DateTime FechaSubida { get; set; } = DateTime.UtcNow;

    // Navigation
    [ForeignKey("IdPropiedad")]
    public Propiedad Propiedad { get; set; } = null!;
}