


using HQ.Domain.QueueAggregate.ValueObjects;
using HQ.Domain.UserAggregate.ValueObjects;
using HQ.Domain.WindowAggregate;
using HQ.Domain.WindowAggregate.ValueObjects;

namespace HQ.Application.Persistence;

public interface IWindowRepository
{
    Task<WindowAggregate?> GetById(WindowId windowId, CancellationToken cancellationToken);
    Task<List<WindowAggregate>> GetByIds(List<WindowId> windowIds, CancellationToken cancellationToken);
    Task<List<WindowAggregate>> GetByQueue(QueueId queueId, CancellationToken cancellationToken);
    Task<WindowAggregate?> GetByUser(UserId userId, CancellationToken cancellationToken);

    Task<bool> IsExistsWithNumber(QueueId queueId, int number, CancellationToken cancellationToken);
    Task<bool> IsExists(WindowId windowId, CancellationToken cancellationToken);

    Task Add(WindowAggregate window, CancellationToken cancellationToken);
    Task Update(WindowAggregate window, CancellationToken cancellationToken);
    Task Delete(WindowAggregate window, CancellationToken cancellationToken);
}