using System;

namespace EmailMarketing.Shared.Abstractions.Services;

public interface IDateTime
{
    DateTime Now { get; }
}
