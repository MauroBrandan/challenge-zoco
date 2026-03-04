using backend.Helpers;
using backend.Models.DTOs;
using backend.Models.Entities;
using backend.Repositories.Interfaces;
using backend.Services.Interfaces;

namespace backend.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly ISessionRepository _sessionRepository;
    private readonly JwtHelper _jwtHelper;

    public AuthService(
        IUserRepository userRepository,
        ISessionRepository sessionRepository,
        JwtHelper jwtHelper)
    {
        _userRepository = userRepository;
        _sessionRepository = sessionRepository;
        _jwtHelper = jwtHelper;
    }

    public async Task<AuthResponseDTO?> LoginAsync(LoginDTO dto)
    {
        var user = await _userRepository.GetByEmailAsync(dto.Email);
        if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            return null;

        var accessToken = _jwtHelper.GenerateAccessToken(user);
        var refreshToken = _jwtHelper.GenerateRefreshToken();

        await _sessionRepository.CreateAsync(new SessionLog
        {
            UserId = user.Id,
            RefreshToken = refreshToken,
            FechaInicio = DateTime.UtcNow
        });

        return new AuthResponseDTO
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            User = new UserResponseDTO
            {
                Id = user.Id,
                Nombre = user.Nombre,
                Apellido = user.Apellido,
                Email = user.Email,
                Rol = user.Rol,
                CreatedAt = user.CreatedAt
            }
        };
    }

    public async Task<bool> LogoutAsync(int userId, string refreshToken)
    {
        var session = await _sessionRepository.GetActiveByRefreshTokenAsync(refreshToken);
        if (session == null || session.UserId != userId) return false;

        await _sessionRepository.EndSessionAsync(session);
        return true;
    }

    public async Task<AuthResponseDTO?> RefreshTokenAsync(string refreshToken)
    {
        var session = await _sessionRepository.GetActiveByRefreshTokenAsync(refreshToken);
        if (session == null) return null;

        var user = session.User;

        // Closing old session
        await _sessionRepository.EndSessionAsync(session);

        // Creating new session with new refresh token
        var newAccessToken = _jwtHelper.GenerateAccessToken(user);
        var newRefreshToken = _jwtHelper.GenerateRefreshToken();

        await _sessionRepository.CreateAsync(new SessionLog
        {
            UserId = user.Id,
            RefreshToken = newRefreshToken,
            FechaInicio = DateTime.UtcNow
        });

        return new AuthResponseDTO
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken,
            User = new UserResponseDTO
            {
                Id = user.Id,
                Nombre = user.Nombre,
                Apellido = user.Apellido,
                Email = user.Email,
                Rol = user.Rol,
                CreatedAt = user.CreatedAt
            }
        };
    }
}