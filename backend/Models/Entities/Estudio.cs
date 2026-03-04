using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models.Entities;

public class Estudio
{
    public int Id { get; set; }

    [Required]
    [MaxLength(150)]
    public string Institucion { get; set; } = string.Empty;

    [Required]
    [MaxLength(150)]
    public string Titulo { get; set; } = string.Empty;

    public NivelEstudio NivelEstudio { get; set; }

    public DateTime FechaInicio { get; set; }

    public DateTime? FechaFin { get; set; }

    public int UserId { get; set; }

    [ForeignKey("UserId")]
    public User User { get; set; } = null!;
}
