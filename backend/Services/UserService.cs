using backend.Models.DTOs;
using backend.Models.Entities;
using backend.Repositories.Interfaces;
using backend.Services.Interfaces;

namespace backend.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<IEnumerable<UserResponseDTO>> GetAllUsersAsync()
    {
        var users = await _userRepository.GetAllAsync();
        return users.Select(MapToResponseDTO);
    }

    public async Task<UserResponseDTO?> GetUserByIdAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        return user == null ? null : MapToResponseDTO(user);
    }

    public async Task<UserResponseDTO> CreateUserAsync(CreateUserDTO dto)
    {
        // Verify email uniqueness
        if (await _userRepository.EmailExistsAsync(dto.Email))
            throw new InvalidOperationException("El email ya está registrado.");

        var user = new User
        {
            Nombre = dto.Nombre,
            Apellido = dto.Apellido,
            Email = dto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            Rol = dto.Rol
        };

        var created = await _userRepository.CreateAsync(user);
        return MapToResponseDTO(created);
    }

    public async Task<UserResponseDTO?> UpdateUserAsync(int id, UpdateUserDTO dto)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null) return null;

        // Verify email uniqueness (excluding the current user)
        if (await _userRepository.EmailExistsAsync(dto.Email, id))
            throw new InvalidOperationException("El email ya está en uso.");

        user.Nombre = dto.Nombre;
        user.Apellido = dto.Apellido;
        user.Email = dto.Email;

        var updated = await _userRepository.UpdateAsync(user);
        return MapToResponseDTO(updated);
    }

    public async Task<bool> DeleteUserAsync(int id)
    {
        return await _userRepository.DeleteAsync(id);
    }

    // Helper to map Entity → DTO
    private static UserResponseDTO MapToResponseDTO(User user)
    {
        return new UserResponseDTO
        {
            Id = user.Id,
            Nombre = user.Nombre,
            Apellido = user.Apellido,
            Email = user.Email,
            Rol = user.Rol,
            CreatedAt = user.CreatedAt
        };
    }
}