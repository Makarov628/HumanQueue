using MediatR;
using ErrorOr;
using HQ.Application.Persistence;
using HQ.Domain.QueueAggregate.ValueObjects;
using HQ.Domain.QueueAggregate;
using HQ.Domain.WindowAggregate;

namespace HQ.UseCases.Window.Commands.Create;

internal class CreateWindowCommandHandler : IRequestHandler<CreateWindowCommand, ErrorOr<Created>>
{
    private readonly IWindowRepository _windowRepository;
    private readonly IQueueRepository _queueRepository;

    public CreateWindowCommandHandler(
        IWindowRepository windowRepository,
        IQueueRepository queueRepository)
    {
        _windowRepository = windowRepository;
        _queueRepository = queueRepository;
    }

    public async Task<ErrorOr<Created>> Handle(CreateWindowCommand request, CancellationToken cancellationToken)
    {
        QueueId queueId = QueueId.Create(request.QueueId);
        QueueAggregate? queue = await _queueRepository.GetById(queueId, cancellationToken);
        if (queue is null)
            return Error.NotFound(description: "Данной очереди не существует");

        bool windowIsExists = await _windowRepository.IsExistsWithNumber(queueId, request.Number, cancellationToken);
        if (windowIsExists)
            return Error.Validation(description: $"Окно с номером '{request.Number}' уже существует");

        WindowAggregate window = WindowAggregate.Create(queueId, request.Number, null);
        await _windowRepository.Add(window, cancellationToken);
        return Result.Created;
    }
}
