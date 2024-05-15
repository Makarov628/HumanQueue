

using ErrorOr;
using HQ.Domain.Common.Models;
using HQ.Domain.Common.ValueObjects;
using HQ.Domain.ServiceAggregate.Enums;
using HQ.Domain.ServiceAggregate.ValueObjects;
using HQ.Domain.TerminalAggregate.ValueObjects;
using HQ.Domain.WindowAggregate.ValueObjects;

namespace HQ.Domain.ServiceAggregate.Entities;

public sealed class Request: Entity<RequestId>
{
    public ServiceId CreatedForServiceId { get; private set; }
    public RequestNumber Number { get; private set; }
    public RequestStatus Status { get; private set; }
    public Culture Culture { get; private set; }
    public TerminalId CreatedFromTerminalId { get; private set; }    
    public WindowId? CalledByWindowId { get; private set; }


    private Request(
        RequestId requestId, 
        ServiceId createdForServiceId,
        RequestNumber number, 
        RequestStatus status,
        Culture culture,
        TerminalId createdFromTerminalId,
        WindowId? calledByWindowId
    ): base(requestId)
    {
        CreatedForServiceId = createdForServiceId;
        Number = number;
        Status = status;
        Culture = culture;
        CreatedFromTerminalId = createdFromTerminalId;
        CalledByWindowId = calledByWindowId;
    }

    public static Request Create(
        ServiceId forServiceId, 
        RequestNumber number, 
        Culture culture,
        TerminalId byTerminalId
    )
    {
        return new(
            RequestId.CreateUnique(),
            forServiceId,
            number,
            RequestStatus.Created,
            culture,
            byTerminalId,
            calledByWindowId: null
        );
    }

    public bool IsCreated()
    {
        return Status == RequestStatus.Created;
    }

    public bool IsWaiting()
    {
        return Status == RequestStatus.Waiting;
    }

    public bool IsCalled()
    {
        return Status == RequestStatus.Called;
    }

    public bool IsWorkStarted()
    {
        return Status == RequestStatus.WorkStarted;
    }

    public ErrorOr<Success> SetWaiting()
    {
        return SetStatus(RequestStatus.Waiting);
    }

    public ErrorOr<Success> SetCalled(WindowLink windowLink)
    {
        ErrorOr<Success> result = SetStatus(RequestStatus.Called);
        if (result.IsError)
            return result.Errors;

        if (CalledByWindowId is not null && CalledByWindowId != windowLink.WindowId)
            return Error.Validation(description: "Заявка обрабатывается другим окном.");

        CalledByWindowId = windowLink.WindowId; 

        return Result.Success;   
    }

    public ErrorOr<Success> SetIsLost()
    {
        return SetStatus(RequestStatus.IsLost);
    }

    public ErrorOr<Success> SetWorkStarted()
    {
        return SetStatus(RequestStatus.WorkStarted);
    }

    public ErrorOr<Success> SetWorkEnded()
    { 
        return SetStatus(RequestStatus.WorkEnded);
    }

    public ErrorOr<Success> SetUnworked()
    {
        return SetStatus(RequestStatus.Unworked);
    }

    private ErrorOr<Success> SetStatus(RequestStatus newStatus) 
    {
        if (!Status.IsCanChangeTo(newStatus))
            return Error.Validation(description: $"Статус заявки не может быть изменен с '{Status.Value}' на '{newStatus.Value}'.");

        Status = newStatus;

        return Result.Success;
    }

    public bool IsCanBeCalled()
    {
        return Status.IsCanChangeTo(RequestStatus.Called);
    }

    public bool IsCannotBeCalled()
    {
        return !IsCanBeCalled();
    }


    public bool IsReadyForUnworked()
    {
        return Status.IsCanChangeTo(RequestStatus.Unworked);
    }

   

    protected Request() { }
}