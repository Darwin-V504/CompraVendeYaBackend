namespace CompraVendeYaBackend.Models;

public class RefreshToken
{
    public int      Id        { get; set; }
    public int      UsuarioId { get; set; }
    public string   Token     { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
    public DateTime CreadoEn  { get; set; } = DateTime.UtcNow;
    public bool     Revocado  { get; set; } = false;

    // Navigation
    public Usuario Usuario { get; set; } = null!;
}