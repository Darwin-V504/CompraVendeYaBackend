namespace CompraVendeYaBackend.Models;

public class Usuario
{
    public int IdUsuario { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Apellido { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string Rol { get; set; } = string.Empty;
    public string? Telefono { get; set; }
    public string? FotoPerfil { get; set; }
    public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;
    public DateTime? UltimoLogin { get; set; }
    public bool Activo { get; set; } = true;

    // Navigation properties
    public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    public ICollection<Propiedad> PropiedadesAsignadas { get; set; } = new List<Propiedad>();
    public ICollection<Transaccion> Transacciones { get; set; } = new List<Transaccion>();
}