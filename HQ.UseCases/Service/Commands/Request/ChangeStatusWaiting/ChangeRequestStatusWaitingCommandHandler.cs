
using ErrorOr;
using HQ.Application.Persistence;
using HQ.Domain.ServiceAggregate;
using HQ.Domain.ServiceAggregate.ValueObjects;
using MediatR;

namespace HQ.UseCases.Service.Commands.Request.ChangeStatusWaiting;

internal class ChangeRequestStatusWaitingCommandHandler : IRequestHandler<ChangeRequestStatusWaitingCommand, ErrorOr<Success>>
{
    private readonly IServiceRepository _serviceRepository;

    public ChangeRequestStatusWaitingCommandHandler(
        IServiceRepository serviceRepository
    )
    {
        _serviceRepository = serviceRepository;
    }

    public async Task<ErrorOr<Success>> Handle(ChangeRequestStatusWaitingCommand request, CancellationToken cancellationToken)
    {
        RequestId requestId = RequestId.Create(request.RequestId);
        ServiceAggregate? service = await _serviceRepository.GetServiceByRequest(requestId, cancellationToken);
        if (service is null)
            return Error.NotFound(description: "Данная услуга не найдена");

        ErrorOr<Success> result = service.SetWaiting(requestId);
        if (result.IsError)
            return result.Errors;

        await _serviceRepository.Update(service, cancellationToken);
        
        return Result.Success;
    }
}