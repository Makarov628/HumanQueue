

using ErrorOr;
using MediatR;

namespace HQ.UseCases.Service.Queries.GetRequestsForTablo;

public record GetRequestsForTabloQuery(
    Guid QueueId
) : IRequest<ErrorOr<TabloResponse>>;

public record TabloResponse(
    List<RequestWaitingResponse> Waiting,
    List<RequestCalledResponse> Called
);

public record RequestWaitingResponse(
    Guid Id,
    string Number,
    string Culture,
    DateTime CreatedDate
);

public record RequestCalledResponse(
    Guid Id,
    string Number,
    string Culture,
    DateTime CreatedDate,
    TabloWindowResponse Window
);

public record TabloWindowResponse(
    Guid Id,
    int Number
);