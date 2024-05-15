using MediatR;
using ErrorOr;
using HQ.Domain.Common.ValueObjects;

namespace HQ.UseCases.Service.Commands.WindowLink.AttachWindow;

public record AttachWindowCommand(
    Guid ServiceId,
    Guid WindowId
): IRequest<ErrorOr<Success>>;

