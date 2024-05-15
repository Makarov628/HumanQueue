
using MediatR;
using ErrorOr;

namespace HQ.UseCases.Window.Commands.ChangeStatusLosted;

public record ChangeRequestStatusLostedCommand(
    Guid RequestId
): IRequest<ErrorOr<Success>>;

