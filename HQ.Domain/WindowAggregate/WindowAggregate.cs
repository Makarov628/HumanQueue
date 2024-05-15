
using HQ.Domain.Common.Models;
using HQ.Domain.QueueAggregate.ValueObjects;
using HQ.Domain.ServiceAggregate.ValueObjects;
using HQ.Domain.UserAggregate.ValueObjects;
using HQ.Domain.WindowAggregate.ValueObjects;

namespace HQ.Domain.WindowAggregate;

public sealed class WindowAggregate : AggregateRoot<WindowId>
{
    // private readonly List<ServiceId> _servicesIds = new();

    public QueueId QueueId { get; private set; }
    public int Number { get; private set; }
    public UserId? AttachedUserId { get; private set; }

    // public IReadOnlyList<ServiceId> ServiceIds => _servicesIds.AsReadOnly();


    private WindowAggregate(
        WindowId windowId,
        QueueId queueId,
        int number,
        UserId? attachedUserId
    ): base(windowId)
    {
        QueueId = queueId;
        Number = number;
        AttachedUserId = attachedUserId;
    }

    public static WindowAggregate Create(QueueId queueId, int number, UserId? attachedUserId)
    {
        return new WindowAggregate(
            WindowId.CreateUnique(),
            queueId,
            number,
            attachedUserId
        );
    }

    public void AttachUser(UserId userId)
    {
        AttachedUserId = userId;
    }

    protected WindowAggregate() { }
}