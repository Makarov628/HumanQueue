
using ErrorOr;
using HQ.Application.Persistence;
using HQ.Domain.Common.ValueObjects;
using HQ.Domain.QueueAggregate;
using MediatR;

namespace HQ.UseCases.Queue.Commands.Create;

internal class CreateQueueCommandHandler : IRequestHandler<CreateQueueCommand, ErrorOr<Created>>
{
    private readonly IQueueRepository _queueRepository;

    public CreateQueueCommandHandler(IQueueRepository queueRepository)
    {
        _queueRepository = queueRepository;
    }

    public async Task<ErrorOr<Created>> Handle(CreateQueueCommand request, CancellationToken cancellationToken)
    {
        ErrorOr<Culture> culture = Culture.Create(request.Culture);
        if (culture.IsError)
            return culture.Errors;

        QueueAggregate queue = QueueAggregate.Create(request.Name, culture.Value);
        await _queueRepository.Add(queue, cancellationToken);
        return Result.Created;
    }
}
