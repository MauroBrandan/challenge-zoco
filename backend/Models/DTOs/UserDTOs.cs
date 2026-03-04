using System.ComponentModel.DataAnnotations;
using backend.Models.Entities;

namespace backend.Models.DTOs;

public class CreateUserDTO
{
    [Required]
    [MaxLength(50)]
    public string Nombre { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string Apellido { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [MaxLength(100)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MinLength(6)]
    public string Password { get; set; } = string.Empty;

    public Role Rol { get; set; } = Role.User;
}

public class UpdateUserDTO
{
    [Required]
    [MaxLength(50)]
    public string Nombre { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string Apellido { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [MaxLength(100)]
    public string Email { get; set; } = string.Empty;
}

public class UserResponseDTO
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Apellido { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public Role Rol { get; set; }
    public DateTime CreatedAt { get; set; }
}