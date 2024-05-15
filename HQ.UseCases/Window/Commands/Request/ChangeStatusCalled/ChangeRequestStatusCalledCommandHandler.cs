

using ErrorOr;
using HQ.Application.Persistence;
using HQ.Domain.ServiceAggregate;
using HQ.Domain.ServiceAggregate.ValueObjects;
using HQ.Domain.WindowAggregate.ValueObjects;
using MediatR;

namespace HQ.UseCases.Window.Commands.ChangeStatusCalled;

internal class ChangeRequestStatusCalledCommandHandler : IRequestHandler<ChangeRequestStatusCalledCommand, ErrorOr<Success>>
{
    private readonly IServiceRepository _serviceRepository;
    private readonly IWindowRepository _windowRepository;

    public ChangeRequestStatusCalledCommandHandler(
        IServiceRepository serviceRepository, 
        IWindowRepository windowRepository
    )
    {
        _serviceRepository = serviceRepository;
        _windowRepository = windowRepository;
    }

    public async Task<ErrorOr<Success>> Handle(ChangeRequestStatusCalledCommand request, CancellationToken cancellationToken)
    {
        RequestId requestId = RequestId.Create(request.RequestId);
        WindowId windowId = WindowId.Create(request.CalledByWindowId);

        bool windowIsExists = await _windowRepository.IsExists(windowId, cancellationToken);
        if (!windowIsExists)
            return Error.NotFound(description: "Данное окно не найдено");

        ServiceAggregate? service = await _serviceRepository.GetServiceByRequest(requestId, cancellationToken);
        if (service is null)
            return Error.NotFound(description: "Данная услуга не найдена");

        ErrorOr<Success> result = service.SetCalled(requestId, windowId);
        if (result.IsError)
            return result.Errors;

        await _serviceRepository.Update(service, cancellationToken);
        
        return Result.Success;
    }
}