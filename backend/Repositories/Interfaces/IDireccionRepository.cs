using backend.Models.Entities;

namespace backend.Repositories.Interfaces;

public interface IDireccionRepository
{
    Task<IEnumerable<Direccion>> GetAllByUserIdAsync(int userId);
    Task<Direccion?> GetByIdAsync(int id);
    Task<Direccion> CreateAsync(Direccion direccion);
    Task<Direccion> UpdateAsync(Direccion direccion);
    Task<bool> DeleteAsync(int id);
}
