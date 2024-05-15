
using ErrorOr;
using HQ.Application.Persistence;
using HQ.Domain.Common.ValueObjects;
using HQ.Domain.QueueAggregate;
using HQ.Domain.QueueAggregate.ValueObjects;
using HQ.Domain.ServiceAggregate;
using HQ.Domain.ServiceAggregate.ValueObjects;
using MediatR;

namespace HQ.UseCases.Service.Commands.Common.Create;

internal class CreateServiceCommandHandler : IRequestHandler<CreateServiceCommand, ErrorOr<Created>>
{
    private readonly IQueueRepository _queueRepository;
    private readonly IServiceRepository _serviceRepository;

    public CreateServiceCommandHandler(
        IQueueRepository queueRepository,
        IServiceRepository serviceRepository
    )
    {
        _queueRepository = queueRepository;
        _serviceRepository = serviceRepository;
    }

    public async Task<ErrorOr<Created>> Handle(CreateServiceCommand request, CancellationToken cancellationToken)
    {

        // Проверяем наличие очереди
        QueueId queueId = QueueId.Create(request.QueueId);
        QueueAggregate? queue = await _queueRepository.GetById(queueId, cancellationToken);
        if (queue is null)
            return Error.NotFound(description: "Данной очереди не существует");

        // Создаем локализируемую строку
        ErrorOr<LocalizedString> serviceName = LocalizedString.Create(request.Name);
        if (serviceName.IsError)
            return serviceName.Errors;

        // Если литерал не был прислан, сохраняем услугу
        if (string.IsNullOrEmpty(request.Literal))
        {
            ServiceAggregate serviceWithoutLiteral = ServiceAggregate.Create(queueId, serviceName.Value, null);
            await _serviceRepository.Add(serviceWithoutLiteral, cancellationToken);
            return Result.Created;
        }

        // Создаем литерал
        ErrorOr<ServiceLiteral> literal = ServiceLiteral.Create(request.Literal);
        if (literal.IsError)
            return literal.Errors;

        // Проверяем что услуги с таким литералом нет
        bool isExistsLiteral = await _serviceRepository.IsLiteralExists(queueId, literal.Value, cancellationToken);
        if (isExistsLiteral)
            return Error.Validation(description: "Услуга с таким литералом уже существует.");

        // Сохраняем услугу
        ServiceAggregate service = ServiceAggregate.Create(queueId, serviceName.Value, null);
        await _serviceRepository.Add(service, cancellationToken);
        return Result.Created;
    }
}
