using Microsoft.AspNetCore.Mvc;
using CompraVendeYaBackend.Services;
using CompraVendeYaBackend.DTOs;

namespace CompraVendeYaBackend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
    {
        try
        {
            var result = await _authService.RegisterAsync(request);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
    {
        try
        {
            var result = await _authService.LoginAsync(request);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("verify")]
    public async Task<IActionResult> Verify()
    {
        var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        var isValid = await _authService.VerifyTokenAsync(token);
        return Ok(new { valid = isValid });
    }

    [HttpGet("me")]
    public async Task<IActionResult> GetCurrentUser()
    {
        var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim))
            return Unauthorized();

        var user = await _authService.GetCurrentUserAsync(int.Parse(userIdClaim));
        if (user == null)
            return NotFound();

        return Ok(user);
    }
}