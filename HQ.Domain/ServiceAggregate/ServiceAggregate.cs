using ErrorOr;
using HQ.Domain.Common.Models;
using HQ.Domain.Common.ValueObjects;
using HQ.Domain.QueueAggregate.ValueObjects;
using HQ.Domain.ServiceAggregate.Entities;
using HQ.Domain.ServiceAggregate.Events;
using HQ.Domain.ServiceAggregate.ValueObjects;
using HQ.Domain.TerminalAggregate.ValueObjects;
using HQ.Domain.WindowAggregate.ValueObjects;

namespace HQ.Domain.ServiceAggregate;

public sealed class ServiceAggregate : AggregateRoot<ServiceId>
{
    private readonly List<Request> _requests = new();
    private readonly List<WindowLink> _windowLinks = new();

    public QueueId QueueId { get; private set; }
    public LocalizedString Name { get; private set; }
    public int RequestNumberCounter { get; private set; }

    public ServiceLiteral? Literal { get; private set; }
    public ServiceId? ParentId { get; private set; }

    public IReadOnlyList<Request> Requests => _requests.AsReadOnly();
    public IReadOnlyList<WindowLink> WindowLinks => _windowLinks.AsReadOnly();

    public List<Guid> LinkedWindowIds => _windowLinks.Select(link => link.WindowId.Value).ToList();

    private ServiceAggregate(
        ServiceId serviceId,
        QueueId queueId,
        LocalizedString name,
        int requestNumberCounter,
        ServiceLiteral? literal,
        ServiceId? parentId
    ) : base(serviceId)
    {
        this.QueueId = queueId;
        this.Name = name;
        this.RequestNumberCounter = requestNumberCounter;
        this.Literal = literal;
        this.ParentId = parentId;
    }

    public static ServiceAggregate Create(QueueId queueId, LocalizedString name, ServiceLiteral? literal)
    {
        return new(
            ServiceId.CreateUnique(),
            queueId,
            name,
            requestNumberCounter: 0,
            literal,
            parentId: null
        );
    }

    public ErrorOr<ServiceAggregate> CreateChild(LocalizedString name, ServiceLiteral? literal)
    {
        if (!IsCanCreateChildService())
            return Error.Validation(description: "Данная услуга не может включать дочерние услуги.");

        return new ServiceAggregate(
            ServiceId.CreateUnique(),
            this.QueueId,
            name,
            requestNumberCounter: 0,
            literal,
            parentId: this.Id
        );
    }

    public bool IsCanAddWindowLinks()
    {
        return Literal is not null;
    }

    public bool IsCanAddRequests()
    {
        return Literal is not null;
    }

    public bool IsCanCreateChildService()
    {
        return Literal is null;
    }

    public bool IsWindowLinkExists(WindowId windowId)
    {
        return _windowLinks.Any(windowLink => windowLink.WindowId == windowId);
    }

    public void UpdateName(LocalizedString name)
    {
        this.Name = name;
    }

    public void UpdateLiteral(ServiceLiteral newLiteral)
    {
        this.Literal = newLiteral;
    }

    public void ResetRequestNumberCounter()
    {
        RequestNumberCounter = 0;
    }

    public void MarkNotFinishedRequestsAsUnworked()
    {
        var readyForUnworkedRequests = _requests.Where(request => request.IsReadyForUnworked());
        foreach (var request in readyForUnworkedRequests)
        {
            request.SetUnworked();
        }

        if (readyForUnworkedRequests.Any())
        {
            // add event with unworked requests
        }
    }

    public ErrorOr<Request> AddNewRequest(TerminalId fromTerminalId, Culture requestCulture)
    {
        if (!IsCanAddRequests())
            return Error.Validation(description: "К данной услуге нельзя добавлять заявки.");

        RequestNumberCounter += 1;

        var request = Request.Create(
            forServiceId: Id,
            RequestNumber.Create(Literal!, RequestNumberCounter),
            requestCulture,
            fromTerminalId
        );
        _requests.Add(request);

        // TODO: add event about new request

        return request;
    }

    public ErrorOr<Created> AddWindowLink(WindowId windowId)
    {
        if (!IsCanAddWindowLinks())
            return Error.Validation(description: "К данной услуге нельзя привязать окно.");

        if (IsWindowLinkExists(windowId))
            return Error.Validation(description: "Данное окно уже привязано к услуге.");

        _windowLinks.Add(WindowLink.Create(this.Id, windowId));

        // TODO: add event about new windowLink

        return Result.Created;
    }

    public void RemoveWindowLink(WindowId windowId)
    {
        if (IsWindowLinkExists(windowId))
        {
            _windowLinks.RemoveAll(windowLink => windowLink.WindowId == windowId);
            // TODO: add event about delete windowLink
        }
    }

    private ErrorOr<Request> GetRequest(RequestId requestId)
    {
        Request? request = _requests.FirstOrDefault(request => request.Id == requestId);
        if (request is null)
            return Error.NotFound(description: "Данная заявка для этой услуги не найдена.");

        return request;
    }

    private ErrorOr<WindowLink> GetWindowLink(WindowId windowId)
    {
        WindowLink? windowLink = _windowLinks.FirstOrDefault(windowLink => windowLink.WindowId == windowId);
        if (windowLink is null)
            return Error.NotFound(description: "Данное окно не закреплено за этой услугой.");

        return windowLink;
    }

    public ErrorOr<Success> SetWaiting(RequestId requestId)
    {
        ErrorOr<Request> request = GetRequest(requestId);
        if (request.IsError)
            return request.Errors;

        ErrorOr<Success> result = request.Value.SetWaiting();
        if (result.IsError)
            return result.Errors;

        // add event about new status
        AddDomainEvent(new RequestStatusChangedEvent(request.Value));

        return Result.Success;
    }

    public ErrorOr<Success> SetCalled(RequestId requestId, WindowId calledByWindowId)
    {
        ErrorOr<WindowLink> windowLink = GetWindowLink(calledByWindowId);
        if (windowLink.IsError)
            return windowLink.Errors;

        ErrorOr<Request> request = GetRequest(requestId);
        if (request.IsError)
            return request.Errors;

        ErrorOr<Success> result = request.Value.SetCalled(windowLink.Value);
        if (result.IsError)
            return result;

        // add event about new status
        AddDomainEvent(new RequestStatusChangedEvent(request.Value));

        return Result.Success;
    }

    public ErrorOr<Success> SetIsLost(RequestId requestId)
    {
        ErrorOr<Request> request = GetRequest(requestId);
        if (request.IsError)
            return request.Errors;

        ErrorOr<Success> result = request.Value.SetIsLost();
        if (result.IsError)
            return result.Errors;

        // add event about new status
        AddDomainEvent(new RequestStatusChangedEvent(request.Value));

        return Result.Success;
    }

    public ErrorOr<Success> SetWorkStarted(RequestId requestId)
    {
        ErrorOr<Request> request = GetRequest(requestId);
        if (request.IsError)
            return request.Errors;

        ErrorOr<Success> result = request.Value.SetWorkStarted();
        if (result.IsError)
            return result.Errors;

        // add event about new status
        AddDomainEvent(new RequestStatusChangedEvent(request.Value));

        return Result.Success;
    }

    public ErrorOr<Success> SetWorkEnded(RequestId requestId)
    {
        ErrorOr<Request> request = GetRequest(requestId);
        if (request.IsError)
            return request.Errors;

        ErrorOr<Success> result = request.Value.SetWorkEnded();
        if (result.IsError)
            return result.Errors;

        // add event about new status
        AddDomainEvent(new RequestStatusChangedEvent(request.Value));

        return Result.Success;
    }

    protected ServiceAggregate() { }
}