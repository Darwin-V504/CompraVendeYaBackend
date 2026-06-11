using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
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
        if (string.IsNullOrEmpty(_configuration["Jwt:Key"]))
            throw new InvalidOperationException("Jwt:Key no está configurada.");
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
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            Rol = request.Rol ?? "Agente",
            Telefono = request.Telefono,
            FechaRegistro = DateTime.UtcNow,
            Activo = true
        };

        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();

        var accessToken = GenerateJwtToken(usuario);
        var refreshToken = await CreateRefreshTokenAsync(usuario.IdUsuario);

        return BuildAuthResponse(accessToken, refreshToken.Token, usuario);
    }

    public async Task<AuthResponseDto> LoginAsync(LoginRequestDto request)
    {
        var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == request.Email);
        if (usuario == null)
            throw new Exception("Email o contraseña incorrectos");

        if (!BCrypt.Net.BCrypt.Verify(request.Password, usuario.PasswordHash))
            throw new Exception("Email o contraseña incorrectos");

        usuario.UltimoLogin = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        var accessToken = GenerateJwtToken(usuario);
        var refreshToken = await CreateRefreshTokenAsync(usuario.IdUsuario);

        return BuildAuthResponse(accessToken, refreshToken.Token, usuario);
    }

    public async Task<AuthResponseDto> RefreshAsync(string refreshTokenValue)
    {
        var stored = await _context.RefreshTokens
            .Include(r => r.Usuario)
            .FirstOrDefaultAsync(r => r.Token == refreshTokenValue);

        if (stored == null || stored.Revocado || stored.ExpiresAt <= DateTime.UtcNow)
            throw new Exception("Refresh token inválido o expirado");

        stored.Revocado = true;
        await _context.SaveChangesAsync();

        var accessToken = GenerateJwtToken(stored.Usuario);
        var newRefresh = await CreateRefreshTokenAsync(stored.UsuarioId);

        return BuildAuthResponse(accessToken, newRefresh.Token, stored.Usuario);
    }

    public async Task LogoutAsync(string refreshTokenValue)
    {
        var stored = await _context.RefreshTokens
            .FirstOrDefaultAsync(r => r.Token == refreshTokenValue);

        if (stored != null && !stored.Revocado)
        {
            stored.Revocado = true;
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> VerifyTokenAsync(string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!);

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

    private async Task<RefreshToken> CreateRefreshTokenAsync(int usuarioId)
    {
        var token = new RefreshToken
        {
            UsuarioId = usuarioId,
            Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
            ExpiresAt = DateTime.UtcNow.AddDays(30),
            CreadoEn = DateTime.UtcNow,
            Revocado = false
        };
        _context.RefreshTokens.Add(token);
        await _context.SaveChangesAsync();
        return token;
    }

    private string GenerateJwtToken(Usuario usuario)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString()),
                new Claim(ClaimTypes.Email, usuario.Email),
                new Claim(ClaimTypes.Role, usuario.Rol),
                new Claim("NombreCompleto", $"{usuario.Nombre} {usuario.Apellido}")
            }),
            Expires = DateTime.UtcNow.AddHours(1),
            Issuer = _configuration["Jwt:Issuer"],
            Audience = _configuration["Jwt:Audience"],
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    private static AuthResponseDto BuildAuthResponse(string accessToken, string refreshToken, Usuario usuario) =>
        new()
        {
            Token = accessToken,
            RefreshToken = refreshToken,
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
