

using MediatR;
using ErrorOr;
using HQ.Application.Persistence;
using HQ.Domain.TerminalAggregate.ValueObjects;
using HQ.Domain.TerminalAggregate;
using HQ.Domain.ServiceAggregate;
using HQ.Domain.Common.ValueObjects;

namespace HQ.UseCases.Terminal.Queries.GetTerminal;

internal class GetTerminalQueryHandler : IRequestHandler<GetTerminalQuery, ErrorOr<TerminalResponse>>
{
    private readonly ITerminalRepository _terminalRepository;
    private readonly IServiceRepository _serviceRepository;

    public GetTerminalQueryHandler(
        ITerminalRepository terminalRepository,
        IServiceRepository serviceRepository
    )
    {
        _terminalRepository = terminalRepository;
        _serviceRepository = serviceRepository;
    }

    public async Task<ErrorOr<TerminalResponse>> Handle(GetTerminalQuery request, CancellationToken cancellationToken)
    {
        TerminalId terminalId = TerminalId.Create(request.Id);
        TerminalAggregate? terminal = await _terminalRepository.GetTerminal(terminalId, cancellationToken);
        if (terminal is null)
            return Error.NotFound(description: "Данный терминал не найден");

        List<ServiceAggregate> services = await _serviceRepository.GetFlatServices(terminal.QueueId, cancellationToken);

        return new TerminalResponse(
            Id: terminal.Id.Value,
            Name: terminal.Name,
            AvailableCultures: AvailableCultures.GetCultures().Select(culture => new AvailableCultureResponse(
                culture.Name,
                culture.LanguageName
            )).ToList(),
            Services: services.ConvertAll(service => new TerminalServiceResponse(
                service.Id.Value,
                service.Name.StringParts,
                service.RequestNumberCounter,
                service.Literal?.Value,
                service.ParentId?.Value,
                new List<TerminalServiceResponse>()
            )).CreateTree()
        );
    }
}
