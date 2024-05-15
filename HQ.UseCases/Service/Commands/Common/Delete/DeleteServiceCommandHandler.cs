
using ErrorOr;
using HQ.Application.Persistence;
using HQ.Domain.ServiceAggregate;
using HQ.Domain.ServiceAggregate.ValueObjects;
using MediatR;

namespace HQ.UseCases.Service.Commands.Common.Delete;

internal class DeleteServiceCommandHandler : IRequestHandler<DeleteServiceCommand, ErrorOr<Deleted>>
{
    private readonly IServiceRepository _serviceRepository;

    public DeleteServiceCommandHandler(
        IServiceRepository serviceRepository
    )
    {
        _serviceRepository = serviceRepository;
    }

    public async Task<ErrorOr<Deleted>> Handle(DeleteServiceCommand request, CancellationToken cancellationToken)
    {
        ServiceId serviceId = ServiceId.Create(request.Id);
        ServiceAggregate? service = await _serviceRepository.GetService(serviceId, cancellationToken);
        
        if (service is null)
            return Error.NotFound(description: "Данная услуга не найдена");

        await _serviceRepository.Delete(service, cancellationToken);
        return Result.Deleted;
    }
}
