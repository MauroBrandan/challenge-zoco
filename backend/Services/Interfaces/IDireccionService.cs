using backend.Models.DTOs;

namespace backend.Services.Interfaces;

public interface IDireccionService
{
    Task<IEnumerable<DireccionResponseDTO>> GetDireccionesByUserIdAsync(int userId);
    Task<DireccionResponseDTO?> GetDireccionByIdAsync(int id);
    Task<DireccionResponseDTO> CreateDireccionAsync(int userId, CreateDireccionDTO dto);
    Task<DireccionResponseDTO?> UpdateDireccionAsync(int id, UpdateDireccionDTO dto);
    Task<bool> DeleteDireccionAsync(int id);
}
