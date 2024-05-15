

using MediatR;
using ErrorOr;

namespace HQ.UseCases.Window.Commands.ChangeStatusCalled;

public record ChangeRequestStatusCalledCommand(
    Guid RequestId,
    Guid CalledByWindowId
): IRequest<ErrorOr<Success>>;

