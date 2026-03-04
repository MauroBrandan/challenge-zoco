using backend.Models.Entities;

namespace backend.Repositories.Interfaces;

public interface ISessionRepository
{
    Task<SessionLog> CreateAsync(SessionLog session);
    Task<SessionLog?> GetActiveByRefreshTokenAsync(string refreshToken);
    Task EndSessionAsync(SessionLog session);
}