
using ErrorOr;
using HQ.Application.Persistence;
using HQ.Domain.QueueAggregate;
using HQ.Domain.QueueAggregate.ValueObjects;
using MediatR;

namespace HQ.UseCases.Queue.Commands.Delete;

internal class DeleteQueueCommandHandler : IRequestHandler<DeleteQueueCommand, ErrorOr<Deleted>>
{
    private readonly IQueueRepository _queueRepository;

    public DeleteQueueCommandHandler(IQueueRepository queueRepository)
    {
        _queueRepository = queueRepository;
    }

    public async Task<ErrorOr<Deleted>> Handle(DeleteQueueCommand request, CancellationToken cancellationToken)
    {
        QueueAggregate? queue = await _queueRepository.GetById(QueueId.Create(request.Id), cancellationToken);
        if (queue is null)
            return Error.NotFound(description: "Очередь не найдена.");

        await _queueRepository.Delete(queue, cancellationToken);

        return Result.Deleted;
    }
}
