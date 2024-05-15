
using ErrorOr;
using HQ.Application.Persistence;
using HQ.Domain.ServiceAggregate;
using HQ.Domain.ServiceAggregate.Entities;
using HQ.Domain.ServiceAggregate.ValueObjects;
using HQ.Domain.WindowAggregate;
using MediatR;

namespace HQ.UseCases.Service.Queries.GetRequest;

internal class GetRequestQueryHandler : IRequestHandler<GetRequestQuery, ErrorOr<RequestResponse>>
{
    private readonly IServiceRepository _serviceRepository;
    private readonly IWindowRepository _windowRepository;

    public GetRequestQueryHandler(
        IServiceRepository serviceRepository, 
        IWindowRepository windowRepository
    )
    {
        _serviceRepository = serviceRepository;
        _windowRepository = windowRepository;
    }

    public async Task<ErrorOr<RequestResponse>> Handle(GetRequestQuery request, CancellationToken cancellationToken)
    {
        RequestId requestId = RequestId.Create(request.Id);
        ServiceAggregate? service = await _serviceRepository.GetServiceByRequest(requestId, cancellationToken);
        if (service is null)
            return Error.NotFound(description: "Услуга или запрос не найдены");

        Request serviceRequest = service.Requests.First(req => req.Id == requestId);

        int? windowNumber = null;
        if (serviceRequest.CalledByWindowId is not null)
        {
            WindowAggregate? window = await _windowRepository.GetById(serviceRequest.CalledByWindowId, cancellationToken);
            windowNumber = window?.Number;
        }

        return new RequestResponse(
            Id: serviceRequest.Id.Value,
            Number: serviceRequest.Number.ToString(),
            ServiceName: service.Name.GetValueByCulture(serviceRequest.Culture) ?? "[No service name]",
            Culture: serviceRequest.Culture.Name,
            CreatedDate: serviceRequest.CreatedAt,
            Status: serviceRequest.Status,
            WindowNumber: windowNumber,
            IsFirstView: serviceRequest.IsCreated()
        );
    }
}