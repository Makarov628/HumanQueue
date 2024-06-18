

using HQ.Domain.Common.Models;
using HQ.Domain.ServiceAggregate.ValueObjects;
using HQ.Domain.WindowAggregate.ValueObjects;

namespace HQ.Domain.ServiceAggregate.Entities;

public sealed class WindowLink : Entity<WindowLinkId>
{
    public ServiceId ServiceId { get; private set; }
    public WindowId WindowId { get; private set; }

    private WindowLink(WindowLinkId windowLinkId, ServiceId serviceId, WindowId windowId) : base(windowLinkId)
    {
        ServiceId = serviceId;
        WindowId = windowId;
    }

    public static WindowLink Create(ServiceId serviceId, WindowId windowId)
    {
        return new(
            WindowLinkId.CreateUnique(),
            serviceId,
            windowId
        );
    }

    protected WindowLink() { }
}