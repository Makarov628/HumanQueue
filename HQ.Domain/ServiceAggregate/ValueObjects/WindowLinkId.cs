

using HQ.Domain.Common.Models;

namespace HQ.Domain.ServiceAggregate.ValueObjects;

public sealed class WindowLinkId : ValueObject
{
    public Guid Value { get; private set; }

    private WindowLinkId(Guid value)
    {
        Value = value;
    }

    public static WindowLinkId CreateUnique()
    {
        // TODO: enforce invariants
        return new WindowLinkId(Guid.NewGuid());
    }

    public static WindowLinkId Create(Guid value)
    {
        // TODO: enforce invariants
        return new WindowLinkId(value);
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}