

using ErrorOr;
using MediatR;

namespace HQ.UseCases.Queue.Commands.ResetQueue;

public record ResetQueueCommand(
    Guid Id
): IRequest<ErrorOr<Success>>;


