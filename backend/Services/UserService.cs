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

    private async Task<bool> IsLastAdminAsync(int userId)
    {
        var users = await _userRepository.GetAllAsync();
        var admins = users.Where(u => u.Rol == Role.Admin).ToList();
        return admins.Count == 1 && admins[0].Id == userId;
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

    public async Task<UserDetailResponseDTO?> GetUserDetailByIdAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        return user == null ? null : MapToDetailResponseDTO(user);
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
        if (await IsLastAdminAsync(id))
            throw new InvalidOperationException("No se puede eliminar al único administrador del sistema.");

        return await _userRepository.DeleteAsync(id);
    }

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

    private static UserDetailResponseDTO MapToDetailResponseDTO(User user)
    {
        return new UserDetailResponseDTO
        {
            Id = user.Id,
            Nombre = user.Nombre,
            Apellido = user.Apellido,
            Email = user.Email,
            Rol = user.Rol,
            CreatedAt = user.CreatedAt,
            Estudios = user.Estudios.Select(e => new EstudioResponseDTO
            {
                Id = e.Id,
                Institucion = e.Institucion,
                Titulo = e.Titulo,
                NivelEstudio = e.NivelEstudio.ToString(),
                FechaInicio = e.FechaInicio,
                FechaFin = e.FechaFin,
                UserId = e.UserId
            }).ToList(),
            Direcciones = user.Direcciones.Select(d => new DireccionResponseDTO
            {
                Id = d.Id,
                Calle = d.Calle,
                Ciudad = d.Ciudad,
                Estado = d.Estado,
                Pais = d.Pais,
                CodigoPostal = d.CodigoPostal,
                UserId = d.UserId
            }).ToList()
        };
    }
}