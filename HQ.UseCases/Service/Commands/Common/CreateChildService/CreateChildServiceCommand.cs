


using ErrorOr;
using HQ.Domain.Common.ValueObjects;
using MediatR;

namespace HQ.UseCases.Service.Commands.Common.CreateChildService;

public record CreateChildServiceCommand(
    Guid ParentServiceId, 
    List<CultureString> Name, 
    string? Literal
): IRequest<ErrorOr<Created>>;