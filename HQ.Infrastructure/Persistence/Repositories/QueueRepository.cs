
using Microsoft.EntityFrameworkCore;
using HQ.Application.Persistence;
using HQ.Domain.QueueAggregate;
using HQ.Domain.QueueAggregate.ValueObjects;

namespace HQ.Infrastructure.Persistence.Repositories;

public class QueueRepository : IQueueRepository
{
    private readonly HQDbContext _dbContext;

    public QueueRepository(HQDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Add(QueueAggregate queue, CancellationToken cancellationToken)
    {
        _dbContext.Queues.Add(queue);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task Delete(QueueAggregate queue, CancellationToken cancellationToken)
    {
        _dbContext.Queues.Remove(queue);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<QueueAggregate>> GetAll(CancellationToken cancellationToken)
    {
        return await _dbContext.Queues.ToListAsync(cancellationToken);
    }

    public async Task<QueueAggregate?> GetById(QueueId queueId, CancellationToken cancellationToken)
    {
        return await _dbContext.Queues.FirstOrDefaultAsync(q => q.Id == queueId, cancellationToken);
    }

    public async Task<bool> IsExists(QueueId queueId, CancellationToken cancellationToken)
    {
        return await _dbContext.Queues.AnyAsync(q => q.Id == queueId, cancellationToken);
    }

    public async Task Update(QueueAggregate queue, CancellationToken cancellationToken)
    {
        _dbContext.Queues.Update(queue);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}