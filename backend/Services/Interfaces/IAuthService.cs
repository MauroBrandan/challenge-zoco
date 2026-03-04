using backend.Models.DTOs;

namespace backend.Services.Interfaces;

public interface IAuthService
{
    Task<AuthResponseDTO?> LoginAsync(LoginDTO dto);
    Task<bool> LogoutAsync(int userId, string refreshToken);
    Task<AuthResponseDTO?> RefreshTokenAsync(string refreshToken);
}