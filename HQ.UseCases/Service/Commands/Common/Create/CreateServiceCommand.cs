

using ErrorOr;
using HQ.Domain.Common.ValueObjects;
using HQ.Domain.ServiceAggregate;
using MediatR;

namespace HQ.UseCases.Service.Commands.Common.Create;

public record CreateServiceCommand(
    Guid QueueId, 
    List<CultureString> Name, 
    string? Literal
): IRequest<ErrorOr<Created>>;

