using ErrorOr;
using HQ.Application.Persistence;
using HQ.Domain.Common.ValueObjects;
using HQ.Domain.QueueAggregate;
using HQ.Domain.QueueAggregate.ValueObjects;
using HQ.Domain.ServiceAggregate;
using HQ.Domain.TerminalAggregate;
using HQ.Domain.WindowAggregate;
using MediatR;

namespace HQ.UseCases.Queue.Queries.GetQueue;

internal class GetQueueQueryHandler : IRequestHandler<GetQueueQuery, ErrorOr<QueueResponse>>
{
    private readonly IQueueRepository _queueRepository;
    private readonly ITerminalRepository _terminalRepository;
    private readonly IWindowRepository _windowRepository;
    private readonly IServiceRepository _serviceRepository;

    public GetQueueQueryHandler(
        IQueueRepository queueRepository,
        IServiceRepository serviceRepository,
        IWindowRepository windowRepository,
        ITerminalRepository terminalRepository)
    {
        _queueRepository = queueRepository;
        _serviceRepository = serviceRepository;
        _windowRepository = windowRepository;
        _terminalRepository = terminalRepository;
    }

    public async Task<ErrorOr<QueueResponse>> Handle(GetQueueQuery request, CancellationToken cancellationToken)
    {
        QueueId queueId = QueueId.Create(request.Id);
        QueueAggregate? queue = await _queueRepository.GetById(queueId, cancellationToken);
        if (queue is null)
            return Error.NotFound(description: "Очередь не найдена.");

        List<TerminalAggregate> terminals = await _terminalRepository.GetTerminalsByQueue(queueId, cancellationToken);
        List<WindowAggregate> windows = await _windowRepository.GetByQueue(queueId, cancellationToken);
        List<ServiceAggregate> services = await _serviceRepository.GetFlatServices(queueId, cancellationToken);

        return new QueueResponse(
            queue.Id.Value,
            queue.Name,
            queue.DefaultCulture.Name,
            AvailableCultures: AvailableCultures.GetCultures().Select(culture => new QueueAvailableCultureResponse(
                culture.Name,
                culture.LanguageName
            )).ToList(),
            terminals.ConvertAll(terminal => new QueueTerminalResponse(
                terminal.Id.Value,
                terminal.Name,
                terminal.ExternalPrinterId
            )),
            windows.ConvertAll(window => new QueueWindowResponse(
                window.Id.Value,
                window.Number
            )),
            services.ConvertAll(service => new QueueServiceResponse(
                service.Id.Value,
                service.Name.GetStringPartByCulture(queue.DefaultCulture.Name)?.Value ?? "Без имени.",
                service.RequestNumberCounter,
                service.Literal?.Value,
                service.ParentId?.Value,
                service.Name.StringParts,
                service.LinkedWindowIds,
                Childs: new List<QueueServiceResponse>()
            )).CreateTree()
        );
    }
}
