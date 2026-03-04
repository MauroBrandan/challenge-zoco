using backend.Models.DTOs;

namespace backend.Services.Interfaces;

public interface IUserService
{
    Task<IEnumerable<UserResponseDTO>> GetAllUsersAsync();
    Task<UserResponseDTO?> GetUserByIdAsync(int id);
    Task<UserResponseDTO> CreateUserAsync(CreateUserDTO dto);
    Task<UserResponseDTO?> UpdateUserAsync(int id, UpdateUserDTO dto);
    Task<bool> DeleteUserAsync(int id);
}