using System;
using MediatR;

namespace EmailMarketing.Shared.Abstractions.Events;

public interface IDomainEvent : INotification
{
    DateTime OccurredOn { get; }
    Guid EventId { get; }
}