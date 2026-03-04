using Microsoft.EntityFrameworkCore;
using backend.Data;
using backend.Models.Entities;
using backend.Repositories.Interfaces;

namespace backend.Repositories;

public class EstudioRepository : IEstudioRepository
{
    private readonly ApplicationDbContext _context;

    public EstudioRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Estudio>> GetAllByUserIdAsync(int userId)
    {
        return await _context.Estudios
            .Where(e => e.UserId == userId)
            .ToListAsync();
    }

    public async Task<Estudio?> GetByIdAsync(int id)
    {
        return await _context.Estudios
            .Include(e => e.User)
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<Estudio> CreateAsync(Estudio estudio)
    {
        _context.Estudios.Add(estudio);
        await _context.SaveChangesAsync();
        return estudio;
    }

    public async Task<Estudio> UpdateAsync(Estudio estudio)
    {
        _context.Estudios.Update(estudio);
        await _context.SaveChangesAsync();
        return estudio;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var estudio = await _context.Estudios.FindAsync(id);
        if (estudio == null) return false;

        _context.Estudios.Remove(estudio);
        await _context.SaveChangesAsync();
        return true;
    }
}
