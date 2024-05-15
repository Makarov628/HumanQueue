using Ardalis.SmartEnum;

namespace HQ.Domain.ServiceAggregate.Enums;

public abstract class RequestStatus : SmartEnum<RequestStatus>
{
    public RequestStatus(string name, int value) : base(name, value)
    {
    }

    public static readonly RequestStatus Created = new CreatedStatus();
    public static readonly RequestStatus Waiting = new WaitingStatus();
    public static readonly RequestStatus Called = new CalledStatus();
    public static readonly RequestStatus IsLost = new IsLostStatus();
    public static readonly RequestStatus WorkStarted = new WorkStartedStatus();
    public static readonly RequestStatus WorkEnded = new WorkEndedStatus();
    public static readonly RequestStatus Unworked = new UnworkedStatus();

    public abstract bool IsCanChangeTo(RequestStatus nextStatus);



    private sealed class CreatedStatus : RequestStatus
    {
        public CreatedStatus() : base("Created", 0) { }
        public override bool IsCanChangeTo(RequestStatus nextStatus)
        {
            return nextStatus == Waiting || nextStatus == Unworked; 
        }
    }

    private sealed class WaitingStatus : RequestStatus
    {
        public WaitingStatus() : base("Waiting", 1) { }
        public override bool IsCanChangeTo(RequestStatus nextStatus)
        {
            return nextStatus == Called || nextStatus == Unworked;
        }
    }

    private sealed class CalledStatus : RequestStatus
    {
        public CalledStatus(): base("Called", 2) {}
        public override bool IsCanChangeTo(RequestStatus nextStatus)
        {
            return nextStatus == Called || nextStatus == IsLost || nextStatus == WorkStarted || nextStatus == Unworked;
        }
    }

    private sealed class IsLostStatus: RequestStatus
    {
        public IsLostStatus(): base("IsLost", 3) {}
        public override bool IsCanChangeTo(RequestStatus nextStatus)
        {
            return false;
        }
    }

    private sealed class WorkStartedStatus: RequestStatus
    {
        public WorkStartedStatus(): base("WorkStarted", 4) {}
        public override bool IsCanChangeTo(RequestStatus nextStatus)
        {
            return nextStatus == WorkEnded;
        }
    }

    private sealed class WorkEndedStatus: RequestStatus
    {
        public WorkEndedStatus(): base("WorkEnded", 5) {}
        public override bool IsCanChangeTo(RequestStatus nextStatus)
        {
            return false;
        }
    }

    private sealed class UnworkedStatus: RequestStatus
    {
        public UnworkedStatus(): base("Unworked", 6) {}
        public override bool IsCanChangeTo(RequestStatus nextStatus)
        {
            return false;
        }
    }


}