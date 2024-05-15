using MediatR;
using ErrorOr;
using HQ.Application.Persistence;
using HQ.Domain.QueueAggregate.ValueObjects;
using HQ.Domain.QueueAggregate;
using HQ.Domain.TerminalAggregate;
using HQ.Domain.TerminalAggregate.ValueObjects;

namespace HQ.UseCases.Terminal.Commands.Delete;

internal class DeleteTerminalCommandHandler : IRequestHandler<DeleteTerminalCommand, ErrorOr<Deleted>>
{
    private readonly ITerminalRepository _terminalRepository;

    public DeleteTerminalCommandHandler(
        ITerminalRepository terminalRepository
    )
    {
        _terminalRepository = terminalRepository;
    }

    public async Task<ErrorOr<Deleted>> Handle(DeleteTerminalCommand request, CancellationToken cancellationToken)
    {
        TerminalId terminalId = TerminalId.Create(request.Id);
        TerminalAggregate? terminal = await _terminalRepository.GetTerminal(terminalId, cancellationToken);
        if (terminal is null)
            return Error.NotFound(description: "Данный терминал не найден");

        await _terminalRepository.Delete(terminal, cancellationToken);
        return Result.Deleted;
    }
}
