using MediatR;
using ErrorOr;

namespace HQ.UseCases.Terminal.Commands.Create;

public record CreateTerminalCommand(
    Guid QueueId,
    string Name,
    string? ExternalPrinterId
) : IRequest<ErrorOr<Created>>;