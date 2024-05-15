using MediatR;
using ErrorOr;
using HQ.Domain.Common.ValueObjects;

namespace HQ.UseCases.Service.Commands.WindowLink.DeattachWindow;

public record DeattachWindowCommand(
    Guid ServiceId,
    Guid WindowId
): IRequest<ErrorOr<Success>>;

