using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models.Entities;

public class SessionLog
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public DateTime FechaInicio { get; set; } = DateTime.UtcNow;

    public DateTime? FechaFin { get; set; }

    public string? RefreshToken { get; set; }

    [ForeignKey("UserId")]
    public User User { get; set; } = null!;
}
