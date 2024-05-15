
using HQ.Domain.Common.Models;

namespace HQ.Domain.TerminalAggregate.ValueObjects;

public sealed class TerminalId : ValueObject
{
    public Guid Value { get; private set; }

    private TerminalId(Guid value)
    {
        Value = value;
    }

    public static TerminalId CreateUnique()
    {
        // TODO: enforce invariants
        return new TerminalId(Guid.NewGuid());
    }

    public static TerminalId Create(Guid value)
    {
        // TODO: enforce invariants
        return new TerminalId(value);
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}