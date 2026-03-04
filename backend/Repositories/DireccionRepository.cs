using Microsoft.EntityFrameworkCore;
using backend.Data;
using backend.Models.Entities;
using backend.Repositories.Interfaces;

namespace backend.Repositories;

public class DireccionRepository : IDireccionRepository
{
    private readonly ApplicationDbContext _context;

    public DireccionRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Direccion>> GetAllByUserIdAsync(int userId)
    {
        return await _context.Direcciones
            .Where(d => d.UserId == userId)
            .ToListAsync();
    }

    public async Task<Direccion?> GetByIdAsync(int id)
    {
        return await _context.Direcciones
            .Include(d => d.User)
            .FirstOrDefaultAsync(d => d.Id == id);
    }

    public async Task<Direccion> CreateAsync(Direccion direccion)
    {
        _context.Direcciones.Add(direccion);
        await _context.SaveChangesAsync();
        return direccion;
    }

    public async Task<Direccion> UpdateAsync(Direccion direccion)
    {
        _context.Direcciones.Update(direccion);
        await _context.SaveChangesAsync();
        return direccion;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var direccion = await _context.Direcciones.FindAsync(id);
        if (direccion == null) return false;

        _context.Direcciones.Remove(direccion);
        await _context.SaveChangesAsync();
        return true;
    }
}
