

using ErrorOr;
using HQ.Application.Persistence;
using HQ.Domain.QueueAggregate.ValueObjects;
using HQ.Domain.ServiceAggregate.Entities;
using HQ.Domain.ServiceAggregate.Enums;
using HQ.Domain.WindowAggregate;
using HQ.Domain.WindowAggregate.ValueObjects;
using MediatR;

namespace HQ.UseCases.Service.Queries.GetRequestsForTablo;

internal class GetRequestsForTabloQueryHandler : IRequestHandler<GetRequestsForTabloQuery, ErrorOr<TabloResponse>>
{
    private readonly IQueueRepository _queueRepository;
    private readonly IServiceRepository _serviceRepository;
    private readonly IWindowRepository _windowRepository;

    public GetRequestsForTabloQueryHandler(
        IQueueRepository queueRepository,
        IServiceRepository serviceRepository,
        IWindowRepository windowRepository
    )
    {
        _queueRepository = queueRepository;
        _serviceRepository = serviceRepository;
        _windowRepository = windowRepository;
    }

    public async Task<ErrorOr<TabloResponse>> Handle(GetRequestsForTabloQuery request, CancellationToken cancellationToken)
    {
        QueueId queueId = QueueId.Create(request.QueueId);
        bool queueIsExists = await _queueRepository.IsExists(queueId, cancellationToken);
        if (!queueIsExists)
            return Error.NotFound(description: "Очередь не найдена");
        
        List<RequestStatus> requestStatuses = new() { RequestStatus.Waiting, RequestStatus.Called };
        List<Request> requests = await _serviceRepository.GetRequestsWithStatuses(queueId, requestStatuses, cancellationToken);

        if (!requests.Any())
            return new TabloResponse(
                Waiting: new(),
                Called: new()
            );

        List<WindowId> windowIds = requests
            .Where(req => req.CalledByWindowId is not null)
            .Select(req => req.CalledByWindowId!)
            .ToList();

        List<WindowAggregate> windows = await _windowRepository.GetByIds(windowIds, cancellationToken);

        List<RequestWaitingResponse> waiting = requests
            .Where(req => req.IsWaiting())
            .Select(req => new RequestWaitingResponse(
                req.Id.Value,
                req.Number.ToString(),
                req.Culture.Name,
                req.CreatedAt
            )).OrderBy(r => r.CreatedDate).ToList();
        
        List<RequestCalledResponse> called = requests
            .Where(req => req.IsCalled() && windows.Any(window => window.Id == req.CalledByWindowId!))
            .Select(req => new RequestCalledResponse(
                req.Id.Value,
                req.Number.ToString(),
                req.Culture.Name,
                req.CreatedAt,
                windows
                    .Where(window => window.Id == req.CalledByWindowId!)
                    .Select(window => new TabloWindowResponse(
                        window.Id.Value,
                        window.Number
                    )).First()
            )).OrderBy(r => r.CreatedDate).ToList();

        return new TabloResponse(
            waiting,
            called
        );
    }
}
