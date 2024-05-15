using HQ.Domain.Common.Models;

namespace HQ.Domain.QueueAggregate.ValueObjects;

public sealed class QueueId : ValueObject
{
    public Guid Value { get; private set; }

    private QueueId(Guid value)
    {
        Value = value;
    }

    public static QueueId CreateUnique()
    {
        // TODO: enforce invariants
        return new QueueId(Guid.NewGuid());
    }

    public static QueueId Create(Guid value)
    {
        // TODO: enforce invariants
        return new QueueId(value);
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}