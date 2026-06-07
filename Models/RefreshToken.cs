using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CompraVendeYaBackend.Models;

[Table("refreshtoken")]
public class RefreshToken
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("usuario_id")]
    public int UsuarioId { get; set; }

    [Column("token")]
    public string Token { get; set; } = string.Empty;

    [Column("expires_at")]
    public DateTime ExpiresAt { get; set; }

    [Column("creado_en")]
    public DateTime CreadoEn { get; set; } = DateTime.UtcNow;

    [Column("revocado")]
    public bool Revocado { get; set; } = false;

    // Navigation
    [ForeignKey("UsuarioId")]
    public Usuario Usuario { get; set; } = null!;
}