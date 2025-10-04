using System;

namespace EmailMarketing.Common.Services
{
    public interface ICurrentUserService
    {
        Guid UserId { get; }
        bool IsAuthenticated { get; }
    }
}
