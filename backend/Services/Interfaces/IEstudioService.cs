using backend.Models.DTOs;

namespace backend.Services.Interfaces;

public interface IEstudioService
{
    Task<IEnumerable<EstudioResponseDTO>> GetEstudiosByUserIdAsync(int userId);
    Task<EstudioResponseDTO?> GetEstudioByIdAsync(int id);
    Task<EstudioResponseDTO> CreateEstudioAsync(int userId, CreateEstudioDTO dto);
    Task<EstudioResponseDTO?> UpdateEstudioAsync(int id, UpdateEstudioDTO dto);
    Task<bool> DeleteEstudioAsync(int id);
}
