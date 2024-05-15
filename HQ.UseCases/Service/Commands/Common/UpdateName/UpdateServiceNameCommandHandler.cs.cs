
using ErrorOr;
using HQ.Application.Persistence;
using HQ.Domain.Common.ValueObjects;
using HQ.Domain.ServiceAggregate;
using HQ.Domain.ServiceAggregate.ValueObjects;
using MediatR;

namespace HQ.UseCases.Service.Commands.Common.UpdateName;

internal class UpdateServiceNameCommandHandler : IRequestHandler<UpdateServiceNameCommand, ErrorOr<Success>>
{
    private readonly IServiceRepository _serviceRepository;

    public UpdateServiceNameCommandHandler(
        IServiceRepository serviceRepository
    )
    {
        _serviceRepository = serviceRepository;
    }

    public async Task<ErrorOr<Success>> Handle(UpdateServiceNameCommand request, CancellationToken cancellationToken)
    {
        ServiceId serviceId = ServiceId.Create(request.Id);
        ServiceAggregate? service = await _serviceRepository.GetService(serviceId, cancellationToken);

        if (service is null)
            return Error.NotFound(description: "Данная услуга не найдена");

        // Создаем локализируемую строку
        ErrorOr<LocalizedString> serviceName = LocalizedString.Create(request.Name);
        if (serviceName.IsError)
            return serviceName.Errors;

        service.UpdateName(serviceName.Value);
        await _serviceRepository.Update(service, cancellationToken);

        return Result.Success;
    }
}
