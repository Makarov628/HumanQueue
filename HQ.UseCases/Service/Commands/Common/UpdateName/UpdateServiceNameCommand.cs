



using ErrorOr;
using HQ.Domain.Common.ValueObjects;
using MediatR;

namespace HQ.UseCases.Service.Commands.Common.UpdateName;

public record UpdateServiceNameCommand(
    Guid Id,
    List<CultureString> Name
): IRequest<ErrorOr<Success>>;