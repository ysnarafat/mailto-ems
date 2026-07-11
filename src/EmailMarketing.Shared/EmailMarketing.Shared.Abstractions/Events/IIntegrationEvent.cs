using System;
using MediatR;

namespace EmailMarketing.Shared.Abstractions.Events;

public interface IIntegrationEvent : INotification
{
    DateTime OccurredOn { get; }
    Guid EventId { get; }
    string EventType { get; }
}