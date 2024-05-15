

using ErrorOr;
using MediatR;

namespace HQ.UseCases.Terminal.Commands.PrintRequest;

public record TerminalPrintRequestCommand(
    Guid TerminalId,
    Guid RequestId
): IRequest<ErrorOr<Success>>;