

using ErrorOr;
using MediatR;

namespace HQ.UseCases.Queue.Commands.Create;

public record CreateQueueCommand(
    string Name,
    string Culture
): IRequest<ErrorOr<Created>>;


