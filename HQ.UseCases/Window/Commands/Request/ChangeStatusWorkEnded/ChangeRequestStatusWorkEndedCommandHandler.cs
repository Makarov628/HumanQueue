




using ErrorOr;
using HQ.Application.Persistence;
using HQ.Domain.ServiceAggregate;
using HQ.Domain.ServiceAggregate.ValueObjects;
using MediatR;

namespace HQ.UseCases.Window.Commands.ChangeStatusWorkEnded;

internal class ChangeRequestStatusWorkEndedCommandHandler : IRequestHandler<ChangeRequestStatusWorkEndedCommand, ErrorOr<Success>>
{
    private readonly IServiceRepository _serviceRepository;

    public ChangeRequestStatusWorkEndedCommandHandler(
        IServiceRepository serviceRepository
    )
    {
        _serviceRepository = serviceRepository;
    }

    public async Task<ErrorOr<Success>> Handle(ChangeRequestStatusWorkEndedCommand request, CancellationToken cancellationToken)
    {
        RequestId requestId = RequestId.Create(request.RequestId);
        ServiceAggregate? service = await _serviceRepository.GetServiceByRequest(requestId, cancellationToken);
        if (service is null)
            return Error.NotFound(description: "Данная услуга не найдена");

        ErrorOr<Success> result = service.SetWorkEnded(requestId);
        if (result.IsError)
            return result.Errors;

        await _serviceRepository.Update(service, cancellationToken);
        
        return Result.Success;
    }
}