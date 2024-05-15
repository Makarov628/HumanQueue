
using ErrorOr;
using HQ.Application.Persistence;
using HQ.Domain.ServiceAggregate;
using HQ.Domain.ServiceAggregate.ValueObjects;
using MediatR;

namespace HQ.UseCases.Service.Commands.Common.UpdateLiteral;

internal class UpdateServiceLiteralCommandHandler : IRequestHandler<UpdateServiceLiteralCommand, ErrorOr<Success>>
{
    private readonly IServiceRepository _serviceRepository;

    public UpdateServiceLiteralCommandHandler(
        IServiceRepository serviceRepository
    )
    {
        _serviceRepository = serviceRepository;
    }

    public async Task<ErrorOr<Success>> Handle(UpdateServiceLiteralCommand request, CancellationToken cancellationToken)
    {
        ServiceId serviceId = ServiceId.Create(request.Id);
        ServiceAggregate? service = await _serviceRepository.GetService(serviceId, cancellationToken);
        
        if (service is null)
            return Error.NotFound(description: "Данная услуга не найдена");

        ErrorOr<ServiceLiteral> literal = ServiceLiteral.Create(request.Litetal);
        if (literal.IsError)
            return literal.Errors;

        bool serviceHasChilds = await _serviceRepository.IsHasChildServices(serviceId, cancellationToken);
        if (serviceHasChilds)
            return Error.Validation(description: "Нельзя назначить литерал услуге, так как она содержит в себе дочерние услуги");

        service.UpdateLiteral(literal.Value);
        await _serviceRepository.Update(service, cancellationToken);

        return Result.Success;
    }
}
