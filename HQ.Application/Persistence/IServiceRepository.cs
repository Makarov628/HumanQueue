

using HQ.Domain.QueueAggregate.ValueObjects;
using HQ.Domain.ServiceAggregate;
using HQ.Domain.ServiceAggregate.Entities;
using HQ.Domain.ServiceAggregate.Enums;
using HQ.Domain.ServiceAggregate.ValueObjects;
using HQ.Domain.WindowAggregate.ValueObjects;

namespace HQ.Application.Persistence;

public interface IServiceRepository
{
    Task<List<ServiceAggregate>> GetFlatServices(QueueId queueId, CancellationToken cancellationToken);
    Task<List<ServiceAggregate>> GetServices(QueueId queueId, CancellationToken cancellationToken);
    Task<List<ServiceAggregate>> GetServicesByWindow(WindowId windowId, CancellationToken cancellationToken);

    Task<ServiceAggregate?> GetService(ServiceId serviceId, CancellationToken cancellationToken);
    Task<ServiceAggregate?> GetServiceByRequest(RequestId requestId, CancellationToken cancellationToken);

    Task<List<Request>> GetRequestsWithStatuses(QueueId queueId, List<RequestStatus> statuses, CancellationToken cancellationToken); 
    
    Task<bool> IsLiteralExists(QueueId queueId, ServiceLiteral literal, CancellationToken cancellationToken);
    Task<bool> IsHasChildServices(ServiceId serviceId, CancellationToken cancellationToken);

    Task Add(ServiceAggregate service, CancellationToken cancellationToken);
    Task Update(ServiceAggregate service, CancellationToken cancellationToken);
    Task UpdateMultiple(List<ServiceAggregate> services, CancellationToken cancellationToken);
    Task Delete(ServiceAggregate service, CancellationToken cancellationToken);
}
