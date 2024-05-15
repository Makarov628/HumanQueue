
using ErrorOr;
using HQ.Application.Persistence;
using HQ.Domain.QueueAggregate;
using HQ.Domain.QueueAggregate.ValueObjects;
using HQ.Domain.ServiceAggregate;
using MediatR;

namespace HQ.UseCases.Queue.Commands.ResetQueue;

internal class ResetQueueCommandHandler : IRequestHandler<ResetQueueCommand, ErrorOr<Success>>
{
    private readonly IQueueRepository _queueRepository;
    private readonly IServiceRepository _serviceRepository;

    public ResetQueueCommandHandler(
        IQueueRepository queueRepository, 
        IServiceRepository serviceRepository
    )
    {
        _queueRepository = queueRepository;
        _serviceRepository = serviceRepository;
    }

    public async Task<ErrorOr<Success>> Handle(ResetQueueCommand request, CancellationToken cancellationToken)
    {
        QueueAggregate? queue = await _queueRepository.GetById(QueueId.Create(request.Id), cancellationToken);
        if (queue is null)
            return Error.NotFound(description: "Очередь не найдена.");

        List<ServiceAggregate> services = await _serviceRepository.GetServices(queue.Id, cancellationToken);

        foreach (ServiceAggregate service in services)
        {
            service.ResetRequestNumberCounter();
            service.MarkNotFinishedRequestsAsUnworked();
        }

        await _serviceRepository.UpdateMultiple(services, cancellationToken);

        return Result.Success;
    }
}
