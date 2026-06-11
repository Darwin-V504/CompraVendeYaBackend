using System.ComponentModel.DataAnnotations;

namespace CompraVendeYaBackend.DTOs;

public class RegisterRequestDto
{
    [Required] public string Nombre { get; set; } = string.Empty;
    [Required] public string Apellido { get; set; } = string.Empty;
    [Required, EmailAddress] public string Email { get; set; } = string.Empty;
    [Required, MinLength(6)] public string Password { get; set; } = string.Empty;
    public string? Telefono { get; set; }
    public string? Rol { get; set; }
}

public class LoginRequestDto
{
    [Required, EmailAddress] public string Email { get; set; } = string.Empty;
    [Required] public string Password { get; set; } = string.Empty;
}

public class AuthResponseDto
{
    public string Token { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public UserDto User { get; set; } = null!;
}

public class RefreshRequestDto
{
    public string RefreshToken { get; set; } = string.Empty;
}

public class UserDto
{
    public int IdUsuario { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string Apellido { get; set; } = string.Empty;
    public string Rol { get; set; } = string.Empty;
    public string? Telefono { get; set; }
}