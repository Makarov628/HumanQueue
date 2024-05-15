

using ErrorOr;
using MediatR;

namespace HQ.UseCases.Queue.Commands.Delete;

public record DeleteQueueCommand(
    Guid Id
): IRequest<ErrorOr<Deleted>>;


