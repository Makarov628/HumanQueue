using MediatR;
using ErrorOr;
using HQ.Application.Persistence;
using HQ.Domain.QueueAggregate.ValueObjects;
using HQ.Domain.QueueAggregate;
using HQ.Domain.TerminalAggregate;
using HQ.Domain.TerminalAggregate.ValueObjects;
using HQ.Domain.WindowAggregate.ValueObjects;
using HQ.Domain.WindowAggregate;

namespace HQ.UseCases.Window.Commands.Delete;

internal class DeleteWindowCommandHandler : IRequestHandler<DeleteWindowCommand, ErrorOr<Deleted>>
{
    private readonly IWindowRepository _windowRepository;

    public DeleteWindowCommandHandler(
        IWindowRepository windowRepository
    )
    {
        _windowRepository = windowRepository;
    }

    public async Task<ErrorOr<Deleted>> Handle(DeleteWindowCommand request, CancellationToken cancellationToken)
    {
        WindowId windowId = WindowId.Create(request.Id);
        WindowAggregate? window = await _windowRepository.GetById(windowId, cancellationToken);
        if (window is null)
            return Error.NotFound(description: "Данное окно не найдено");

        await _windowRepository.Delete(window, cancellationToken);
        return Result.Deleted;
    }
}
