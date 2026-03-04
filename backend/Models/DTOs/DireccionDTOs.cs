using System.ComponentModel.DataAnnotations;

namespace backend.Models.DTOs;

public class CreateDireccionDTO
{
    [Required]
    [MaxLength(200)]
    public string Calle { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string Ciudad { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string Estado { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string Pais { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    public string CodigoPostal { get; set; } = string.Empty;
}

public class UpdateDireccionDTO
{
    [Required]
    [MaxLength(200)]
    public string Calle { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string Ciudad { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string Estado { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string Pais { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    public string CodigoPostal { get; set; } = string.Empty;
}

public class DireccionResponseDTO
{
    public int Id { get; set; }
    public string Calle { get; set; } = string.Empty;
    public string Ciudad { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;
    public string Pais { get; set; } = string.Empty;
    public string CodigoPostal { get; set; } = string.Empty;
    public int UserId { get; set; }
}
