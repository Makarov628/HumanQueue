

using HQ.Domain.UserAggregate;
using HQ.Domain.UserAggregate.ValueObjects;

namespace HQ.Application.Persistence;

public interface IUserRepository
{
    Task<List<UserAggregate>> GetUsers(CancellationToken cancellationToken);
    Task<UserAggregate?> GetUser(UserId userId, CancellationToken cancellationToken);

    Task<bool> IsExistsWithLogin(string login, CancellationToken cancellationToken);
    Task<bool> IsExistsWithEmail(string email, CancellationToken cancellationToken);

    Task Add(UserAggregate user, CancellationToken cancellationToken);
    Task Update(UserAggregate user, CancellationToken cancellationToken);
    Task Delete(UserAggregate user, CancellationToken cancellationToken);
}