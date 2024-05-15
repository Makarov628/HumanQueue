

using HQ.Domain.Common.Models;
using HQ.Domain.ServiceAggregate.Entities;

namespace HQ.Domain.ServiceAggregate.Events;

public record RequestStatusChangedEvent(Request request): IDomainEvent;
