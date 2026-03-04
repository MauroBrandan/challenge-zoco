using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models.Entities;

public class Direccion
{
    public int Id { get; set; }

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

    public int UserId { get; set; }

    [ForeignKey("UserId")]
    public User User { get; set; } = null!;
}
