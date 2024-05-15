

using ErrorOr;
using HQ.Application.Persistence;
using HQ.Application.Printer;
using HQ.Domain.ServiceAggregate;
using HQ.Domain.ServiceAggregate.Entities;
using HQ.Domain.ServiceAggregate.ValueObjects;
using HQ.Domain.TerminalAggregate;
using HQ.Domain.TerminalAggregate.ValueObjects;
using MediatR;

namespace HQ.UseCases.Terminal.Commands.PrintRequest;

internal class TerminalPrintRequestCommandHandler : IRequestHandler<TerminalPrintRequestCommand, ErrorOr<Success>>
{
    private readonly ITerminalRepository _terminalRepository;
    private readonly IServiceRepository _serviceRepository;
    private readonly IExternalPrinterProvider _externalPrinterProvider;

    public TerminalPrintRequestCommandHandler(
        ITerminalRepository terminalRepository,
        IServiceRepository serviceRepository,
        IExternalPrinterProvider externalPrinterProvider
    )
    {
        _terminalRepository = terminalRepository;
        _serviceRepository = serviceRepository;
        _externalPrinterProvider = externalPrinterProvider;
    }

    public async Task<ErrorOr<Success>> Handle(TerminalPrintRequestCommand request, CancellationToken cancellationToken)
    {
        // Ищем нужный терминал
        TerminalId terminalId = TerminalId.Create(request.TerminalId);
        TerminalAggregate? terminal = await _terminalRepository.GetTerminal(terminalId, cancellationToken);
        if (terminal is null)
            return Error.NotFound(description: "Данный терминал не найден");

        // Если внешний принтер не подключен, уходим
        if (terminal.ExternalPrinterId is null)
            return Error.Failure(description: "К данному терминалу не привязан внешний принтер");

        // Извлекаем услугу и ее запросы
        RequestId requestId = RequestId.Create(request.RequestId);
        ServiceAggregate? service = await _serviceRepository.GetServiceByRequest(requestId, cancellationToken);
        if (service is null)
            return Error.NotFound(description: "Данная услуга или запрос не найдены");

        // Назначаем статус "В ожидании" запросу
        ErrorOr<Success> requestResult = service.SetWaiting(requestId);
        if (requestResult.IsError)
            return requestResult.Errors;

        // Готовим запрос для печати
        Request serviceRequest = service.Requests.First(req => req.Id == requestId);
        RequestForPrint requestForPrint = new(
            serviceRequest.Id.Value,
            serviceRequest.Number.ToString(),
            service.Name.GetValueByCulture(serviceRequest.Culture) ?? "[No service name]",
            serviceRequest.CreatedAt
        );

        // Отправляем на печать
        ErrorOr<Success> printerResult = await _externalPrinterProvider.Print(
            terminal.ExternalPrinterId!, 
            requestForPrint,
            cancellationToken
        );
        if (printerResult.IsError)
            return printerResult.Errors;

        // Сохраняем услугу и ее измененный запрос
        await _serviceRepository.Update(service, cancellationToken);
        return Result.Success;
    }
}