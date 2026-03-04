using System.ComponentModel.DataAnnotations;

namespace backend.Models.DTOs;

public class LoginDTO
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;
}

public class AuthResponseDTO
{
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public UserResponseDTO User { get; set; } = null!;
}

public class RefreshTokenDTO
{
    [Required]
    public string RefreshToken { get; set; } = string.Empty;
}