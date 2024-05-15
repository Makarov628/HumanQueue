

using MediatR;
using ErrorOr;

namespace HQ.UseCases.Window.Commands.ChangeStatusWorkStarted;

public record ChangeRequestStatusWorkStartedCommand(
    Guid RequestId
): IRequest<ErrorOr<Success>>;

