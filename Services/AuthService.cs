using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CompraVendeYaBackend.Data;
using CompraVendeYaBackend.DTOs;
using CompraVendeYaBackend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace CompraVendeYaBackend.Services;

public class AuthService
{
    private readonly BienesRaicesDbContext _context;
    private readonly IConfiguration _configuration;

    public AuthService(BienesRaicesDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request)
    {
        var existingUser = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == request.Email);
        if (existingUser != null)
            throw new Exception("El email ya está registrado");

        var usuario = new Usuario
        {
            Nombre = request.Nombre,
            Apellido = request.Apellido,
            Email = request.Email,
            PasswordHash = request.Password, // Temporal: guarda la contraseña sin hash
            Rol = request.Rol ?? "Agente",
            Telefono = request.Telefono,
            FechaRegistro = DateTime.UtcNow,
            Activo = true
        };

        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();

        var token = GenerateJwtToken(usuario);

        return new AuthResponseDto
        {
            Token = token,
            User = new UserDto
            {
                IdUsuario = usuario.IdUsuario,
                Email = usuario.Email,
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido,
                Rol = usuario.Rol,
                Telefono = usuario.Telefono
            }
        };
    }

    public async Task<AuthResponseDto> LoginAsync(LoginRequestDto request)
    {
        var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == request.Email);
        if (usuario == null)
            throw new Exception("Email o contraseña incorrectos");

        if (usuario.PasswordHash != request.Password)
            throw new Exception("Email o contraseña incorrectos");

        usuario.UltimoLogin = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        var token = GenerateJwtToken(usuario);

        return new AuthResponseDto
        {
            Token = token,
            User = new UserDto
            {
                IdUsuario = usuario.IdUsuario,
                Email = usuario.Email,
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido,
                Rol = usuario.Rol,
                Telefono = usuario.Telefono
            }
        };
    }

    public async Task<bool> VerifyTokenAsync(string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? "CompraVendeYa1234567890SecureKey2024!@#$%");

            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = _configuration["Jwt:Issuer"],
                ValidateAudience = true,
                ValidAudience = _configuration["Jwt:Audience"],
                ClockSkew = TimeSpan.Zero
            }, out _);

            return await Task.FromResult(true);
        }
        catch
        {
            return false;
        }
    }

    public async Task<UserDto?> GetCurrentUserAsync(int userId)
    {
        var usuario = await _context.Usuarios.FindAsync(userId);
        if (usuario == null) return null;

        return new UserDto
        {
            IdUsuario = usuario.IdUsuario,
            Email = usuario.Email,
            Nombre = usuario.Nombre,
            Apellido = usuario.Apellido,
            Rol = usuario.Rol,
            Telefono = usuario.Telefono
        };
    }

    private string GenerateJwtToken(Usuario usuario)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? "CompraVendeYa1234567890SecureKey2024!@#$%");

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString()),
                new Claim(ClaimTypes.Email, usuario.Email),
                new Claim(ClaimTypes.Role, usuario.Rol),
                new Claim("NombreCompleto", $"{usuario.Nombre} {usuario.Apellido}")
            }),
            Expires = DateTime.UtcNow.AddDays(7),
            Issuer = _configuration["Jwt:Issuer"],
            Audience = _configuration["Jwt:Audience"],
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}