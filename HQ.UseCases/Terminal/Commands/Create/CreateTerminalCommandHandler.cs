using MediatR;
using ErrorOr;
using HQ.Application.Persistence;
using HQ.Domain.QueueAggregate.ValueObjects;
using HQ.Domain.QueueAggregate;
using HQ.Domain.TerminalAggregate;

namespace HQ.UseCases.Terminal.Commands.Create;

internal class CreateTerminalCommandHandler : IRequestHandler<CreateTerminalCommand, ErrorOr<Created>>
{
    private readonly IQueueRepository _queueRepository;
    private readonly ITerminalRepository _terminalRepository;

    public CreateTerminalCommandHandler(
        IQueueRepository queueRepository,
        ITerminalRepository terminalRepository
    )
    {
        _queueRepository = queueRepository;
        _terminalRepository = terminalRepository;
    }

    public async Task<ErrorOr<Created>> Handle(CreateTerminalCommand request, CancellationToken cancellationToken)
    {
        // Проверяем наличие очереди
        QueueId queueId = QueueId.Create(request.QueueId);
        QueueAggregate? queue = await _queueRepository.GetById(queueId, cancellationToken);
        if (queue is null)
            return Error.NotFound(description: "Данной очереди не существует");

        TerminalAggregate terminal = TerminalAggregate.Create(queueId, request.Name, request.ExternalPrinterId);
        await _terminalRepository.Add(terminal, cancellationToken);
        return Result.Created;
    }
}
