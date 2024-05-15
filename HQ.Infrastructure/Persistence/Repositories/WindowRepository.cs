

using Microsoft.EntityFrameworkCore;
using HQ.Application.Persistence;
using HQ.Domain.QueueAggregate.ValueObjects;
using HQ.Domain.UserAggregate.ValueObjects;
using HQ.Domain.WindowAggregate;
using HQ.Domain.WindowAggregate.ValueObjects;

namespace HQ.Infrastructure.Persistence;

public class WindowRepository : IWindowRepository
{
    private readonly HQDbContext _dbContext;

    public WindowRepository(HQDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Add(WindowAggregate window, CancellationToken cancellationToken)
    {
        _dbContext.Windows.Add(window);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task Delete(WindowAggregate window, CancellationToken cancellationToken)
    {
        _dbContext.Windows.Remove(window);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<WindowAggregate?> GetById(WindowId windowId, CancellationToken cancellationToken)
    {
        return await _dbContext.Windows.FirstOrDefaultAsync(w => w.Id == windowId, cancellationToken);
    }

    public async Task<List<WindowAggregate>> GetByIds(List<WindowId> windowIds, CancellationToken cancellationToken)
    {
        return await _dbContext.Windows.Where(w => windowIds.Contains(w.Id)).ToListAsync(cancellationToken);
    }

    public async Task<List<WindowAggregate>> GetByQueue(QueueId queueId, CancellationToken cancellationToken)
    {
        return await _dbContext.Windows.Where(w => w.QueueId == queueId).ToListAsync(cancellationToken);
    }

    public async Task<WindowAggregate?> GetByUser(UserId userId, CancellationToken cancellationToken)
    {
        return await _dbContext.Windows.FirstOrDefaultAsync(w => w.AttachedUserId == userId, cancellationToken);
    }

    public async Task<bool> IsExists(WindowId windowId, CancellationToken cancellationToken)
    {
        return await _dbContext.Windows.AnyAsync(w => w.Id == windowId, cancellationToken);
    }

    public async Task<bool> IsExistsWithNumber(QueueId queueId, int number, CancellationToken cancellationToken)
    {
        return await _dbContext.Windows.AnyAsync(w => w.QueueId == queueId && w.Number == number, cancellationToken);
    }

    public async Task Update(WindowAggregate window, CancellationToken cancellationToken)
    {
        _dbContext.Windows.Update(window);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}