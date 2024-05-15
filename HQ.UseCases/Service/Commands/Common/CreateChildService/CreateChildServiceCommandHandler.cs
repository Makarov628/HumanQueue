

using ErrorOr;
using HQ.Application.Persistence;
using HQ.Domain.Common.ValueObjects;
using HQ.Domain.ServiceAggregate;
using HQ.Domain.ServiceAggregate.ValueObjects;
using MediatR;

namespace HQ.UseCases.Service.Commands.Common.CreateChildService;

internal class CreateChildServiceCommandHandler : IRequestHandler<CreateChildServiceCommand, ErrorOr<Created>>
{
    private readonly IServiceRepository _serviceRepository;

    public CreateChildServiceCommandHandler(IServiceRepository serviceRepository)
    {
        _serviceRepository = serviceRepository;
    }

    public async Task<ErrorOr<Created>> Handle(CreateChildServiceCommand request, CancellationToken cancellationToken)
    {

        // Проверяем что родительская услуга существует
        ServiceId parentId = ServiceId.Create(request.ParentServiceId);
        ServiceAggregate? parentService = await _serviceRepository.GetService(parentId, cancellationToken); 
        if (parentService is null)
            return Error.NotFound(description: "Данная услуга не найдена");

        // Создаем локализируемую строку
        ErrorOr<LocalizedString> serviceName = LocalizedString.Create(request.Name);
        if (serviceName.IsError)
            return serviceName.Errors;


        // Если литерал не был прислан, сохраняем услугу
        if (string.IsNullOrEmpty(request.Literal))
        {
            ErrorOr<ServiceAggregate> childServiceWithoutLiteral = parentService.CreateChild(serviceName.Value, null);
            if (childServiceWithoutLiteral.IsError)
                return childServiceWithoutLiteral.Errors;

            await _serviceRepository.Add(childServiceWithoutLiteral.Value, cancellationToken);
            return Result.Created;
        }

        // Создаем литерал
        ErrorOr<ServiceLiteral> literal = ServiceLiteral.Create(request.Literal);
        if (literal.IsError)
            return literal.Errors;

        // Проверяем что услуги с таким литералом нет
        bool isExistsLiteral = await _serviceRepository.IsLiteralExists(parentService.QueueId, literal.Value, cancellationToken);
        if (isExistsLiteral)
            return Error.Validation(description: "Услуга с таким литералом уже существует.");


        // Проверяем можно ли создать дочернюю услугу и сохраняем
        ErrorOr<ServiceAggregate> childService = parentService.CreateChild(serviceName.Value, null);
        if (childService.IsError)
            return childService.Errors;

        await _serviceRepository.Add(childService.Value, cancellationToken);
        return Result.Created;
    }
}
