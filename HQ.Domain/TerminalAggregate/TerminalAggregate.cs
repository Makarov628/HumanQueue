

using HQ.Domain.Common.Models;
using HQ.Domain.QueueAggregate.ValueObjects;
using HQ.Domain.TerminalAggregate.ValueObjects;

namespace HQ.Domain.TerminalAggregate;

public sealed class TerminalAggregate : AggregateRoot<TerminalId>
{
    public QueueId QueueId { get; private set; }
    public string Name { get; private set; }
    public string? ExternalPrinterId { get; private set; }

    private TerminalAggregate(
        TerminalId terminalId,
        QueueId queueId,
        string name,
        string? externalPrinterId
    ): base(terminalId)
    {
        QueueId = queueId;
        Name = name;
        ExternalPrinterId = externalPrinterId;
    }

    public static TerminalAggregate Create(QueueId queueId, string name, string? externalPrinterId)
    {
        return new TerminalAggregate(
            TerminalId.CreateUnique(),
            queueId, 
            name, 
            externalPrinterId
        );
    }

    public void SetName(string name)
    {
        Name = name;
    }

    public void SetExternalPrinterId(string? externalPrinterId)
    {
        ExternalPrinterId = externalPrinterId;
    }

    protected TerminalAggregate() { }
}