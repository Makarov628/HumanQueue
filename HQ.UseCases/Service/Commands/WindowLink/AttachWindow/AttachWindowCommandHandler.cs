using MediatR;
using ErrorOr;
using HQ.Application.Persistence;
using HQ.Domain.WindowAggregate.ValueObjects;
using HQ.Domain.ServiceAggregate.ValueObjects;
using HQ.Domain.ServiceAggregate;

namespace HQ.UseCases.Service.Commands.WindowLink.AttachWindow;

internal class AttachWindowCommandHandler : IRequestHandler<AttachWindowCommand, ErrorOr<Success>>
{
    private readonly IServiceRepository _serviceRepository;
    private readonly IWindowRepository _windowRepository;

    public AttachWindowCommandHandler(
        IServiceRepository serviceRepository,
        IWindowRepository windowRepository
    )
    {
        _serviceRepository = serviceRepository;
        _windowRepository = windowRepository;
    }

    public async Task<ErrorOr<Success>> Handle(AttachWindowCommand request, CancellationToken cancellationToken)
    {
        WindowId windowId = WindowId.Create(request.WindowId);
        ServiceId serviceId = ServiceId.Create(request.ServiceId);

        bool windowIsExists = await _windowRepository.IsExists(windowId, cancellationToken);
        if (!windowIsExists)
            return Error.NotFound(description: "Данное окно не найдено");

        ServiceAggregate? service = await _serviceRepository.GetService(serviceId, cancellationToken);
        if (service is null)
            return Error.NotFound(description: "Данная услуга не найдена");

        ErrorOr<Created> result = service.AddWindowLink(windowId);
        if (result.IsError)
            return result.Errors;

        await _serviceRepository.Update(service, cancellationToken);

        return Result.Success;
    }
}