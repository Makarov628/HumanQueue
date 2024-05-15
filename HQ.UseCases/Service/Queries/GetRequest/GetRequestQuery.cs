

using ErrorOr;
using HQ.Domain.ServiceAggregate.Enums;
using MediatR;

namespace HQ.UseCases.Service.Queries.GetRequest;

public record GetRequestQuery(
    Guid Id
): IRequest<ErrorOr<RequestResponse>>;

public record RequestResponse(
    Guid Id,
    string Number,
    string ServiceName,
    string Culture,
    DateTime CreatedDate,
    RequestStatus Status,
    int? WindowNumber,
    bool IsFirstView
);