using System.ComponentModel.DataAnnotations;
using backend.Models.Entities;

namespace backend.Models.DTOs;

public class CreateEstudioDTO
{
    [Required]
    [MaxLength(150)]
    public string Institucion { get; set; } = string.Empty;

    [Required]
    [MaxLength(150)]
    public string Titulo { get; set; } = string.Empty;

    public NivelEstudio NivelEstudio { get; set; }

    public DateTime FechaInicio { get; set; }

    public DateTime? FechaFin { get; set; }
}

public class UpdateEstudioDTO
{
    [Required]
    [MaxLength(150)]
    public string Institucion { get; set; } = string.Empty;

    [Required]
    [MaxLength(150)]
    public string Titulo { get; set; } = string.Empty;

    public NivelEstudio NivelEstudio { get; set; }

    public DateTime FechaInicio { get; set; }

    public DateTime? FechaFin { get; set; }
}

public class EstudioResponseDTO
{
    public int Id { get; set; }
    public string Institucion { get; set; } = string.Empty;
    public string Titulo { get; set; } = string.Empty;
    public string NivelEstudio { get; set; } = string.Empty;
    public DateTime FechaInicio { get; set; }
    public DateTime? FechaFin { get; set; }
    public int UserId { get; set; }
}
