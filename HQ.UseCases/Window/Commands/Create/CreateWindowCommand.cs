using MediatR;
using ErrorOr;

namespace HQ.UseCases.Window.Commands.Create;

public record CreateWindowCommand(
    Guid QueueId, 
    int Number
) : IRequest<ErrorOr<Created>>;
