

using HQ.Domain.ServiceAggregate.Events;
using HQ.Web.Hubs;
using MediatR;
using Microsoft.AspNetCore.SignalR;

internal class RequestStatusChangedEventHandler : INotificationHandler<RequestStatusChangedEvent>
{

    private readonly IHubContext<HQHub> _hubContext;

    public RequestStatusChangedEventHandler(IHubContext<HQHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task Handle(RequestStatusChangedEvent notification, CancellationToken cancellationToken)
    {
        await _hubContext.Clients.All.SendAsync("request-status-changed", notification, cancellationToken);
    }
}
