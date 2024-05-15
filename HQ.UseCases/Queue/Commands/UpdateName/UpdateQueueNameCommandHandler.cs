
using ErrorOr;
using HQ.Application.Persistence;
using HQ.Domain.QueueAggregate;
using HQ.Domain.QueueAggregate.ValueObjects;
using HQ.Domain.ServiceAggregate;
using MediatR;

namespace HQ.UseCases.Queue.Commands.UpdateName;

internal class UpdateQueueNameCommandHandler : IRequestHandler<UpdateQueueNameCommand, ErrorOr<Success>>
{
    private readonly IQueueRepository _queueRepository;

    public UpdateQueueNameCommandHandler(
        IQueueRepository queueRepository
    )
    {
        _queueRepository = queueRepository;
    }

    public async Task<ErrorOr<Success>> Handle(UpdateQueueNameCommand request, CancellationToken cancellationToken)
    {
        QueueAggregate? queue = await _queueRepository.GetById(QueueId.Create(request.Id), cancellationToken);
        if (queue is null)
            return Error.NotFound(description: "Очередь не найдена.");

        queue.ChangeName(request.Name);
        await _queueRepository.Update(queue, cancellationToken);

        return Result.Success;
    }
}
