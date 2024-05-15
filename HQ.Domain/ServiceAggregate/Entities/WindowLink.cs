

using HQ.Domain.Common.Models;
using HQ.Domain.ServiceAggregate.ValueObjects;
using HQ.Domain.WindowAggregate.ValueObjects;

namespace HQ.Domain.ServiceAggregate.Entities;

public sealed class WindowLink : Entity<ServiceId>
{
    public WindowId WindowId { get; private set; }

    private WindowLink(ServiceId serviceId, WindowId windowId) : base(serviceId)
    {
        WindowId = windowId;
    }

    public static WindowLink Create(ServiceId serviceId, WindowId windowId)
    {
        return new(
            serviceId,
            windowId
        );
    }

    protected WindowLink() { }
}