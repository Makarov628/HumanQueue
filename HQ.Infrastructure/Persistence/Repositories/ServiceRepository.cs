
using Microsoft.EntityFrameworkCore;
using HQ.Application.Persistence;
using HQ.Domain.QueueAggregate.ValueObjects;
using HQ.Domain.ServiceAggregate;
using HQ.Domain.ServiceAggregate.Entities;
using HQ.Domain.ServiceAggregate.Enums;
using HQ.Domain.ServiceAggregate.ValueObjects;
using HQ.Domain.WindowAggregate.ValueObjects;

namespace HQ.Infrastructure.Persistence.Repositories;

public class ServiceRepository : IServiceRepository
{
    private readonly HQDbContext _dbContext;

    public ServiceRepository(HQDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Add(ServiceAggregate service, CancellationToken cancellationToken)
    {
        _dbContext.Services.Add(service);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task Delete(ServiceAggregate service, CancellationToken cancellationToken)
    {
        _dbContext.Services.Remove(service);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<ServiceAggregate>> GetFlatServices(QueueId queueId, CancellationToken cancellationToken)
    {
        return await _dbContext.Services
            .Include(s => s.WindowLinks)
            .Where(service => service.QueueId == queueId)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<Request>> GetRequestsWithStatuses(QueueId queueId, List<RequestStatus> statuses, CancellationToken cancellationToken)
    {
        return await _dbContext.Services
            .AsNoTracking()
            .Include(s => s.Requests.Where(r => statuses.Contains(r.Status)))
            .Where(s => s.QueueId == queueId)
            .SelectMany(s => s.Requests.Where(r => statuses.Contains(r.Status)))
            .ToListAsync(cancellationToken);
    }

    public async Task<ServiceAggregate?> GetService(ServiceId serviceId, CancellationToken cancellationToken)
    {
        return await _dbContext.Services
            .Include(s => s.Requests)
            .Include(s => s.WindowLinks)
            .FirstOrDefaultAsync(s => s.Id == serviceId, cancellationToken);
    }

    public async Task<ServiceAggregate?> GetServiceByRequest(RequestId requestId, CancellationToken cancellationToken)
    {
        return await _dbContext.Services
            .Include(s => s.Requests.Where(r => r.Id == requestId))
            .FirstOrDefaultAsync(s => s.Requests.Any(r => r.Id == requestId), cancellationToken);
    }

    public async Task<List<ServiceAggregate>> GetServices(QueueId queueId, CancellationToken cancellationToken)
    {
        return await _dbContext.Services
            .Include(s => s.Requests)
            .Include(s => s.WindowLinks)
            .Where(s => s.QueueId == queueId)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<ServiceAggregate>> GetServicesByWindow(WindowId windowId, CancellationToken cancellationToken)
    {
        return await _dbContext.Services
            .Include(s => s.Requests)
            .Where(s => s.WindowLinks.Any(wl => wl.WindowId == windowId))
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> IsHasChildServices(ServiceId serviceId, CancellationToken cancellationToken)
    {
        return await _dbContext.Services.AnyAsync(s => s.ParentId == serviceId, cancellationToken);
    }

    public async Task<bool> IsLiteralExists(QueueId queueId, ServiceLiteral literal, CancellationToken cancellationToken)
    {
        return await _dbContext.Services.AnyAsync(s => s.QueueId == queueId && s.Literal == literal, cancellationToken);
    }

    public async Task Update(ServiceAggregate service, CancellationToken cancellationToken)
    {
        _dbContext.Services.Update(service);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateMultiple(List<ServiceAggregate> services, CancellationToken cancellationToken)
    {
        _dbContext.Services.UpdateRange(services);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
