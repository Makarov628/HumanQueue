using MediatR;
using ErrorOr;

namespace HQ.UseCases.Terminal.Commands.Delete;

public record DeleteTerminalCommand(
    Guid Id
) : IRequest<ErrorOr<Deleted>>;