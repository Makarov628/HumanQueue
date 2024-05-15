


using ErrorOr;
using HQ.Application.Persistence;
using HQ.Domain.ServiceAggregate;
using HQ.Domain.ServiceAggregate.ValueObjects;
using MediatR;

namespace HQ.UseCases.Window.Commands.ChangeStatusWorkStarted;

internal class ChangeRequestStatusWorkStartedCommandHandler : IRequestHandler<ChangeRequestStatusWorkStartedCommand, ErrorOr<Success>>
{
    private readonly IServiceRepository _serviceRepository;

    public ChangeRequestStatusWorkStartedCommandHandler(
        IServiceRepository serviceRepository
    )
    {
        _serviceRepository = serviceRepository;
    }

    public async Task<ErrorOr<Success>> Handle(ChangeRequestStatusWorkStartedCommand request, CancellationToken cancellationToken)
    {
        RequestId requestId = RequestId.Create(request.RequestId);
        ServiceAggregate? service = await _serviceRepository.GetServiceByRequest(requestId, cancellationToken);
        if (service is null)
            return Error.NotFound(description: "Данная услуга не найдена");

        ErrorOr<Success> result = service.SetWorkStarted(requestId);
        if (result.IsError)
            return result.Errors;

        await _serviceRepository.Update(service, cancellationToken);
        
        return Result.Success;
    }
}