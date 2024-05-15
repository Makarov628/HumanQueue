

using HQ.Domain.QueueAggregate;
using HQ.Domain.QueueAggregate.ValueObjects;

namespace HQ.Application.Persistence;

public interface IQueueRepository
{

    Task<List<QueueAggregate>> GetAll(CancellationToken cancellationToken);
    Task<QueueAggregate?> GetById(QueueId queueId, CancellationToken cancellationToken);

    Task<bool> IsExists(QueueId queueId, CancellationToken cancellationToken);

    Task Add(QueueAggregate queue, CancellationToken cancellationToken);
    Task Update(QueueAggregate queue, CancellationToken cancellationToken);
    Task Delete(QueueAggregate queue, CancellationToken cancellationToken);
}