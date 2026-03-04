using Microsoft.EntityFrameworkCore;
using backend.Data;
using backend.Models.Entities;
using backend.Repositories.Interfaces;

namespace backend.Repositories;

public class SessionRepository : ISessionRepository
{
    private readonly ApplicationDbContext _context;

    public SessionRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<SessionLog> CreateAsync(SessionLog session)
    {
        _context.SessionLogs.Add(session);
        await _context.SaveChangesAsync();
        return session;
    }

    public async Task<SessionLog?> GetActiveByRefreshTokenAsync(string refreshToken)
    {
        return await _context.SessionLogs
            .Include(s => s.User)
            .FirstOrDefaultAsync(s =>
                s.RefreshToken == refreshToken &&
                s.FechaFin == null);
    }

    public async Task EndSessionAsync(SessionLog session)
    {
        session.FechaFin = DateTime.UtcNow;
        _context.SessionLogs.Update(session);
        await _context.SaveChangesAsync();
    }
}