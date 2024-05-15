using ErrorOr;
using HQ.Application.Persistence;
using HQ.Domain.QueueAggregate;
using MediatR;

namespace HQ.UseCases.Queue.Queries.GetQueues;

internal class GetQueueQueryHandler : IRequestHandler<GetQueuesQuery, ErrorOr<List<QueuesResponse>>>
{
    private readonly IQueueRepository _queueRepository;

    public GetQueueQueryHandler(IQueueRepository queueRepository)
    {
        _queueRepository = queueRepository;
    }

    public async Task<ErrorOr<List<QueuesResponse>>> Handle(GetQueuesQuery request, CancellationToken cancellationToken)
    {
        List<QueueAggregate> queues = await _queueRepository.GetAll(cancellationToken);
        return queues.ConvertAll(queue => new QueuesResponse(
            queue.Id.Value,
            queue.Name
        ));
    }
}
