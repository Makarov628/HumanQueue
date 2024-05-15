

using HQ.Domain.Common.Models;

namespace HQ.Domain.ServiceAggregate.ValueObjects;

public sealed class RequestId : ValueObject
{
    public Guid Value { get; private set; }

    private RequestId(Guid value)
    {
        Value = value;
    }

    public static RequestId CreateUnique()
    {
        // TODO: enforce invariants
        return new RequestId(Guid.NewGuid());
    }

    public static RequestId Create(Guid value)
    {
        // TODO: enforce invariants
        return new RequestId(value);
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}