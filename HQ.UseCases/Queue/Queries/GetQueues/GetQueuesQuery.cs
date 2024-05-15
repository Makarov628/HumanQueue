using ErrorOr;
using MediatR;

namespace HQ.UseCases.Queue.Queries.GetQueues;

public record GetQueuesQuery() : IRequest<ErrorOr<List<QueuesResponse>>>;

public record QueuesResponse(
    Guid Id,
    string Name
);