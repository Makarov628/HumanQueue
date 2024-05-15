
using Microsoft.EntityFrameworkCore;
using HQ.Application.Persistence;
using HQ.Domain.UserAggregate;
using HQ.Domain.UserAggregate.ValueObjects;

namespace HQ.Infrastructure.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly HQDbContext _dbContext;

    public UserRepository(HQDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Add(UserAggregate user, CancellationToken cancellationToken)
    {
        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task Delete(UserAggregate user, CancellationToken cancellationToken)
    {
        _dbContext.Users.Remove(user);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<UserAggregate?> GetUser(UserId userId, CancellationToken cancellationToken)
    {
        return await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);
    }

    public async Task<List<UserAggregate>> GetUsers(CancellationToken cancellationToken)
    {
        return await _dbContext.Users.ToListAsync(cancellationToken);
    }

    public async Task<bool> IsExistsWithEmail(string email, CancellationToken cancellationToken)
    {
        return await _dbContext.Users.AnyAsync(u => u.Email == email, cancellationToken);
    }

    public async Task<bool> IsExistsWithLogin(string login, CancellationToken cancellationToken)
    {
        return await _dbContext.Users.AllAsync(u => u.Login == login, cancellationToken);
    }

    public async Task Update(UserAggregate user, CancellationToken cancellationToken)
    {
        _dbContext.Users.Update(user);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}