using MediatR;
using ErrorOr;
using HQ.Domain.Common.ValueObjects;
using HQ.Domain.ServiceAggregate.Enums;

namespace HQ.UseCases.Window.Queries.GetWindow;

public record GetWindowQuery(
    Guid Id
): IRequest<ErrorOr<WindowResponse>>;

public record WindowResponse(
    Guid Id,
    int Number,
    List<AttachedService> AttachedServices,
    CurrentRequestResponse? CurrentRequest,
    List<WaitingRequestResponse> WaitingRequests
);

public record AttachedService(
    Guid Id,
    List<CultureString> Name
);

public record CurrentRequestResponse(
    Guid Id,
    string Number,
    string ServiceName,
    string Culture,
    RequestStatus Status,
    DateTime CreatedDate
);

public record WaitingRequestResponse(
    Guid Id,
    string Number,
    string ServiceName,
    string Culture,
    DateTime CreatedDate
);



