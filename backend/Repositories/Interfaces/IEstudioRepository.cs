using backend.Models.Entities;

namespace backend.Repositories.Interfaces;

public interface IEstudioRepository
{
    Task<IEnumerable<Estudio>> GetAllByUserIdAsync(int userId);
    Task<Estudio?> GetByIdAsync(int id);
    Task<Estudio> CreateAsync(Estudio estudio);
    Task<Estudio> UpdateAsync(Estudio estudio);
    Task<bool> DeleteAsync(int id);
}
