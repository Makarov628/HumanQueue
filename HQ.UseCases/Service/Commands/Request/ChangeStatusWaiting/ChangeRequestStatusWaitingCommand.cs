
using MediatR;
using ErrorOr;

namespace HQ.UseCases.Service.Commands.Request.ChangeStatusWaiting;

public record ChangeRequestStatusWaitingCommand(
    Guid RequestId
): IRequest<ErrorOr<Success>>;

