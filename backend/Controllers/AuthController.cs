using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using backend.Models.DTOs;
using backend.Services.Interfaces;

namespace backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    // POST /api/auth/login
    [HttpPost("login")]
    public async Task<ActionResult<AuthResponseDTO>> Login([FromBody] LoginDTO dto)
    {
        var result = await _authService.LoginAsync(dto);
        if (result == null)
            return Unauthorized(new { message = "Credenciales inválidas." });

        return Ok(result);
    }

    // POST /api/auth/logout
    [Authorize]
    [HttpPost("logout")]
    public async Task<ActionResult> Logout([FromBody] RefreshTokenDTO dto)
    {
        // Get userId from JWT token
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userIdClaim == null) return Unauthorized();

        var userId = int.Parse(userIdClaim);
        var result = await _authService.LogoutAsync(userId, dto.RefreshToken);

        if (!result) return BadRequest(new { message = "Sesión no encontrada." });
        return Ok(new { message = "Sesión cerrada correctamente." });
    }

    // POST /api/auth/refresh
    [HttpPost("refresh")]
    public async Task<ActionResult<AuthResponseDTO>> Refresh([FromBody] RefreshTokenDTO dto)
    {
        var result = await _authService.RefreshTokenAsync(dto.RefreshToken);
        if (result == null)
            return Unauthorized(new { message = "Refresh token inválido o expirado." });

        return Ok(result);
    }
}