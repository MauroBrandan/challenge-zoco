using backend.Models.DTOs;
using backend.Models.Entities;
using backend.Repositories.Interfaces;
using backend.Services.Interfaces;

namespace backend.Services;

public class DireccionService : IDireccionService
{
    private readonly IDireccionRepository _direccionRepository;

    public DireccionService(IDireccionRepository direccionRepository)
    {
        _direccionRepository = direccionRepository;
    }

    public async Task<IEnumerable<DireccionResponseDTO>> GetDireccionesByUserIdAsync(int userId)
    {
        var direcciones = await _direccionRepository.GetAllByUserIdAsync(userId);
        return direcciones.Select(MapToResponseDTO);
    }

    public async Task<DireccionResponseDTO?> GetDireccionByIdAsync(int id)
    {
        var direccion = await _direccionRepository.GetByIdAsync(id);
        return direccion == null ? null : MapToResponseDTO(direccion);
    }

    public async Task<DireccionResponseDTO> CreateDireccionAsync(int userId, CreateDireccionDTO dto)
    {
        var direccion = new Direccion
        {
            Calle = dto.Calle,
            Ciudad = dto.Ciudad,
            Estado = dto.Estado,
            Pais = dto.Pais,
            CodigoPostal = dto.CodigoPostal,
            UserId = userId
        };

        var created = await _direccionRepository.CreateAsync(direccion);
        return MapToResponseDTO(created);
    }

    public async Task<DireccionResponseDTO?> UpdateDireccionAsync(int id, UpdateDireccionDTO dto)
    {
        var direccion = await _direccionRepository.GetByIdAsync(id);
        if (direccion == null) return null;

        direccion.Calle = dto.Calle;
        direccion.Ciudad = dto.Ciudad;
        direccion.Estado = dto.Estado;
        direccion.Pais = dto.Pais;
        direccion.CodigoPostal = dto.CodigoPostal;

        var updated = await _direccionRepository.UpdateAsync(direccion);
        return MapToResponseDTO(updated);
    }

    public async Task<bool> DeleteDireccionAsync(int id)
    {
        return await _direccionRepository.DeleteAsync(id);
    }

    private static DireccionResponseDTO MapToResponseDTO(Direccion direccion)
    {
        return new DireccionResponseDTO
        {
            Id = direccion.Id,
            Calle = direccion.Calle,
            Ciudad = direccion.Ciudad,
            Estado = direccion.Estado,
            Pais = direccion.Pais,
            CodigoPostal = direccion.CodigoPostal,
            UserId = direccion.UserId
        };
    }
}
