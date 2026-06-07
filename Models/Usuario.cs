using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CompraVendeYaBackend.Models;

[Table("usuario")]
public class Usuario
{
    [Key]
    [Column("id_usuario")]
    public int IdUsuario { get; set; }

    [Column("nombre")]
    public string Nombre { get; set; } = string.Empty;

    [Column("apellido")]
    public string Apellido { get; set; } = string.Empty;

    [Column("email")]
    public string Email { get; set; } = string.Empty;

    [Column("password_hash")]
    public string PasswordHash { get; set; } = string.Empty;

    [Column("rol")]
    public string Rol { get; set; } = string.Empty;  // Admin, Agente, Propietario

    [Column("telefono")]
    public string? Telefono { get; set; }

    [Column("foto_perfil")]
    public string? FotoPerfil { get; set; }

    [Column("fecha_registro")]
    public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;

    [Column("ultimo_login")]
    public DateTime? UltimoLogin { get; set; }

    [Column("activo")]
    public bool Activo { get; set; } = true;

    // Navigation
    public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    public ICollection<Propiedad> PropiedadesAsignadas { get; set; } = new List<Propiedad>();
    public ICollection<Transaccion> Transacciones { get; set; } = new List<Transaccion>();
}