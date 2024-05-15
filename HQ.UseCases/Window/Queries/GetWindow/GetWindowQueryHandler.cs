using MediatR;
using ErrorOr;
using HQ.Application.Persistence;
using HQ.Domain.WindowAggregate.ValueObjects;
using HQ.Domain.WindowAggregate;
using HQ.Domain.ServiceAggregate;

namespace HQ.UseCases.Window.Queries.GetWindow;

internal class GetWindowQueryHandler : IRequestHandler<GetWindowQuery, ErrorOr<WindowResponse>>
{
    private readonly IWindowRepository _windowRepository;
    private readonly IServiceRepository _serviceRepository;

    public GetWindowQueryHandler(IWindowRepository windowRepository, IServiceRepository serviceRepository)
    {
        _windowRepository = windowRepository;
        _serviceRepository = serviceRepository;
    }

    public async Task<ErrorOr<WindowResponse>> Handle(GetWindowQuery request, CancellationToken cancellationToken)
    {
        WindowId windowId = WindowId.Create(request.Id);
        WindowAggregate? window = await _windowRepository.GetById(windowId, cancellationToken);
        if (window is null)
            return Error.NotFound(description: "Данное окно не найдено");

        List<ServiceAggregate> services = await _serviceRepository.GetServicesByWindow(windowId, cancellationToken);

        List<AttachedService> attachedServices = services.ConvertAll(service => new AttachedService(
            Id: service.Id.Value,
            Name: service.Name.StringParts
        ));

        List<WaitingRequestResponse> waitingRequests = services
            .Where(service => service.Requests.Any(req => req.IsWaiting()))
            .SelectMany(service =>
                service.Requests
                    .Where(req => req.IsWaiting())
                    .Select(req => new WaitingRequestResponse(
                        Id: req.Id.Value,
                        Number: req.Number.ToString(),
                        ServiceName: service.Name.GetValueByCulture(req.Culture) ?? "[No service name]",
                        Culture: req.Culture.Name,
                        CreatedDate: req.CreatedAt
                    ))
            )
            .OrderBy(r => r.CreatedDate)
            .ToList();

        CurrentRequestResponse? currentRequest = services
            .Where(service => service.Requests
                .Any(req =>
                    (req.IsCalled() || req.IsWorkStarted()) &&
                    req.CalledByWindowId?.Value == windowId.Value
                )
            )
            .Select(service => service.Requests
                .Where(req => 
                    (req.IsCalled() || req.IsWorkStarted()) && 
                    req.CalledByWindowId?.Value == windowId.Value
                ).Select(req => new CurrentRequestResponse(
                    Id: req.Id.Value,
                    Number: req.Number.ToString(),
                    ServiceName: service.Name.GetValueByCulture(req.Culture) ?? "[No service name]",
                    Culture: req.Culture.Name,
                    Status: req.Status,
                    CreatedDate: req.CreatedAt
                )).FirstOrDefault()
            ).FirstOrDefault();

        return new WindowResponse(
            Id: window.Id.Value,
            Number: window.Number,
            AttachedServices: attachedServices,
            CurrentRequest: currentRequest,
            WaitingRequests: waitingRequests
        );
    }
}
