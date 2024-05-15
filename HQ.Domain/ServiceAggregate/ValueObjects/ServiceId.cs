

using HQ.Domain.Common.Models;

namespace HQ.Domain.ServiceAggregate.ValueObjects;

public sealed class ServiceId : ValueObject
{
    public Guid Value { get; private set; }

    private ServiceId(Guid value)
    {
        Value = value;
    }

    public static ServiceId CreateUnique()
    {
        // TODO: enforce invariants
        return new ServiceId(Guid.NewGuid());
    }

    public static ServiceId Create(Guid value)
    {
        // TODO: enforce invariants
        return new ServiceId(value);
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}