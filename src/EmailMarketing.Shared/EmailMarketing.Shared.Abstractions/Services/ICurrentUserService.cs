using System;

namespace EmailMarketing.Shared.Abstractions.Services;

public interface ICurrentUserService
{
    Guid UserId { get; }
    bool IsAuthenticated { get; }
}
