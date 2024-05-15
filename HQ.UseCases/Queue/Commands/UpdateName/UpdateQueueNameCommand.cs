

using ErrorOr;
using MediatR;

namespace HQ.UseCases.Queue.Commands.UpdateName;

public record UpdateQueueNameCommand(
    Guid Id,
    string Name
) : IRequest<ErrorOr<Success>>;

