using MediatR;
using ErrorOr;

namespace HQ.UseCases.Window.Commands.Delete;

public record DeleteWindowCommand(
    Guid Id
) : IRequest<ErrorOr<Deleted>>;