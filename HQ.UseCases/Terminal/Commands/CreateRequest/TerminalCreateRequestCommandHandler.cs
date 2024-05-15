using ErrorOr;
using HQ.Application.Persistence;
using HQ.Domain.Common.ValueObjects;
using HQ.Domain.ServiceAggregate;
using HQ.Domain.ServiceAggregate.ValueObjects;
using HQ.Domain.TerminalAggregate.ValueObjects;
using MediatR;

namespace HQ.UseCases.Terminal.Commands.CreateRequest;

internal class TerminalCreateRequestCommandHandler : IRequestHandler<TerminalCreateRequestCommand, ErrorOr<TerminalRequestResponse>>
{
    private readonly ITerminalRepository _terminalRepository;
    private readonly IServiceRepository _serviceRepository;

    public TerminalCreateRequestCommandHandler(
        ITerminalRepository terminalRepository,
        IServiceRepository serviceRepository
    )
    {
        _terminalRepository = terminalRepository;
        _serviceRepository = serviceRepository;
    }

    public async Task<ErrorOr<TerminalRequestResponse>> Handle(TerminalCreateRequestCommand request, CancellationToken cancellationToken)
    {
        ServiceId serviceId = ServiceId.Create(request.ToServiceId);
        ServiceAggregate? service = await _serviceRepository.GetService(serviceId, cancellationToken);
        if (service is null)
            return Error.NotFound(description: "Данная услуга не найдена");

        TerminalId terminalId = TerminalId.Create(request.FromTerminalId);
        bool terminalIsExists = await _terminalRepository.IsExists(terminalId, cancellationToken);
        if (!terminalIsExists)
            return Error.NotFound(description: "Данный терминал не найден");

        ErrorOr<Culture> culture = Culture.Create(request.Culture);
        if (culture.IsError)
            return culture.Errors;

        var newRequest = service.AddNewRequest(terminalId, culture.Value);
        if (newRequest.IsError)
            return newRequest.Errors;

        await _serviceRepository.Update(service, cancellationToken);

        return new TerminalRequestResponse(
            newRequest.Value.Id.Value,
            newRequest.Value.Number.ToString(),
            service.Name.GetValueByCulture(newRequest.Value.Culture) ?? "[No service name]",
            newRequest.Value.Culture.Name,
            newRequest.Value.CreatedAt
        );
    }
}