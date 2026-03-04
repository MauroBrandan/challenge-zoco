using backend.Models.DTOs;
using backend.Models.Entities;
using backend.Repositories.Interfaces;
using backend.Services.Interfaces;

namespace backend.Services;

public class EstudioService : IEstudioService
{
    private readonly IEstudioRepository _estudioRepository;

    public EstudioService(IEstudioRepository estudioRepository)
    {
        _estudioRepository = estudioRepository;
    }

    public async Task<IEnumerable<EstudioResponseDTO>> GetEstudiosByUserIdAsync(int userId)
    {
        var estudios = await _estudioRepository.GetAllByUserIdAsync(userId);
        return estudios.Select(MapToResponseDTO);
    }

    public async Task<EstudioResponseDTO?> GetEstudioByIdAsync(int id)
    {
        var estudio = await _estudioRepository.GetByIdAsync(id);
        return estudio == null ? null : MapToResponseDTO(estudio);
    }

    public async Task<EstudioResponseDTO> CreateEstudioAsync(int userId, CreateEstudioDTO dto)
    {
        if (dto.FechaFin.HasValue && dto.FechaFin.Value <= dto.FechaInicio)
            throw new InvalidOperationException("La fecha de fin debe ser posterior a la fecha de inicio.");

        var estudio = new Estudio
        {
            Institucion = dto.Institucion,
            Titulo = dto.Titulo,
            NivelEstudio = dto.NivelEstudio,
            FechaInicio = dto.FechaInicio,
            FechaFin = dto.FechaFin,
            UserId = userId
        };

        var created = await _estudioRepository.CreateAsync(estudio);
        return MapToResponseDTO(created);
    }

    public async Task<EstudioResponseDTO?> UpdateEstudioAsync(int id, UpdateEstudioDTO dto)
    {
        var estudio = await _estudioRepository.GetByIdAsync(id);
        if (estudio == null) return null;

        if (dto.FechaFin.HasValue && dto.FechaFin.Value <= dto.FechaInicio)
            throw new InvalidOperationException("La fecha de fin debe ser posterior a la fecha de inicio.");

        estudio.Institucion = dto.Institucion;
        estudio.Titulo = dto.Titulo;
        estudio.NivelEstudio = dto.NivelEstudio;
        estudio.FechaInicio = dto.FechaInicio;
        estudio.FechaFin = dto.FechaFin;

        var updated = await _estudioRepository.UpdateAsync(estudio);
        return MapToResponseDTO(updated);
    }

    public async Task<bool> DeleteEstudioAsync(int id)
    {
        return await _estudioRepository.DeleteAsync(id);
    }

    private static EstudioResponseDTO MapToResponseDTO(Estudio estudio)
    {
        return new EstudioResponseDTO
        {
            Id = estudio.Id,
            Institucion = estudio.Institucion,
            Titulo = estudio.Titulo,
            NivelEstudio = estudio.NivelEstudio.ToString(),
            FechaInicio = estudio.FechaInicio,
            FechaFin = estudio.FechaFin,
            UserId = estudio.UserId
        };
    }
}
