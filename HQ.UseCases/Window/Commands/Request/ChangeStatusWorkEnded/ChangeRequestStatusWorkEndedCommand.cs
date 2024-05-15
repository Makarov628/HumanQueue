


using MediatR;
using ErrorOr;

namespace HQ.UseCases.Window.Commands.ChangeStatusWorkEnded;

public record ChangeRequestStatusWorkEndedCommand(
    Guid RequestId
): IRequest<ErrorOr<Success>>;

