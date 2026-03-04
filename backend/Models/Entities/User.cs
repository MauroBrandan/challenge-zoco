using System.ComponentModel.DataAnnotations;

namespace backend.Models.Entities;

public class User
{
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string Nombre { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string Apellido { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string PasswordHash { get; set; } = string.Empty;

    public Role Rol { get; set; } = Role.User;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<Estudio> Estudios { get; set; } = new List<Estudio>();
    public ICollection<Direccion> Direcciones { get; set; } = new List<Direccion>();
    public ICollection<SessionLog> SessionLogs { get; set; } = new List<SessionLog>();
}
