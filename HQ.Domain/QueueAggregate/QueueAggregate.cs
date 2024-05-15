using HQ.Domain.Common.Models;
using HQ.Domain.Common.ValueObjects;
using HQ.Domain.QueueAggregate.ValueObjects;
using HQ.Domain.ServiceAggregate.ValueObjects;
using HQ.Domain.TerminalAggregate.ValueObjects;
using HQ.Domain.WindowAggregate.ValueObjects;

namespace HQ.Domain.QueueAggregate;

public sealed class QueueAggregate: AggregateRoot<QueueId>
{
    // private readonly List<ServiceId> _servicesIds = new();
    // private readonly List<TerminalId> _terminalIds = new();
    // private readonly List<WindowId> _windowIds = new();

    public string Name { get; private set; }
    public Culture DefaultCulture { get; private set; }
    
    // public IReadOnlyList<ServiceId> ServiceIds => _servicesIds.AsReadOnly();
    // public IReadOnlyList<TerminalId> TerminalIds => _terminalIds.AsReadOnly();
    // public IReadOnlyList<WindowId> WindowsIds => _windowIds.AsReadOnly();

    private QueueAggregate(
        QueueId queueId, 
        string name,
        Culture defaultCulture
    ): base(queueId)
    {
        this.Name = name;
        this.DefaultCulture = defaultCulture;
    }

    public static QueueAggregate Create(string name, Culture defaultCulture)
    {
        return new (
            QueueId.CreateUnique(),
            name,
            defaultCulture
        );
    }

    public void ChangeName(string name) 
    {
        Name = name;
    }

    public void ChangeDefaultCulture(Culture culture)
    {
        DefaultCulture = culture;
    }

    protected QueueAggregate() { }
}