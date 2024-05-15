using MediatR;
using ErrorOr;
using HQ.Application.Persistence;
using HQ.Domain.QueueAggregate.ValueObjects;
using HQ.Domain.QueueAggregate;
using HQ.Domain.TerminalAggregate;
using HQ.Domain.TerminalAggregate.ValueObjects;

namespace HQ.UseCases.Terminal.Commands.Update;

internal class UpdateTerminalCommandHandler : IRequestHandler<UpdateTerminalCommand, ErrorOr<Updated>>
{
    private readonly ITerminalRepository _terminalRepository;

    public UpdateTerminalCommandHandler(
        ITerminalRepository terminalRepository
    )
    {
        _terminalRepository = terminalRepository;
    }

    public async Task<ErrorOr<Updated>> Handle(UpdateTerminalCommand request, CancellationToken cancellationToken)
    {
        TerminalId terminalId = TerminalId.Create(request.TerminalId);
        TerminalAggregate? terminal = await _terminalRepository.GetTerminal(terminalId, cancellationToken);
        if (terminal is null)
            return Error.NotFound(description: "Данный терминал не найден");

        terminal.SetName(request.Name);
        terminal.SetExternalPrinterId(request.ExternalPrinterId);

        await _terminalRepository.Update(terminal, cancellationToken);
        return Result.Updated;
    }
}
