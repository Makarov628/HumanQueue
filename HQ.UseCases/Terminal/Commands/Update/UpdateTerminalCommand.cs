using MediatR;
using ErrorOr;

namespace HQ.UseCases.Terminal.Commands.Update;

public record UpdateTerminalCommand(
    Guid TerminalId,
    string Name,
    string? ExternalPrinterId
) : IRequest<ErrorOr<Updated>>;