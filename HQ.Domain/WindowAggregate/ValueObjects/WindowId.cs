

using HQ.Domain.Common.Models;

namespace HQ.Domain.WindowAggregate.ValueObjects;

public sealed class WindowId : ValueObject
{
    public Guid Value { get; private set; }

    private WindowId(Guid value)
    {
        Value = value;
    }

    public static WindowId CreateUnique()
    {
        // TODO: enforce invariants
        return new WindowId(Guid.NewGuid());
    }

    public static WindowId Create(Guid value)
    {
        // TODO: enforce invariants
        return new WindowId(value);
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}