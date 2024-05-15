

using ErrorOr;
using MediatR;

namespace HQ.UseCases.Service.Commands.Common.Delete;

public record DeleteServiceCommand(
    Guid Id
): IRequest<ErrorOr<Deleted>>;